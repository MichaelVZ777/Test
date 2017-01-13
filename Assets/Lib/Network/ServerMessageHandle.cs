using UnityEngine;
using System.Collections;

namespace UnityLib.Network
{
    public abstract class ServerMessageHandle : MonoBehaviour
    {
        public IServer server;

        void Awake()
        {
            if (server == null)
                server = GetComponent<IServer>();
        }

        public abstract void HandleMessage(string header, string content);

        public virtual void OnConnected() { }

        public virtual void OnConnecting() { }

        public virtual void OnDisconencted() { }
    }

    public interface IServer
    {
        void Broadcast(string header, string content);
    }
}