using System;

namespace UnityLib.Network
{
    public class Relay
    {
        public enum OpCode
        {
            ping = 0,
            broadcast = 1,
            channelcast = 2,
            specific = 3,
            binaryMode = 4,
            stringMode = 5,
            listChannels = 6,
            joinChannel = 7,
            leaveChannel = 8,
            listClients = 9
        }

        //Send String
        public static string Message(uint id, string header, string content)
        {
            //return OpCode.specific + "\r\n\r\n" + id + "\r\n\r\n" + header + "\r\n\r\n" + content;
            return StringMessage(OpCode.specific, id, header, content);
        }

        public static string Broadcast(string header, string content)
        {
            //return OpCode.broadcast + "\r\n\r\n" + header + "\r\n\r\n" + content;
            return StringMessage(OpCode.broadcast, 0, header, content);
        }

        public static string Channelcast(uint id, string header, string content)
        {
            //return OpCode.channelcast + "\r\n\r\n" + id + "\r\n\r\n" + header + "\r\n\r\n" + content;
            return StringMessage(OpCode.channelcast, id, header, content);
        }

        public static string JoinChannel(uint id)
        {
            //return OpCode.joinChannel + "\r\n\r\n" + id + "\r\n\r\n\r\n\r\n";
            return StringMessage(OpCode.joinChannel, id, "", "");
        }

        public static string StringMessage(OpCode opCode, uint id, string header, string content)
        {
            return (byte)opCode + "\r\n\r\n" + id + "\r\n\r\n" + header + "\r\n\r\n" + content;
        }

        //Send Bytes
        public static byte[] Message(uint id, byte msgCode, byte[] bytes)
        {
            return Binary((byte)OpCode.specific, id, msgCode, bytes);
        }

        public static byte[] Broadcast(byte msgCode, byte[] bytes)
        {
            return Binary((byte)OpCode.broadcast, 0, msgCode, bytes);
        }

        public static byte[] Channelcast(uint id, byte msgCode, byte[] bytes)
        {
            return Binary((byte)OpCode.channelcast, id, msgCode, bytes);
        }

        public static byte[] Binary(byte relayCode, uint ID, byte msgCode, byte[] bytes)
        {
            var payload = new byte[6 + bytes.Length];
            payload[0] = relayCode;
            BitConverter.GetBytes(ID).CopyTo(payload, 1);
            payload[5] = msgCode;
            bytes.CopyTo(payload, 6);
            return payload;
        }

        public static string ClientList()
        {
            return OpCode.listClients.ToString();
        }

        public static string ParseResponse(string message, ref string content)
        {
            string[] stringSeparators = new string[] { "\r\n\r\n" };
            var splitted = message.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

            if (splitted.Length > 1)
                content = splitted[1];
            return splitted[0];
        }

        public static RelayMessage ParseRelayMessage(string message)
        {
            string[] stringSeparators = new string[] { "\r\n\r\n" };
            var splitted = message.Split(stringSeparators, StringSplitOptions.None);
            return new RelayMessage { senderID = uint.Parse(splitted[0]), header = splitted[1], content = splitted[2] };
        }

        public struct RelayMessage
        {
            public uint senderID;
            public string header;
            public string content;
        }
    }
}