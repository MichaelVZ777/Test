using System;

namespace UnityLib.Network
{
    public class RelayHandle : MessageHandle
    {
        public enum DataMode { String, Binary }
        public DataMode dataMode;
        DataMode currentDataMode;

        public override void OnConnected()
        {
            if (dataMode != DataMode.String)
                SetMode(dataMode);
        }

        //Binary
        public virtual void HandleBytesMessage(uint senderID, byte msgCode, byte[] bytes) { }

        public override void OnRawBytes(byte[] bytes)
        {
            var payload = new byte[bytes.Length - 5];
            Array.Copy(bytes, 5, payload, 0, payload.Length);
            HandleBytesMessage(BitConverter.ToUInt32(bytes, 0), bytes[4], payload);
        }

        //String
        public virtual void HandleStringMessage(uint senderID, string header, string content) { }

        public override void OnRawString(string rawString)
        {
            var message = Relay.ParseRelayMessage(rawString);

            if (message.header == "0")
                return;
            HandleStringMessage(message.senderID, message.header, message.content);
        }

        //Send String
        public void Broadcast(string header, string content)
        {
            connection.Send(Relay.Broadcast(header, content));
        }

        public void Channelcast(uint id, string header, string content)
        {
            connection.Send(Relay.Channelcast(id, header, content));
        }

        public void Channelcast(uint id, int header, string content)
        {
            connection.Send(Relay.Channelcast(id, header.ToString(), content));
        }

        public void Send(uint id, string header)
        {
            connection.Send(Relay.Message(id, header, ""));
        }

        public void Send(uint id, int header, string content)
        {
            connection.Send(Relay.Message(id, header.ToString(), content));
        }

        public void Send<T>(uint id,T header,string content) where T:struct,IConvertible
        {
            connection.Send(Relay.Message(id, header.ToString(), content));
        }

        public void Send(uint id, string header, string content)
        {
            connection.Send(Relay.Message(id, header, content));
        }

        //Send Bytes
        public void Send(byte[] bytes)
        {
            connection.SendBytes(bytes);
        }

        public void Broadcast(byte messageCode, byte[] bytes)
        {
            connection.SendBytes(Relay.Broadcast(messageCode, bytes));
        }

        public void Channelcast(uint id, byte messageCode, byte[] bytes)
        {
            connection.SendBytes(Relay.Channelcast(id, messageCode, bytes));
        }

        public void Send(uint id, byte messageCode, byte[] bytes)
        {
            connection.SendBytes(Relay.Message(id, messageCode, bytes));
        }

        //Common
        public void SetMode(DataMode mode)
        {
            if (currentDataMode != mode)
                currentDataMode = mode;

            switch (currentDataMode)
            {
                case DataMode.Binary:
                    connection.Send(Relay.StringMessage(Relay.OpCode.binaryMode, 0, "", ""));
                    break;
                case DataMode.String:
                    connection.SendBytes(Relay.Binary((byte)Relay.OpCode.stringMode, 0, 0, new byte[0]));
                    break;
            }
            currentDataMode = mode;
            dataMode = mode;
        }

        public void Join(uint channelID)
        {
            connection.Send(Relay.StringMessage(Relay.OpCode.joinChannel, channelID, "", ""));
        }
    }
}