using UnityEngine;

namespace SafeTween
{
    public class TweenAmbientColor : TweenPropertyBase
    {
        public Color start;
        public Color end;

        public TweenAmbientColor(Color start, Color end, float startTime, float endTime)
        {
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            RenderSettings.ambientLight = Color.Lerp(start, end, time);
        }

        public override bool TargetNotNull() { return true; }
    }

    public class TweenFogColor : TweenPropertyBase
    {
        public Color start;
        public Color end;

        public TweenFogColor(Color start, Color end, float startTime, float endTime)
        {
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            RenderSettings.fogColor = Color.Lerp(start, end, time);
        }

        public override bool TargetNotNull() { return true; }
    }

    public class TweenReflectionIntensity : TweenPropertyBase
    {
        public float start;
        public float end;

        public TweenReflectionIntensity(float start, float end, float startTime, float endTime)
        {
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            RenderSettings.reflectionIntensity = Mathf.Lerp(start, end, time);
        }

        public override bool TargetNotNull() { return true; }
    }

    public class TweenFogStartDistance : TweenPropertyBase
    {
        public float start;
        public float end;

        public TweenFogStartDistance(float start, float end, float startTime, float endTime)
        {
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            RenderSettings.fogStartDistance = Mathf.Lerp(start, end, time);
        }

        public override bool TargetNotNull() { return true; }
    }

    public class TweenFogEndDistance : TweenPropertyBase
    {
        public float start;
        public float end;

        public TweenFogEndDistance(float start, float end, float startTime, float endTime)
        {
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            RenderSettings.fogEndDistance = Mathf.Lerp(start, end, time);
        }

        public override bool TargetNotNull() { return true; }
    }
}