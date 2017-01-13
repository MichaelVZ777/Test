using UnityEngine;
using System.Collections;

namespace UnityLib.Network
{
    public class SimpleMessageHandle : MessageHandle
    {
        public override void OnRawString(string message)
        {
            string content = "";
            var header = Relay.ParseResponse(message, ref content);
            if (header == "0")
                return;
            HandleMessage(header, content);
        }

        public override void OnRawBytes(byte[] bytes)
        {
            HandleBinary(bytes);
        }

        public virtual void HandleMessage(string header, string content) { }

        public void Send(string header, string content)
        {
            connection.Send(Relay.Broadcast(header, content));
        }

        public void Send(uint id, string header)
        {
            connection.Send(Relay.Message(id, header, ""));
        }

        public void Send(uint id, string header, string content)
        {
            connection.Send(Relay.Message(id, header, content));
        }

        public void Send(byte[] bytes)
        {
            connection.SendBytes(bytes);
        }
    }
}