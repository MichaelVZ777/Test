using UnityEngine;
using System.Collections;
using UnityLib.Network;
using System;
using UnityEngine.UI;

public class LANTestHandle : SimpleMessageHandle
{
    public Text pingText;
    public Text dropText;
    public Text receivedText;
    public Text connectCountText;

    int next = 0, received, dropped;
    int connectCount, j;

    public override void HandleMessage(string header, string content)
    {
        switch (header)
        {
            case "ping":
                pingText.text = content;
                break;
            case "test":
                received++;
                var i = int.Parse(content);
                if (i != next + 1)
                    dropped++;
                next = i;
                
                break;
        }
    }

    public override void OnConnected()
    {
        pingText.color = Color.green;
        connectCountText.text = connectCount++.ToString(); ;
    }

    public override void OnConnecting()
    {
        pingText.color = Color.yellow;
    }

    public override void OnDisconencted()
    {
        pingText.color = Color.red;
    }

    void Update()
    {
        //connection.send("test", j++.ToString());
        //var tcpCon = (TCPConnection)connection;

        //receivedText.text = received.ToString();
        //dropText.text = dropped.ToString();
    }
}
