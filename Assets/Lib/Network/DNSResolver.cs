using UnityEngine;
using System.Collections;
using System.Net;

namespace UnityLib.Network
{
    public class DNSResolver : MonoBehaviour
    {
        public string url = "relay.vz777.com";

        void Start()
        {

        }

        public IPAddress Resolve()
        {
            return DNS.Resolve(url);
        }
    }

    public class DNS
    {
        public static IPAddress Resolve(string url)
        {
            IPHostEntry hostInfo = Dns.GetHostEntry(url);
            IPAddress[] address = hostInfo.AddressList;

            if (address.Length > 0)
                return address[0];
            return null;
        }
    }
}