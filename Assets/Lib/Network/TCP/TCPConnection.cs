using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityLib.EditorTool;
using System.IO;
using System.Text;

namespace UnityLib.Network
{
    public class TCPConnection : MonoBehaviour, IConnection
    {
        public event Action<string> OnMessageReceived;

        public enum Status { Disconnected, InvalidIP, Connecting, Connected }
        public string IP = "127.0.0.1";
        public int port = 12000;
        public bool autoConnect;
        public MessageHandle handle;

        Status status, lastStatus;
        IPEndPoint endPoint;
        TcpClient client;
        Thread thread;
        NetworkStream stream;
        bool isListening;
        bool initialized;
        List<string> messageQueue;

        void Start()
        {
            status = Status.Disconnected;

            var dns = GetComponent<DNSResolver>();
            if (dns != null && dns.enabled)
                endPoint = new IPEndPoint(dns.Resolve(), port);
            else
                endPoint = new IPEndPoint(IPAddress.Parse(IP), port);

            messageQueue = new List<string>();
            handle = GetComponent<MessageHandle>();

            initialized = true;
        }

        void OnDisable()
        {
            Disconnect();
        }

        void OnApplicationPause()
        {
            Disconnect();
        }

        void OnApplicationQuit()
        {
            Disconnect();
        }

        public void ShowInfo()
        {
            GetComponent<Image>().enabled = true;
        }

        public void HideInfo()
        {
            GetComponent<Image>().enabled = false;
        }

        public void OnPortNumberChanged(int port)
        {
            endPoint.Port = port;
            Connect();
        }

        void Update()
        {
            if (status == Status.Disconnected && autoConnect)
                Connect();


            string[] messages = null;
            lock (messageQueue)
            {
                messages = messageQueue.ToArray();
                messageQueue.Clear();
            }

            if (handle != null)
                foreach (var message in messages)
                    handle.OnRawString(message);

            if (status != lastStatus)
            {
                lastStatus = status;
                if (handle != null)
                    switch (status)
                    {
                        case Status.Connected:
                            handle.OnConnected();
                            break;
                        case Status.Connecting:
                            handle.OnConnecting();
                            break;
                        case Status.Disconnected:
                            handle.OnDisconencted();
                            break;
                    }
            }
        }

        public void ParseIP(string IP)
        {
            if (!initialized)
                return;
            PlayerPrefs.SetString("client" + GetInstanceID(), IP);
            Regex regex = new Regex(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
            Match match = regex.Match(IP);
            if (!match.Success)
                return;

            IPAddress address = null;

            if (IPAddress.TryParse(IP, out address))
            {
                endPoint.Address = address;
                status = Status.Disconnected;
            }
            else
                status = Status.InvalidIP;


            print("Parsed: " + endPoint.Address);
        }

        public void SetIP(IPAddress address)
        {
            Disconnect();

            if (endPoint == null)
                endPoint = new IPEndPoint(address, port);
            else
                endPoint.Address = address;
            Connect();
        }

        [EditorButton]
        public void Connect()
        {
            if (status == Status.InvalidIP)
                return;

            Disconnect();
            thread = new Thread(StartConnection);
            thread.Start();
        }

        public void Disconnect()
        {
            status = Status.Disconnected;
            if (thread != null)
                thread.Abort();
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }

        void StartConnection()
        {
            status = Status.Connecting;
            //Debug.Log("connecting" + endPoint.Address);
            try
            {
                client = new TcpClient();
                //client.time
                client.Connect(endPoint);
                client.ReceiveTimeout = 3000;
                client.NoDelay = true;
                status = Status.Connected;

                stream = client.GetStream();
                var messageLengthBuf = new byte[4];
                while (status != Status.Disconnected)
                {
                    //read frame length
                    int receivedHeaderLength = 0;

                    while (receivedHeaderLength < 4)
                    {
                        var thisReadLength = stream.Read(messageLengthBuf, receivedHeaderLength, 4 - receivedHeaderLength);
                        receivedHeaderLength += thisReadLength;

                        if (thisReadLength == 0)
                        {
                            status = Status.Disconnected;
                            break;
                        }
                    }

                    var frameLength = (int)BitConverter.ToUInt32(messageLengthBuf, 0);

                    //read frame
                    int receivedFrameLength = 0;
                    var dataBuf = new byte[frameLength];

                    while (receivedFrameLength < frameLength)
                    {
                        var thisReadLength = stream.Read(dataBuf, receivedFrameLength, frameLength - receivedFrameLength);
                        if (thisReadLength == 0)
                        {
                            status = Status.Disconnected;
                            break;
                        }

                        receivedFrameLength += thisReadLength;
                    }

                    if (status == Status.Disconnected)
                        break;

                    lock (messageQueue)
                    {
                        messageQueue.Add(System.Text.Encoding.UTF8.GetString(dataBuf, 0, dataBuf.Length));
                    }
                }
            }
            catch (Exception e)
            {
                //if (!(e is SocketException || e is IOException || e is ObjectDisposedException))
                //    Debug.LogException(e);
            }

            Disconnect();
        }

        string ParseResponse(string message, ref string content)
        {
            string[] stringSeparators = new string[] { "\r\n\r\n" };
            var splitted = message.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

            if (splitted.Length > 1)
                content = splitted[1];
            return splitted[0];
        }

        public void Send(string message)
        {
            SendBytes(Encoding.UTF8.GetBytes(message));
        }

        public void SendBytes(byte[] bytes)
        {
            var frame = NetworkUtility.FrameBytes(bytes);
            try
            {
                if (stream != null && stream.CanWrite)
                    stream.Write(frame, 0, frame.Length);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public string GetAddress()
        {
            return IP;
        }
    }
}