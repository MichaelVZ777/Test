using UnityEngine;
using System.Collections;

namespace UnityLib.Extension
{
    public static class VectorExtension
    {
        public static Vector2 Rotate(this Vector2 v, float d)
        {
            return new Vector2(v.x * Mathf.Cos(d) - v.y * Mathf.Sin(d), v.x * Mathf.Sin(d) + v.y * Mathf.Cos(d));
        }
    }
}