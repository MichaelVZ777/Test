using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.Threading;
using System.IO;

namespace UnityLib.Network
{
    /// <summary>
    /// Created by the tcp server when a client connect
    /// </summary>
    public class TCPHandle
    {
        public enum Status { Disconnected, Connected }
        public Status status { get; private set; }

        TCPServer server;
        TcpClient client;
        NetworkStream stream;
        Thread listenThread;
        public int ping;
        DateTime pingTime;

        public TCPHandle(TCPServer server, TcpClient client)
        {
            this.server = server;
            this.client = client;
            client.NoDelay = true;
            stream = client.GetStream();
        }

        public void Listen()
        {
            listenThread = new Thread(HandleClient);
            listenThread.Start();
        }

        void HandleClient()
        {
            status = Status.Connected;
            stream.ReadTimeout = 3000;
            var messageLengthBuf = new byte[4];
            //int receivedHeaderLength = 0;
            try
            {
                while (status != Status.Disconnected)
                {
                    //read frame length
                    int receivedHeaderLength = 0;
                    receivedHeaderLength = stream.Read(messageLengthBuf, 0, 4);
                    if (receivedHeaderLength == 0)
                        break;

                    while (receivedHeaderLength < 4)
                    {
                        int thisReadLength;
                        thisReadLength = stream.Read(messageLengthBuf, receivedHeaderLength, 4 - receivedHeaderLength);
                    }

                    var frameLength = (int)BitConverter.ToUInt32(messageLengthBuf, 0);

                    //read frame
                    int receivedFrameLength = 0;
                    var dataBuf = new byte[frameLength];

                    while (receivedFrameLength < frameLength)
                    {
                        int thisReadLength;
                        thisReadLength = stream.Read(dataBuf, receivedFrameLength, frameLength - receivedFrameLength);
                        if (thisReadLength == 0)
                        {
                            status = Status.Disconnected;
                            break;
                        }

                        receivedFrameLength += thisReadLength;
                    }

                    if (status == Status.Disconnected)
                        break;

                    OnMessage(System.Text.Encoding.ASCII.GetString(dataBuf, 0, dataBuf.Length));
                    //Debug.Log("server: " + System.Text.Encoding.ASCII.GetString(dataBuf, 0, dataBuf.Length));
                }
            }
            catch (Exception e)
            {
                if (!(e is SocketException || e is IOException))
                    Debug.LogException(e);
            }
            status = Status.Disconnected;
            stream.Close();
            client.Close();
            server.OnClientDisconnect(this);
        }

        public void SendPing()
        {
            pingTime = DateTime.Now;
            SendMessage("ping", ping.ToString());
        }

        public void OnMessage(string message)
        {
            var content = "";
            var header = Relay.ParseResponse(message, ref content);

            if (header == "pong")
                ping = (DateTime.Now - pingTime).Milliseconds;
        }

        public void WriteFrame(string message)
        {
            var frame = NetworkUtility.FrameMessage(message);
            client.GetStream().Write(frame, 0, frame.Length);
        }

        public int SendMessage(string header, string content)
        {
            var frame = NetworkUtility.FrameMessage(header + "\r\n\r\n" + content);
            try
            {
                if (client != null && client.Connected)
                    stream.Write(frame, 0, frame.Length);
            }
            catch (Exception e)
            {
                if (!(e is SocketException || e is IOException))
                    Debug.LogException(e);
            }
            return frame.Length;
        }
    }
}