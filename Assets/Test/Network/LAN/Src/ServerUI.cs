using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityLib.Network;

public class ServerUI : MonoBehaviour
{
    public Text serverState;
    public Text clientCount;
    public Text IP;

    public TCPServer server;

    void Start()
    {
        IP.text = NetworkUtility.GetLocalIPAddress().ToString();
    }

    void Update()
    {
        serverState.text = server.state.ToString();
        clientCount.text = server.GetClientCount() + " clients";
    }
}
