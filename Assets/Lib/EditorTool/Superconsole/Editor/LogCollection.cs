using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using ProtoBuf;
using Newtonsoft.Json.Serialization;

namespace SuperConsole
{
    [ProtoContract]
    public class LogCollection
    {
        public event Action OnLog;

        [ProtoMember(1)]
        public List<LogMessage> logs;
        [ProtoMember(2)]
        public int logCount { get; private set; }
        [ProtoMember(3)]
        public int warningCount { get; private set; }
        [ProtoMember(4)]
        public int errorCount { get; private set; }


        public LogCollection()
        {
            logs = new List<LogMessage>();
        }

        public void Add(LogMessage log)
        {
            logs.Add(log);
        }

        public void Clear()
        {
            logs.Clear();
        }

        public void OnLogMessageReceived(string condition, string stackTrace, LogType logType)
        {
            var lastLogFrameCount = Time.frameCount;

            var newMessage = new LogMessage();
            newMessage.frameCount = Time.frameCount;
            newMessage.message = condition;
            newMessage.stackTrace = stackTrace;
            newMessage.logType = logType;
            //messages.Add(newMessage);


            //update message count
            switch (logType)
            {
                case LogType.Log:
                    logCount++;
                    break;
                case LogType.Warning:
                    warningCount++;
                    break;
                case LogType.Error:
                    errorCount++;
                    break;
                case LogType.Exception:
                    errorCount++;
                    break;
                case LogType.Assert:
                    errorCount++;
                    break;
            }
            logs.Add(newMessage);

            if (OnLog != null)
                OnLog();
        }
    }

    [ProtoContract]
    public class LogMessage
    {
        [ProtoMember(1)]
        public int frameCount;
        [ProtoMember(2)]
        public string message;
        [ProtoMember(3)]
        public string stackTrace;
        [ProtoMember(4)]
        public LogType logType;
    }
}