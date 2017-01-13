using UnityLib.Network;

public class TCPTest : SimpleMessageHandle
{
    public string header;
    public string content;

    public override void HandleMessage(string header, string content)
    {
        if (header != "0")
            print(header + "|" + content);
    }

    void Update()
    {
        connection.Send(Relay.Broadcast(header, content));
    }
}