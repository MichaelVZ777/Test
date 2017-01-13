using UnityEngine;
using System.Collections;
using UnityLib.Network;
using UnityEngine.UI;
using System.Net;
using UnityLib.EditorTool;

public class ManualIP : MonoBehaviour
{
    public TCPConnection[] connections;
    public InputField IPText;
    public string defaultIP = "127.0.0.1";
    string lastIP;

    void Awake()
    {
        lastIP = "";
        IPText.text = PlayerPrefs.GetString("IP", defaultIP);
        connections = GetComponentsInChildren<TCPConnection>();
        Update();
        foreach (var connection in connections)
        {
            connection.autoConnect = true;
            connection.IP = IPText.text;
        }
    }

    [EditorButton]
    public void Set()
    {
        IPText.text = defaultIP;
    }

    void Update()
    {
        if (IPText.text != lastIP)
        {
            lastIP = IPText.text;
            IPAddress newIP = null;
            if (IPAddress.TryParse(lastIP, out newIP))
            {
                foreach (var connection in connections)
                    connection.SetIP(newIP);
                PlayerPrefs.SetString("IP", newIP.ToString());
            }
        }
    }
}
