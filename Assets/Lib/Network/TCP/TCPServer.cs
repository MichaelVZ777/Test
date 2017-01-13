using UnityEngine;
using System.Net.Sockets;
using System;
using System.Threading;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Net;

namespace UnityLib.Network
{
    public class TCPServer : MonoBehaviour, IServer
    {
        ServerMessageHandle messageHandle;
        Queue<HandleMessage> messageQueue;

        public class HandleMessage
        {
            public TCPHandle handle;
            public string header;
            public string content;
        }

        public enum States
        {
            Started,
            Stopped
        }

        public States state { get; private set; }

        public int port = 12000;
        public float pingInterval = 1;

        TcpListener listener;
        Thread serverThread, pingThread;
        List<TCPHandle> handles;

        public int GetClientCount()
        {
            return handles.Count;
        }

        void Awake()
        {
            handles = new List<TCPHandle>();
            messageQueue = new Queue<HandleMessage>();

            if (messageHandle == null)
                messageHandle = GetComponent<ServerMessageHandle>();
        }

        void OnEnable()
        {
            serverThread = new Thread(Listen);
            serverThread.Start();
        }

        void OnDisable()
        {
            if (serverThread != null)
                serverThread.Abort();
            if (pingThread != null)
                pingThread.Abort();
            if (listener != null)
                listener.Stop();
        }

        void OnApplicationQuit()
        {
            OnDisable();
        }

        void Update()
        {
            var queuedMessage = new List<HandleMessage>();
            lock (messageQueue)
            {
                while (messageQueue.Count > 0)
                    queuedMessage.Add(messageQueue.Dequeue());
            }

            foreach (var message in queuedMessage)
                if (messageHandle != null)
                    messageHandle.HandleMessage(message.header, message.content);
        }

        void Listen()
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            pingThread = new Thread(Ping);
            pingThread.Start();

            while (true)
            {
                var client = listener.AcceptTcpClient();
                var handle = new TCPHandle(this, client);
                lock (handles)
                {
                    handles.Add(handle);
                }
                handle.Listen();
            }
        }

        public void OnClientDisconnect(TCPHandle handle)
        {
            lock (handles)
            {
                handles.Remove(handle);
            }
        }

        void Ping()
        {
            while (true)
            {
                lock (handles)
                {
                    foreach (var handle in handles)
                        handle.SendPing();
                }

                Thread.Sleep((int)(pingInterval * 1000));
            }
        }

        public void OnHandleMessage(TCPHandle handle, string header, string content)
        {
            lock (messageQueue)
            {
                messageQueue.Enqueue(new HandleMessage { handle = handle, header = header, content = content });
            }
        }

        public void Broadcast(string header, string content)
        {
            int length = 0;
            lock (handles)
            {
                foreach (var handle in handles)
                    length = handle.SendMessage(header, content);
            }
        }
    }
}