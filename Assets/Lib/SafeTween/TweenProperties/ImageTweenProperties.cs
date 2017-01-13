using UnityEngine;
using UnityEngine.UI;

namespace SafeTween
{
    public class TweenImageFill : TweenPropertyUnityGeneric<Image>
    {
        public float start;
        public float end;

        public TweenImageFill(Image image, float start, float end, float startTime, float endTime)
        {
            target = image;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.fillAmount = Mathf.Lerp(start, end, time);
        }
    }

    public class TweenImageColor : TweenPropertyUnityGeneric<Image>
    {
        public Color start;
        public Color end;

        public TweenImageColor(Image image, Color start, Color end, float startTime, float endTime)
        {
            target = image;
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

    public class TweenImageAlpha : TweenPropertyUnityGeneric<Image>
    {
        public float start;
        public float end;

        public TweenImageAlpha(Image image, float start, float end, float startTime, float endTime)
        {
            target = image;
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