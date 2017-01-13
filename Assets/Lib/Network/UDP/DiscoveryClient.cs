using System.Net;

namespace UnityLib.Network
{
    public class DiscoveryClient : SimpleMessageHandle
    {
        public TCPConnection client;
        string ip;

        public override void HandleMessage(string header, string content)
        {
            if (ip != content)
            {
                ip = content;
                client.SetIP(IPAddress.Parse(ip));
            }
        }
    }
}