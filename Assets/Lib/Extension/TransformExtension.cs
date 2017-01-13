using UnityEngine;
using System.Collections.Generic;

namespace UnityLib.Extension
{
    public static class TransformExtension
    {
        public static void SetLayer(this GameObject gameObject, int layer)
        {
            gameObject.transform.SetLayer(layer);
        }

        public static void SetLayer(this Transform transform, int layer)
        {
            transform.gameObject.layer = layer;
            foreach (Transform child in transform)
                SetLayer(child, layer);
        }

        public static void DestroyChilds(this Transform transform)
        {
            var childs = new List<Transform>();
            foreach (Transform child in transform)
                childs.Add(child);

            for (int i = 0; i < childs.Count; i++)
                GameObject.Destroy(childs[i].gameObject);
        }
    }
}