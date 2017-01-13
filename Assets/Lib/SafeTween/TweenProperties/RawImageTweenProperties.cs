using UnityEngine;
using UnityEngine.UI;

namespace SafeTween
{
    public class TweenRawImageColor : TweenPropertyUnityGeneric<RawImage>
    {
        public Color start;
        public Color end;

        public TweenRawImageColor(RawImage rawImage, Color start, Color end, float startTime, float endTime)
        {
            target = rawImage;
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
}