using UnityEngine;
using UnityLib.EditorTool;
using UnityLib.Extension;

namespace UnityLib.UI
{
    public class CircleSpread : MonoBehaviour
    {
        public float degreeOffset;
        public float spreadRange;
        public float distance;

        [EditorButton]
        public void Spread()
        {
            int i = 0;
            var startVector = new Vector2(0, distance);

            foreach (Transform child in transform)
            {
#if UNITY_EDITOR
                UnityEditor.PrefabUtility.ResetToPrefabState(child.gameObject);
#endif
                var degree = Mathf.Lerp(degreeOffset, degreeOffset + spreadRange, (float)i++ / transform.childCount);
                child.localRotation = Quaternion.Euler(0, 360 - degree, 0);
                var position = startVector.Rotate(degree * Mathf.Deg2Rad);
                child.localPosition = new Vector3(position.x, 0, position.y);
            }
        }

        void OnValidate()
        {
            Spread();
        }
    }
}