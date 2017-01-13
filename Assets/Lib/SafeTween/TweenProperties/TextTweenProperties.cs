using UnityEngine;
using UnityEngine.UI;

namespace SafeTween
{
    public class TweenFontSize : TweenPropertyUnityGeneric<Text>
    {
        public int start;
        public int end;

        public TweenFontSize(Text text, int start, int end, float startTime, float endTime)
        {
            this.target = text;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.fontSize = Mathf.RoundToInt(Mathf.Lerp(start, end, time));
        }
    }

    public class TweenTextColor : TweenPropertyUnityGeneric<Text>
    {
        public Color start;
        public Color end;

        public TweenTextColor(Text text, Color start, Color end, float startTime, float endTime)
        {
            this.target = text;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.color = Color.Lerp(start, end, time);
        }
    }

    public class TweenTextAlpha : TweenPropertyUnityGeneric<Text>
    {
        public float start;
        public float end;

        public TweenTextAlpha(Text text, float start, float end, float startTime, float endTime)
        {
            this.target = text;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            var color = target.color;
            color.a = Mathf.Lerp(start, end, time);
            target.color = color;
        }
    }
}