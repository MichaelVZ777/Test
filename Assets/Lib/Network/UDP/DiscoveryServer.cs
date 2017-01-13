using UnityEngine;
using System.Net.Sockets;
using UnityLib.EditorTool;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Collections;

namespace UnityLib.Network
{
    public class DiscoveryServer : MonoBehaviour
    {
        public int port = 12000;
        public float broadcastInterval = 1;
        public string header;
        Socket s;

        [EditorButton]
        public void BroadcastIP()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                foreach (var uipi in ni.GetIPProperties().UnicastAddresses)
                    if (uipi.IPv4Mask != null)
                    {
                        var ep = new IPEndPoint(uipi.Address.GetBroadcastAddress(uipi.IPv4Mask), port);
                        var message = header + "\r\n\r\n" + uipi.Address.ToString();
                        s.SendTo(Encoding.UTF8.GetBytes(message), ep);
                    }
        }

        void Awake()
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.EnableBroadcast = true;
        }

        void OnEnable()
        {
            StartCoroutine(Broadcast());
        }

        void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator Broadcast()
        {
            while (true)
            {
                BroadcastIP();
                yield return new WaitForSeconds(broadcastInterval);
            }
        }
    }
}