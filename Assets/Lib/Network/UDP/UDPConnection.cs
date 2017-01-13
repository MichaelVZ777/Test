using UnityEngine;
using System.Text;

namespace UnityLib.Network
{
    public class UDPConnection : MonoBehaviour, IConnection
    {
        public int port;
        public MessageHandle handle;
        public bool autoRestart = true;
        public UDPEndPoint udp;

        void Awake()
        {
            udp = new UDPEndPoint();
            udp.listenPort = port;

            if (handle == null)
                handle = GetComponent<MessageHandle>();
        }

        void OnEnable()
        {
            udp.StartListener();
        }

        void OnDisable()
        {
            udp.StopListener();
        }

        void OnApplicationPause()
        {
            print("pause");
            udp.StopListener();
        }

        void Update()
        {
            foreach (var packetInfo in udp.ReceiveAll())
                if (handle != null)
                {
                    handle.OnRawBytes(packetInfo.data);
                    handle.OnRawString(Encoding.UTF8.GetString(packetInfo.data));
                }

            if (!udp.isRunning && autoRestart)
                udp.StartListener();
        }

        public void Connect()
        {

        }

        public void Broadcast(string header, string content)
        {
            NetworkUtility.BoardcastAll(Encoding.UTF8.GetBytes(header + "\r\n\r\n" + content), port);
        }

        public void Send(string message)
        {
            NetworkUtility.BoardcastAll(Encoding.UTF8.GetBytes(message), port);
        }

        public void SendBytes(byte[] bytes)
        {
            NetworkUtility.BoardcastAll(bytes, port);
        }

        public string GetAddress()
        {
            return "UDP";
        }
    }
}