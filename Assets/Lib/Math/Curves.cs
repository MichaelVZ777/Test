using UnityEngine;

namespace UnityLib.Math
{
    public static class Curves
    {
        public static float EaseInCircular(float t)
        {
            return 1 - Mathf.Sqrt(1 - t * t);
        }
    }
}