using UnityEngine;
using UnityEditor;
using System.ComponentModel;
using System.Collections.Generic;
using System;

namespace UnityLib.EditorTool
{
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class EnhancedInspector : Editor
    {
        static HashSet<Type> excludedClass = new HashSet<Type> { typeof(MeshFilter) };

        PropertyField[] m_fields;

        void OnEnable()
        {
            if (target != null)
                m_fields = ExposeProperties.GetProperties(target);
            var inpc = target as INotifyPropertyChanged;
            if (inpc != null)
                inpc.PropertyChanged += PropertyChangedEventHandler;
        }

        void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            Repaint();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            //Draw EditorButton
            foreach (var method in target.GetType().GetMethods())
                foreach (var attribue in method.GetCustomAttributes(true))
                {
                    var editorButton = attribue as EditorButton;
                    if (editorButton != null)
                        if (GUILayout.Button(editorButton.name == null ? method.Name : editorButton.name))
                            method.Invoke(target, null);
                }

            //Draw Property
            if (excludedClass.Contains(target.GetType()))
                return;
            ExposeProperties.Expose(m_fields);
        }
    }
}