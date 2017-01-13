using UnityEngine;

namespace SafeTween
{
    public class TweenCameraFOV : TweenPropertyUnityGeneric<Camera>
    {
        public float start;
        public float end;

        public TweenCameraFOV(Camera camera, float start, float end, float startTime, float endTime)
        {
            target = camera;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.fieldOfView = Mathf.Lerp(start, end, time);
        }
    }

    public class TweenCameraColor : TweenPropertyUnityGeneric<Camera>
    {
        public Color start;
        public Color end;

        public TweenCameraColor(Camera camera, Color start, Color end, float startTime, float endTime)
        {
            target = camera;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.backgroundColor = Color.Lerp(start, end, time);
        }
    }
}