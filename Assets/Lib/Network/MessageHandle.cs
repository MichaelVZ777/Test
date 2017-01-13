using UnityEngine;
using System.Collections;

namespace UnityLib.Network
{
    public abstract class MessageHandle : MonoBehaviour
    {
        public IConnection connection;

        protected void Awake()
        {
            if (connection == null)
                connection = GetComponent<IConnection>();
        }

        public virtual void HandleString(string message) { }
        public virtual void HandleBinary(byte[] bytes) { }

        public virtual void OnRawString(string message)
        {
            HandleString(message);
        }

        public virtual void OnRawBytes(byte[] bytes)
        {
            HandleBinary(bytes);
        }

        public virtual void OnConnected() { }

        public virtual void OnConnecting() { }

        public virtual void OnDisconencted() { }
    }

    public interface IConnection
    {
        void Send(string message);
        void SendBytes(byte[] bytes);
        string GetAddress();
        void Connect();
    }
}