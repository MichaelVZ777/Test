using UnityEngine;

namespace SafeTween
{
    public class TweenCanvasGroupAlpha : TweenPropertyUnityGeneric<CanvasGroup>
    {
        public float start;
        public float end;

        public TweenCanvasGroupAlpha(CanvasGroup target, float start, float end, float startTime, float endTime)
        {
            this.target = target;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.alpha = Mathf.Lerp(start, end, time);
        }
    }
}