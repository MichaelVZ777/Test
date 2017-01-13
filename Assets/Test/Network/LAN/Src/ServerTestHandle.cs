using UnityEngine;
using System.Collections;
using UnityLib.Network;
using System;

public class ServerTestHandle : ServerMessageHandle
{
    int i = 0;

    public override void HandleMessage(string header, string content)
    {
        Debug.Log(header + " " + content);
    }

    void Update()
    {
        if (Time.frameCount > 10)
            server.Broadcast("test", i++.ToString());
    }
}
