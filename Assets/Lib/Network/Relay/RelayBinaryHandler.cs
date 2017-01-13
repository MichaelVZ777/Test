using UnityEngine;
using System.Collections;
using System;

namespace UnityLib.Network
{
    public class RelayBinaryHandler : SimpleMessageHandle
    {
        public override void OnRawBytes(byte[] bytes)
        {
            var payload = new byte[bytes.Length - 1];
            Array.Copy(bytes, 1, payload, 0, payload.Length);
            HandleBytesMessage(bytes[0], payload);
        }

        public virtual void HandleBytesMessage(byte msgCode, byte[] bytes) { }

        public override void OnConnected()
        {
            Send(0, "b");
        }

        public void Send(Relay.OpCode relayCode, uint ID, byte msgCode, byte[] bytes)
        {
            connection.SendBytes(Relay.Binary((byte)relayCode, ID, msgCode, bytes));
        }

        public void Send(byte relayCode, uint ID, byte msgCode, byte[] bytes)
        {
            connection.SendBytes(Relay.Binary(relayCode, ID, msgCode, bytes));
        }
    }
}