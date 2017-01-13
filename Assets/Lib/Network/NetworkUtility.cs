using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using UnityEngine;

namespace UnityLib.Network
{
    public class NetworkUtility
    {
        public static void BoardcastToSubnet(string message, int port)
        {
            BoardcastToSubnet(Encoding.ASCII.GetBytes(message), port);
        }

        public static void BoardcastToSubnet(byte[] message, int port)
        {
            //        Console.WriteLine("Boardcast " + port);
            var s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress subnetMask = IPAddress.Parse("255.255.255.0");
            IPAddress broadcastAddress = GetLocalIPAddress().GetBroadcastAddress(subnetMask);

            var ep = new IPEndPoint(broadcastAddress, port);
            s.EnableBroadcast = true;
            s.SendTo(message, ep);
            //        Console.WriteLine("Boardcast done");
        }

        public static void BoardcastAll(byte[] message, int port)
        {
            var s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.EnableBroadcast = true;

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                foreach (var uipi in ni.GetIPProperties().UnicastAddresses)
                    if (uipi.IPv4Mask != null)
                    {
                        var ep = new IPEndPoint(uipi.Address.GetBroadcastAddress(uipi.IPv4Mask), port);
                        s.SendTo(message, ep);
                    }
        }

        public static void SendDgram(byte[] message, IPAddress address, int port)
        {
            var s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var ep = new IPEndPoint(address, port);
            s.EnableBroadcast = true;
            s.SendTo(message, ep);
        }

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;

            throw new Exception("Local IP Address Not Found!");
        }

        public static byte[] FrameMessage(string message)
        {
            return FrameBytes(Encoding.UTF8.GetBytes(message));
        }

        public static byte[] FrameBytes(byte[] msg)
        {
            var length = (uint)msg.Length;
            var frame = new byte[length + 4];
            byte[] header = BitConverter.GetBytes(length);
            header.CopyTo(frame, 0);
            msg.CopyTo(frame, 4);
            return frame;
        }        
    }

    public static class IPAddressExtensions
    {
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }

        public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }

        public static bool IsInSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
        {
            IPAddress network1 = address.GetNetworkAddress(subnetMask);
            IPAddress network2 = address2.GetNetworkAddress(subnetMask);

            return network1.Equals(network2);
        }

        public static void PrintNIInfo()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                Debug.Log("================================");
                Debug.Log(ni.Name + " " + ni.OperationalStatus.ToString());
                Debug.LogFormat("MAC: {0}", ni.GetPhysicalAddress());

                var gipis = ni.GetIPProperties().GatewayAddresses;

                if (gipis.Count > 0)
                {
                    Debug.Log("Gateways:");
                    foreach (var gipi in gipis)
                        Debug.LogFormat("\t{0}", gipi.Address);
                }

                var uipis = ni.GetIPProperties().UnicastAddresses;

                if (uipis.Count > 0)
                {
                    Debug.Log("IP Addresses:");
                    foreach (var uipi in uipis)
                    {
                        Debug.LogFormat("\t{0} / {1}", uipi.Address, uipi.IPv4Mask);
                        if (uipi.IPv4Mask != null)
                            Debug.Log("b: " + uipi.Address.GetBroadcastAddress(uipi.IPv4Mask));
                    }
                }
            }
        }

       
    }

    public static class SubnetMask
    {
        public static readonly IPAddress ClassA = IPAddress.Parse("255.0.0.0");
        public static readonly IPAddress ClassB = IPAddress.Parse("255.255.0.0");
        public static readonly IPAddress ClassC = IPAddress.Parse("255.255.255.0");

        public static IPAddress CreateByHostBitLength(int hostpartLength)
        {
            int hostPartLength = hostpartLength;
            int netPartLength = 32 - hostPartLength;

            if (netPartLength < 2)
                throw new ArgumentException("Number of hosts is to large for IPv4");

            Byte[] binaryMask = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                if (i * 8 + 8 <= netPartLength)
                    binaryMask[i] = (byte)255;
                else if (i * 8 > netPartLength)
                    binaryMask[i] = (byte)0;
                else
                {
                    int oneLength = netPartLength - i * 8;
                    string binaryDigit =
                        String.Empty.PadLeft(oneLength, '1').PadRight(8, '0');
                    binaryMask[i] = Convert.ToByte(binaryDigit, 2);
                }
            }
            return new IPAddress(binaryMask);
        }

        public static IPAddress CreateByNetBitLength(int netpartLength)
        {
            int hostPartLength = 32 - netpartLength;
            return CreateByHostBitLength(hostPartLength);
        }

        public static IPAddress CreateByHostNumber(int numberOfHosts)
        {
            int maxNumber = numberOfHosts + 1;

            string b = Convert.ToString(maxNumber, 2);

            return CreateByHostBitLength(b.Length);
        }
    }
}