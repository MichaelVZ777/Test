using UnityEngine;

namespace SafeTween
{
    public class TweenAnchorPosition : TweenPropertyUnityGeneric<RectTransform>
    {
        public Vector2 start;
        public Vector2 end;

        public TweenAnchorPosition(RectTransform rectTransform, Vector2 start, Vector2 end, float startTime, float endTime)
        {
            target = rectTransform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.anchoredPosition = Vector2.Lerp(start, end, time);
        }
    }

    public class TweenSizeDelta : TweenPropertyUnityGeneric<RectTransform>
    {
        public Vector2 start;
        public Vector2 end;

        public TweenSizeDelta(RectTransform rectTransform, Vector2 start, Vector2 end, float startTime, float endTime)
        {
            target = rectTransform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.sizeDelta = Vector2.Lerp(start, end, time);
        }
    }

    public class TweenSizeDeltaX : TweenPropertyUnityGeneric<RectTransform>
    {
        public float start;
        public float end;

        public TweenSizeDeltaX(RectTransform rectTransform, float start, float end, float startTime, float endTime)
        {
            target = rectTransform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            var sizeDelta = target.sizeDelta;
            target.sizeDelta = Vector2.Lerp(new Vector2(start, sizeDelta.y), new Vector2(end, sizeDelta.y), time);
        }
    }
}