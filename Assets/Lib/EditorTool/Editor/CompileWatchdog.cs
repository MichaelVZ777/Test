using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace UnityLib.EditorTool
{
    [InitializeOnLoad]
    public class CompileWatchdog
    {
        public static event Action OnCompile;
        public static bool compiling;

        static CompileWatchdog()
        {
            Unused(_instance);
            _instance = new CompileWatchdog();
        }

        private CompileWatchdog()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        ~CompileWatchdog()
        {
            EditorApplication.update -= OnEditorUpdate;
            // Silence the unused variable warning with an if.
            _instance = null;
        }

        // Called each time the editor updates.
        private static void OnEditorUpdate()
        {
            if (EditorApplication.isCompiling)
                compiling = true;

            if (!EditorApplication.isCompiling && compiling)
            {
                compiling = false;

                if (OnCompile != null)
                    OnCompile();
            }
        }

        // Used to silence the 'is assigned by its value is never used' warning for _instance.
        private static void Unused<T>(T unusedVariable) { }

        private static CompileWatchdog _instance = null;
    }
}