using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityLib.CI
{
    public class BuildInfo : MonoBehaviour
    {
        void Awake()
        {
            var text = GetComponent<Text>();
            if (text != null)
                text.text = GitHash;
        }

        public static string GitHash
        {
            get
            {
                var buildInfo = Resources.Load("buildInfo") as TextAsset;
                if (buildInfo != null)
                    return buildInfo.text;
                else
                    return "n/a";
            }
        }
    }
}