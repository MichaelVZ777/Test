using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;

public class DebugInfo : SingletonMonoManager<DebugInfo>
{
    public static int candidatesCount;
    public static int connectedObjectCount;
    public Color logColor = Color.green;
    public int logCount = 30;
    public int fontSize = 12;
    public bool disableInEditor;

    static Thread mainThread;

    protected Dictionary<string, Info> infos;
    Dictionary<string, Action> buttons;
    protected Log[] logs;
    protected int logIndex { get; private set; }
    protected int nextIndex { get { return logIndex + 1 == logs.Length ? 0 : logIndex + 1; } }

    void Awake()
    {
        infos = new Dictionary<string, Info>();
        buttons = new Dictionary<string, Action>();
        logs = new Log[logCount];
        Application.logMessageReceivedThreaded += OnLogReceived;
        //Application.logMessageReceived += OnLogReceived;
        mainThread = Thread.CurrentThread;
    }

    public static void Toggle()
    {
        Instance.enabled = !Instance.enabled;
    }

    public static void Set(string name, object content)
    {
        var info = new Info();
        info.color = Color.white;
        Instance.infos[name] = info;

        if (content == null)
            info.content = "null";
        else
            info.content = content;
    }

    public static void RemoveText(string name)
    {
        if (Instance.infos.ContainsKey(name))
            Instance.infos.Remove(name);
    }

    public static void Set(string name, Action action)
    {
        Instance.buttons[name] = action;
    }

    void OnGUI()
    {
#if UNITY_EDITOR
        if (disableInEditor)
            return;
#endif

        if (!enabled)
            return;

        GUI.depth = -100;
        GUI.skin.label.fontSize = fontSize;

        DrawInfos();

        //DrawLabel("");

        foreach (var pair in buttons)
            DrawButtons(pair.Key, pair.Value);

        //DrawLabel("");

        DrawLogs();
    }

    protected virtual void DrawInfos()
    {
        foreach (var pair in infos)
            DrawLabel(pair.Key + pair.Value.content.ToString());
    }

    void DrawLabel(string s)
    {
        DrawLabel(s, logColor);
    }

    protected virtual void DrawLabel(string s, Color color)
    {
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.normal.textColor = color;
        style.fontSize = fontSize;
        GUILayout.Label(s, style);
    }

    void DrawButtons(string s, Action action)
    {
        GUIStyle style = new GUIStyle();
        style.richText = true;
        //        style.normal.textColor = color;
        style.fontSize = fontSize;
        if (GUILayout.Button(s, style))
            action();
    }

    protected virtual void DrawLogs()
    {
        string text = "";

        for (int i = nextIndex; i != logIndex;)
        {
            i = i + 1 == logs.Length ? 0 : i + 1;
            var log = logs[i];

            if (log == null)
                continue;

            text += log.message + "\n";
            if (log.logType == LogType.Exception)
                text += @"<color=red>" + log.stackTrace + @"</color>";
        }

        DrawLabel(text);
    }

    public static void LogThread(string message)
    {
        if (Thread.CurrentThread == mainThread)
            Debug.Log(message);
        else
        {
            Debug.Log(message);
            _instance.OnLogReceived(message, "", LogType.Log);
        }
    }

    void OnLogReceived(string message, string stackTrace, LogType logType)
    {
        var log = new Log();
        log.message = message;
        log.stackTrace = stackTrace;
        log.logType = logType;

        logIndex = nextIndex;
        logs[logIndex] = log;
    }

    protected class Info
    {
        public object content;
        public Color color;
    }

    protected class Log
    {
        public string message;
        public string stackTrace;
        public LogType logType;
    }
}
