using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UnityLib.UI
{
    public class UIBuilderConfig : EditorWindow
    {
        public bool buildOnCompile;

        void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbarButton);
            buildOnCompile = GUILayout.Toggle(buildOnCompile, "Build on compile", EditorStyles.toolbarButton);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        void OnEnable()
        {
            titleContent = GetIconContent("icons/UnityEditor.ConsoleWindow.png", "UIBuilder");
            LoadPreference();
        }

        void OnDisable()
        {
            SavePreference();
        }

        void LoadPreference()
        {
            buildOnCompile = EditorPrefs.GetBool("UIBuilder-buildOnCompile");
        }

        void SavePreference()
        {
            EditorPrefs.SetBool("UIBuilder-buildOnCompile", buildOnCompile);
        }

        [MenuItem("Window/UIBuilder")]
        public static void ShowWindow()
        {
            GetWindow(typeof(UIBuilderConfig));
        }

        GUIContent GetIconContent(string path, string text)
        {
            Texture tex = EditorGUIUtility.Load(path) as Texture2D;

            var content = new GUIContent();
            content.text = text;
            content.image = tex;
            return content;
        }
    }
}