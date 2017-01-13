using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace UnityLib.Network
{
    public class UDPListener
    {
        public UDPEndPoint host;
        public bool end { get; private set; }
        UdpClient client;

        public UDPListener(UDPEndPoint host)
        {
            this.host = host;
        }

        public void Begin()
        {
            new Thread(Listen).Start();
        }

        public void End()
        {
            end = true;
            client.Close();
        }

        void Listen()
        {
            Debug.Log("thread start");
            client = new UdpClient(host.listenPort);
            client.Client.ReceiveTimeout = 3000;

            while (!end)
            {
                try
                {
                    IPEndPoint endpoint = null;
                    byte[] bytes = client.Receive(ref endpoint);

                    var packetInfo = new PacketInfo();
                    packetInfo.endPoint = endpoint;
                    packetInfo.data = bytes;
                    host.QueuePacket(packetInfo);

                }
                catch (Exception e)
                {
                    //Debug.LogException(e);
                }
            }
            client.Close();
            Debug.Log("thread end");
        }
    }

    public class PacketInfo
    {
        public IPEndPoint endPoint;
        public byte[] data;
    }
}