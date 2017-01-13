using UnityEngine;
using UnityLib.EditorTool;
using UnityLib.Network;
using System.Text;

public class LanTest : MonoBehaviour
{
    public TCPServer server;
    public string message;
    public int port;
    string ip;

    [EditorButton]
    public void Boardcast()
    {
        NetworkUtility.BoardcastAll(Encoding.UTF8.GetBytes(message), port);
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}