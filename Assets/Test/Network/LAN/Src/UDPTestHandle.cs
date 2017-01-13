using UnityEngine;
using System.Collections;
using UnityLib.Network;
using System;

public class UDPTestHandle : SimpleMessageHandle
{
    public override void HandleMessage(string header, string content)
    {
        Debug.Log(header + "|" + content);
    }
}
