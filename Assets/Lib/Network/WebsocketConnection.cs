using UnityEngine;
using WebSocket4Net;
using System;
using SuperSocket.ClientEngine;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace UnityLib.Network
{
    public class WebsocketConnection : MonoBehaviour, IConnection
    {
        public string remoteHost = @"relay.vz777.com";
        public string localHost = @"localhost:8080";
        public bool useSSL;
        public bool useLocalHost;
        public MessageHandle handle;

        List<string> messageQueue;
        List<byte[]> binaryQueue;
        WebSocket websocket;
        bool opened;

        void Awake()
        {
            if (handle == null)
                handle = GetComponent<MessageHandle>();

            if (handle == null)
                Debug.Log("no message handler");

            messageQueue = new List<string>();
            binaryQueue = new List<byte[]>();
        }

        void OnEnable()
        {
            StartCoroutine(AutoConnect());
        }

        void OnDisable()
        {
            Disconnect();
            StopAllCoroutines();
        }

        void Update()
        {
            //byte
            byte[][] bytes = null;
            lock (binaryQueue)
            {
                bytes = binaryQueue.ToArray();
                binaryQueue.Clear();
            }

            if (handle != null)
                foreach (var b in bytes)
                    handle.OnRawBytes(b);

            //string
            string[] messages = null;
            lock (messageQueue)
            {
                messages = messageQueue.ToArray();
                messageQueue.Clear();
            }

            if (handle != null)
                foreach (var message in messages)
                    handle.OnRawString(message);
        }

        IEnumerator AutoConnect()
        {
            while (true)
            {
                if (!opened)
                    Connect();
                yield return new WaitForSeconds(3);
            }
        }

        public void Connect()
        {
            Disconnect();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var wsURL = GetAddress();
            //print("connecting" + wsURL);
            websocket = new WebSocket(wsURL);
            websocket.AllowUnstrustedCertificate = true;
            websocket.Opened += new EventHandler(OnWebSocketOpen);
            websocket.Error += new EventHandler<ErrorEventArgs>(OnWebSocketError);
            websocket.Closed += new EventHandler(OnWebsocketClosed);
            websocket.DataReceived += new EventHandler<DataReceivedEventArgs>(OnWebSocketDataReceived);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(OnWebSocketMessageReceived);
            websocket.Open();
        }

        public void Disconnect()
        {
            //print("disconnecting");
            if (websocket != null)
                websocket.Close();
        }

        public void UseSSL(bool useSSL)
        {
            this.useSSL = useSSL;
            Connect();
        }

        public void UseLocal(bool useLocal)
        {
            useLocalHost = useLocal;
            Connect();
        }

        public string GetAddress()
        {
            return (useSSL ? "wss://" : "ws://") + (useLocalHost ? localHost : remoteHost) + "/ws";
        }

        void OnWebSocketOpen(object sender, EventArgs e)
        {
            //print("opened");
            opened = true;

            try
            {
                if (handle != null)
                    handle.OnConnected();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        void OnWebSocketError(object sender, ErrorEventArgs e)
        {
            //Debug.LogException(e.Exception);
        }

        void OnWebsocketClosed(object sender, EventArgs e)
        {
            //print("closed");
            opened = false;

            try
            {
                if (handle != null)
                    handle.OnDisconencted();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        void OnWebSocketDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (binaryQueue)
            {
                binaryQueue.Add(e.Data);
            }
        }

        void OnWebSocketMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            lock (messageQueue)
            {
                messageQueue.Add(e.Message);
            }
        }

        public void Send(string message)
        {
            websocket.Send(message);
        }

        public void SendBytes(byte[] bytes)
        {
            websocket.Send(bytes, 0, bytes.Length);
        }
    }
}