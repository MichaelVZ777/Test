using UnityEngine;
using System.Collections;

namespace UnityLib.Proceduaral
{
    public static class RandomColor
    {
        static float h;
        public static float saturation = 0.5f;
        public static float brightness = 0.95f;

        const float golden_ratio_conjugate = 0.618033988749895f;

        public static void Reset()
        {
            h = Random.value;
        }

        public static Color GetColor()
        {
            h += golden_ratio_conjugate;
            h %= 1;
            return Color.HSVToRGB(h, saturation, brightness);
        }
    }
}