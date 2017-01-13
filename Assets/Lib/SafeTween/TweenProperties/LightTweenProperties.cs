using UnityEngine;

namespace SafeTween
{
    public class TweenLightIntensity : TweenPropertyUnityGeneric<Light>
    {
        public float start;
        public float end;

        public TweenLightIntensity(Light light, float start, float end, float startTime, float endTime)
        {
            target = light;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.intensity = Mathf.Lerp(start, end, time);
        }
    }

    public class TweenLightRange: TweenPropertyUnityGeneric<Light>
    {
        public float start;
        public float end;

        public TweenLightRange(Light light, float start, float end, float startTime, float endTime)
        {
            target = light;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.range = Mathf.Lerp(start, end, time);
        }
    }

    public class TweenLightSpotAngle: TweenPropertyUnityGeneric<Light>
    {
        public float start;
        public float end;

        public TweenLightSpotAngle(Light light, float start, float end, float startTime, float endTime)
        {
            target = light;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.spotAngle = Mathf.Lerp(start, end, time);
        }
    }   
}