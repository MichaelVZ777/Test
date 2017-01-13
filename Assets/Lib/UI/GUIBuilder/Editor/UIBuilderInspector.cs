using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UnityLib.UI
{
    [CustomEditor(typeof(UIBuilder), true)]
    public class UIBuilderInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            //foreach (var method in target.GetType().GetMethods())
            //    foreach (var attribue in method.GetCustomAttributes(true))
            //    {
            //        var editorButton = attribue as EditorButton;
            //        if (editorButton != null)
            //            if (GUILayout.Button(editorButton.name == null ? method.Name : editorButton.name))
            //                method.Invoke(target, null);
            //    }
        }
    }
}