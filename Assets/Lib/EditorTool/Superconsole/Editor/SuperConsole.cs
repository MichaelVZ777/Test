using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace SuperConsole
{
    public class SuperConsole : EditorWindow, ISerializationCallbackReceiver
    {
        GUIContent content;
        LogCollection logs;

        //title bar
        bool collapse, clearOnPlay, pauseOnError;
        bool showLog = true;
        bool showWarning = true;
        bool showError = true;
        int logCount, warningCount, errorCount;
        bool compiling;

        //input
        double clickTime;
        double doubleClickTime = 0.3;

        //debug
        string text, text2;

        //content
        Vector2 contentSize;

        //stack view
        Vector2 stackViewScrollPosition;

        //split bar
        float splitBarWidth = 5;
        float stackViewWidth;
        bool resize = false;
        Rect splitBarRect;

        //log view
        Vector2 logViewScrollPosition;
        LogMessage selected;
        float logHeight = 10;
        int lastLogFrameCount;
        GUIStyle logStyle;
        Texture2D whiteTexture;

        byte[] bytes;

        void OnEnable()
        {
            if (logs == null)
                logs = new LogCollection();

            Application.logMessageReceived += logs.OnLogMessageReceived;
            logs.OnLog += () => { Repaint(); };

            content = new GUIContent();
            titleContent = GetIconContent("icons/UnityEditor.ConsoleWindow.png", "SConsole");

            stackViewWidth = position.width * 0.25f;
            splitBarRect = new Rect(stackViewWidth, position.y, 5, position.height);

            lastLogFrameCount = -1;

            LoadPreference();

            if (whiteTexture == null)
            {
                whiteTexture = new Texture2D(16, 16);
                for (int x = 0; x < 16; x++)
                    for (int y = 0; y < 16; y++)
                        whiteTexture.SetPixel(x, y, Color.white);
                whiteTexture.Apply();
            }
        }

        void OnDisable()
        {
            SavePreference();
        }

        void LoadPreference()
        {
            collapse = EditorPrefs.GetBool("SC-collapse");
            clearOnPlay = EditorPrefs.GetBool("SC-clearOnPlay");
            pauseOnError = EditorPrefs.GetBool("SC-pauseOnError");

            showLog = EditorPrefs.GetBool("SC-showLog");
            showWarning = EditorPrefs.GetBool("SC-showWarning");
            showError = EditorPrefs.GetBool("SC-showError");
        }

        void SavePreference()
        {
            EditorPrefs.SetBool("SC-collapse", collapse);
            EditorPrefs.SetBool("SC-clearOnPlay", clearOnPlay);
            EditorPrefs.SetBool("SC-pauseOnError", pauseOnError);

            EditorPrefs.SetBool("SC-showLog", showLog);
            EditorPrefs.SetBool("SC-showWarning", showWarning);
            EditorPrefs.SetBool("SC-showError", showError);
        }

        void OnGUI()
        {
            //Header
            GUILayout.BeginHorizontal(EditorStyles.toolbarButton);
            if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
                Clear();

            GUILayout.Space(5);

            collapse = GUILayout.Toggle(collapse, "Collapse", EditorStyles.toolbarButton);

            //broken
            //clearOnPlay = GUILayout.Toggle(clearOnPlay, "Clear on Play", EditorStyles.toolbarButton);
            pauseOnError = GUILayout.Toggle(pauseOnError, "Error Pause", EditorStyles.toolbarButton);
            DrawStatus();

            GUILayout.Label(text, EditorStyles.toolbarButton);
            GUILayout.Label(text2, EditorStyles.toolbarButton);

            GUILayout.FlexibleSpace();

            showLog = GUILayout.Toggle(showLog, GetIconContent("icons/console.infoicon.sml.png", logCount.ToString()), EditorStyles.toolbarButton);
            showWarning = GUILayout.Toggle(showWarning, GetIconContent("icons/console.warnicon.sml.png", warningCount.ToString()), EditorStyles.toolbarButton);
            showError = GUILayout.Toggle(showError, GetIconContent("icons/console.erroricon.sml.png", errorCount.ToString()), EditorStyles.toolbarButton);
            GUILayout.EndHorizontal();

            //content
            var contentRect = EditorGUILayout.BeginHorizontal();
            if (contentRect.size != Vector2.zero)
                contentSize = contentRect.size;

            DrawStackView();
            ResizeScrollView(contentRect);
            GUILayout.Space(splitBarWidth);
            DrawLogView();
            EditorGUILayout.EndHorizontal();
        }

        void DrawStatus()
        {
            if (EditorApplication.isCompiling)
            {
                GUILayout.Label("Compiling", EditorStyles.toolbarButton);
                compiling = true;
            }
            //else if (compiling)
            //    CompilingCompleted();

            if (EditorApplication.isPlaying)
                GUILayout.Label("Playing", EditorStyles.toolbarButton);
        }

        void CompilingCompleted()
        {
            compiling = false;
            Clear();
        }

        void DrawStackView()
        {
            stackViewScrollPosition = EditorGUILayout.BeginScrollView(stackViewScrollPosition, GUILayout.Width(stackViewWidth));

            if (selected == null)
            {
                var style = EditorStyles.helpBox;
                style.normal.textColor = Color.black;
                GUILayout.Label("Select a log to show stack trace", style);
                EditorGUILayout.EndScrollView();
                return;
            }

            var items = StackTraceExtractor.ExtractStackTraceItems(selected.stackTrace);

            foreach (var item in items)
                DrawStackTraceItem(item);

            EditorGUILayout.EndScrollView();
        }

        void DrawStackTraceItem(StackTraceItem item)
        {
            var style = EditorStyles.helpBox;
            style.alignment = TextAnchor.MiddleLeft;
            style.richText = true;
            style.normal.textColor = Color.black;

            content.text = item.methodName + "\n<color=#808000ff>" + item.fileName + ":" + item.lineNumber + "</color>";
            //content.text = "<size=30>Some <color=yellow>RICH</color> text</size>";

            if (GUILayout.Button(content, style, GUILayout.ExpandWidth(true)))
            {
                if ((EditorApplication.timeSinceStartup - clickTime) < doubleClickTime)
                    OnStackDoubleClick(item);
                clickTime = EditorApplication.timeSinceStartup;
            }
        }

        void OnStackDoubleClick(StackTraceItem item)
        {
            EditorTools.OpenScript(item.path, item.lineNumber);
        }

        void ResizeScrollView(Rect contentRect)
        {
            splitBarRect.Set(stackViewWidth, contentRect.y, splitBarWidth, contentRect.height);
            GUI.DrawTexture(splitBarRect, EditorGUIUtility.whiteTexture);
            EditorGUIUtility.AddCursorRect(splitBarRect, MouseCursor.ResizeHorizontal);

            //Mouse Down
            if (Event.current.type == EventType.mouseDown && splitBarRect.Contains(Event.current.mousePosition))
                resize = true;

            if (resize)
            {
                stackViewWidth = Event.current.mousePosition.x;

                stackViewWidth = Mathf.Clamp(stackViewWidth, 0, contentSize.x - splitBarWidth);
                splitBarRect.Set(stackViewWidth, splitBarRect.y, splitBarRect.width, splitBarRect.height);
                Repaint();
            }

            //Mouse Up
            if (Event.current.type == EventType.MouseUp)
                resize = false;
        }

        void DrawLogView()
        {
            logViewScrollPosition = EditorGUILayout.BeginScrollView(logViewScrollPosition);

            foreach (var message in logs.logs)
                DrawLog(message);

            EditorGUILayout.EndScrollView();
        }

        void DrawLog(LogMessage message)
        {
            if (Filtered(message.logType))
                return;

            EditorGUILayout.BeginHorizontal();
            DrawLogColorLabel(message);
            DrawLogFrameCount(message);
            DrawLogMessage(message);

            EditorGUILayout.EndHorizontal();

        }

        void DrawLogColorLabel(LogMessage message)
        {
            switch (message.logType)
            {
                case LogType.Log:
                    GUI.backgroundColor = Color.white;
                    break;
                case LogType.Warning:
                    GUI.backgroundColor = Color.yellow;
                    break;
                case LogType.Error:
                    GUI.backgroundColor = Color.red;
                    break;
                case LogType.Exception:
                    GUI.backgroundColor = Color.red;
                    break;
            }
            if (GUILayout.Button("", EditorStyles.helpBox, GUILayout.Width(10)))
                selected = message;
        }

        void DrawLogFrameCount(LogMessage message)
        {
            var style = EditorStyles.helpBox;
            style.normal.textColor = Color.black;
            style.alignment = TextAnchor.MiddleLeft;
            style.richText = true;

            int frameCount = message.frameCount;
            if (frameCount == lastLogFrameCount)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.white;

            if (GUILayout.Button((frameCount % 1000).ToString(), style, GUILayout.Width(30)))
                selected = message;
        }

        void DrawLogMessage(LogMessage message)
        {
            //GetIconContent(message.logType).text = message.message;
            content.text = message.message;

            //var style = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).textField);
            var style = EditorStyles.helpBox;
            style.alignment = TextAnchor.MiddleLeft;
            style.richText = true;

            //if (style.normal.background != null)
            //    text = style.normal.background.name;
            style.normal.background = whiteTexture;

            if (message == selected)
            {
                GUI.backgroundColor = new Color32(62, 125, 231, 255);
                style.normal.textColor = Color.white;
            }
            else
            {
                GUI.backgroundColor = Color.white;
                style.normal.textColor = Color.black;
            }
            GUI.color = Color.white;

            if (GUILayout.Button(content, style, GUILayout.ExpandWidth(true)))
            {
                if ((EditorApplication.timeSinceStartup - clickTime) < doubleClickTime)
                    OnLogDoubleClick();
                else
                    selected = message;
                clickTime = EditorApplication.timeSinceStartup;
            }
        }

        void OnLogDoubleClick()
        {
            int lineNumber = 0;
            string fileName = StackTraceExtractor.GetFirstPath(selected.stackTrace, out lineNumber);
            if (fileName == null)
                fileName = StackTraceExtractor.GetFileAndLineFromError(selected.message, out lineNumber);

            EditorTools.OpenScript(fileName, lineNumber);
        }

        bool Filtered(LogType logType)
        {
            switch (logType)
            {
                case LogType.Log:
                    if (showLog)
                        return false;
                    break;
                case LogType.Warning:
                    if (showWarning)
                        return false;
                    break;
                case LogType.Error:
                    if (showError)
                        return false;
                    break;
                case LogType.Exception:
                    if (showError)
                        return false;
                    break;
            }
            return true;
        }

        void Clear()
        {
            logs.Clear();
            logCount = 0;
            warningCount = 0;
            errorCount = 0;
        }

        GUIContent GetIconContent(LogType logType)
        {
            switch (logType)
            {
                case LogType.Log:
                    return GetIconContent("icons/console.infoicon.png", "");
                case LogType.Warning:
                    return GetIconContent("icons/console.warnicon.png", "");
                case LogType.Error:
                    return GetIconContent("icons/console.erroricon.png", "");
            }
            return content;
        }

        GUIContent GetIconContent(string path, string text)
        {
            Texture tex = EditorGUIUtility.Load(path) as Texture2D;

            var content = new GUIContent();
            content.text = text;
            content.image = tex;
            return content;
        }

        [MenuItem("Window/SuperConsole")]
        public static void ShowWindow()
        {
            GetWindow(typeof(SuperConsole));
        }

        public void OnBeforeSerialize()
        {
            bytes = ProtoBufSerializer.SerializeToBytes(logs);
        }

        public void OnAfterDeserialize()
        {
            if (bytes != null)
                logs = ProtoBufSerializer.DeserializeFromBytes<LogCollection>(bytes);
        }
    }
}