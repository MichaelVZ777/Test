using UnityEngine;
using System.Collections;
using UnityLib.Network;
using System;
using UnityLib.EditorTool;

public class WebsocketTests : SimpleMessageHandle
{
    public override void HandleMessage(string header, string content)
    {
        print(header + "|" + content);
    }
}
