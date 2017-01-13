#if UNITY_5_3_OR_NEWER

using UnityEngine;
using UnityEngine.UI;

namespace UnityLib.UI
{
    public class Reticle : MonoBehaviour
    {
        public GameObject reticleTarget;
        public GazeInputModule input;
        public Image ring;

        void LateUpdate()
        {
            transform.position = reticleTarget.transform.position;
            transform.rotation = reticleTarget.transform.rotation;
        }
    }
}

#endif