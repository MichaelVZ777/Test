using UnityEngine;
using System.Collections.Generic;
using Random = System.Random;
using UnityLib.Primitives;

namespace UnityLib.Proceduaral
{
    public class ColorRoller
    {
        List<Color> rolledColors;
        List<Color> excludeColors;

        public float cullLimit;
        Random random;

        public ColorRoller()
        {
            rolledColors = new List<Color>();
            excludeColors = new List<Color>();
            random = new Random();
        }

        public void AddExcludeColor(Color color)
        {
            excludeColors.Add(color);
        }

        public void Clear()
        {
            rolledColors.Clear();
        }

        public Color Roll()
        {
            var color = Reroll();

            int i = 0;
            //avoid AI color
            while (!CheckExcludedColor(color) && i++ < 100)
                color = Reroll();

            i = 0;
            //avoid other player color
            while (!CheckRolledColor(color) && i++ < 100)
                color = Reroll();


            rolledColors.Add(color);
            return color;
        }

        bool CheckExcludedColor(Color color)
        {
            foreach (var excludedColor in excludeColors)
                if (GetDifferent(color, excludedColor) < cullLimit)
                    return false;
            return true;
        }

        bool CheckRolledColor(Color color)
        {
            foreach (var rolledColor in rolledColors)
                if (GetDifferent(color, rolledColor) < cullLimit)
                    return false;
            return true;
        }

        Color Reroll()
        {
            return new HSBColor(GetRandom01(), GetRandom01(), 1).ToColor();
        }

        float GetRandom01()
        {
            return (float)random.NextDouble();
        }

        float GetDifferent(Color c1, Color c2)
        {
            return Mathf.Abs(c1.r - c2.r) + Mathf.Abs(c1.g - c2.g) + Mathf.Abs(c1.b - c2.b);
        }
    }
}