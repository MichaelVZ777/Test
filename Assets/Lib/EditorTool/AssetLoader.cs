using UnityEngine;
using System.IO;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;

namespace UnityLib.EditorTool
{
    public class AssetsLoader
    {
        public static List<Texture2D> LoadAllTextures(string path)
        {
            var result = new List<Texture2D>();

            string[] files = Directory.GetFiles(Application.dataPath + path);
            foreach (string file in files)
            {
                //ignore .meta
                var extension = Path.GetExtension(file);
                if (extension == ".meta")
                    continue;

                //load asset
                var relativePath = file.Replace(Application.dataPath, "");
                result.Add(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets" + relativePath));
            }

            return result;
        }
    }
}
#endif