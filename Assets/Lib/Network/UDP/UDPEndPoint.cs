using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.Collections.Generic;

namespace UnityLib.Network
{
    public class UDPEndPoint
    {
        public int listenPort;
        public bool isRunning { get { return listener != null; } }

        public IPEndPoint remote;

        public Thread thread;
        Queue<PacketInfo> received;

        object receiveLock;

        UDPListener listener;

        public UDPEndPoint()
        {
            received = new Queue<PacketInfo>();
            receiveLock = new object();
        }

        public void StartListener()
        {
            Debug.Log("start");
            listener = new UDPListener(this);
            listener.Begin();
        }

        public void StopListener()
        {
            Debug.Log("stop");
            if (listener != null)
                listener.End();
            listener = null;
        }

        public PacketInfo Receive()
        {
            lock (receiveLock)
            {
                if (received.Count > 0)
                    return received.Dequeue();
                else
                    return null;
            }
        }

        public List<PacketInfo> ReceiveAll()
        {
            lock (receiveLock)
            {
                var result = new List<PacketInfo>(received.Count);

                for (int i = 0; i < received.Count; i++)
                    result.Add(received.Dequeue());

                return result;
            }
        }

        public void QueuePacket(PacketInfo packetInfo)
        {
            lock (receiveLock)
            {
                received.Enqueue(packetInfo);
            }
        }

        public void Send(string message)
        {
            Send(remote, Encoding.ASCII.GetBytes(message));
        }

        public void Send(byte[] message)
        {
            if (remote == null)
                return;

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                s.SendTo(message, remote);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        #region static

        public static void Send(IPEndPoint endPoint, string message)
        {
            Send(endPoint, Encoding.ASCII.GetBytes(message));
        }

        public static void Send(IPEndPoint endPoint, byte[] message)
        {
            if (endPoint == null)
                return;

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.SendTo(message, endPoint);
        }

        #endregion
    }
}