using UnityEditor;

namespace SuperConsole
{
    public static class EditorTools
    {
        public static void OpenScript(string path, int lineNumber)
        {
            var script = AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
            AssetDatabase.OpenAsset(script.GetInstanceID(), lineNumber);
        }
    }
}