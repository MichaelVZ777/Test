using UnityEngine;

namespace UnityLib.Utility
{
    public class Interval
    {
        float start, end;
        int index;
        int level;
        int levelCount;

        public Interval(float start, float end)
        {
            this.start = start;
            this.end = end;
            level = 0;
            index = 0;
            levelCount = 0;
        }

        public float Sample()
        {
            float interval = end - start;
            float segment = Mathf.Pow(0.5f, (float)level) * interval;
            float lastSegment = Mathf.Pow(0.5f, (float)(level - 1)) * interval;

            if (index == levelCount)
            {
                index = 0;
                levelCount = (int)Mathf.Pow(2, (float)level);
                level++;
            }

            return index++ * lastSegment + segment;
        }

        public void Clear()
        {
            index = 0;
            level = 0;
            levelCount = 0;
        }
    }
}