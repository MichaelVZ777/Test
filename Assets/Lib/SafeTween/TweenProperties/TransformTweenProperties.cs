using UnityEngine;

namespace SafeTween
{
    public class TweenLocalPosition : TweenPropertyUnityGeneric<Transform>
    {
        public Vector3 start;
        public Vector3 end;

        public TweenLocalPosition(Transform transform, Vector3 start, Vector3 end, float startTime, float endTime)
        {
            target = transform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.localPosition = Vector3.Lerp(start, end, time);
        }
    }

    public class TweenPosition : TweenPropertyUnityGeneric<Transform>
    {
        public Vector3 start;
        public Vector3 end;

        public TweenPosition(Transform transform, Vector3 start, Vector3 end, float startTime, float endTime)
        {
            target = transform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.position = Vector3.Lerp(start, end, time);
        }
    }

    public class TweenLocalRotation : TweenPropertyUnityGeneric<Transform>
    {
        public Quaternion start;
        public Quaternion end;
        public Vector3 startEuler;
        public Vector3 endEuler;
        public bool eular;

        public TweenLocalRotation(Transform transform, Quaternion start, Quaternion end, float startTime, float endTime)
        {
            target = transform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public TweenLocalRotation(Transform transform, Vector3 start, Vector3 end, float startTime, float endTime)
        {
            target = transform;
            this.startEuler = start;
            this.endEuler = end;
            this.startTime = startTime;
            this.endTime = endTime;
            eular = true;
        }

        public override void UpdateValue(float time)
        {
            if (eular)
                target.localRotation = Quaternion.Euler(Vector3.Slerp(startEuler, endEuler, time));
            else
                target.localRotation = Quaternion.Slerp(start, end, time);
        }
    }

    public class TweenRotation : TweenPropertyUnityGeneric<Transform>
    {
        public Quaternion start;
        public Quaternion end;
        public Vector3 startEuler;
        public Vector3 endEuler;
        public bool eular;

        public TweenRotation(Transform transform, Quaternion start, Quaternion end, float startTime, float endTime)
        {
            target = transform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public TweenRotation(Transform transform, Vector3 start, Vector3 end, float startTime, float endTime)
        {
            target = transform;
            this.startEuler = start;
            this.endEuler = end;
            this.startTime = startTime;
            this.endTime = endTime;
            eular = true;
        }

        public override void UpdateValue(float time)
        {
            if (eular)
                target.rotation = Quaternion.Euler(Vector3.Slerp(startEuler, endEuler, time));
            else
                target.rotation = Quaternion.Slerp(start, end, time);
        }
    }

    public class TweenLocalScale: TweenPropertyUnityGeneric<Transform>
    {
        public Vector3 start;
        public Vector3 end;

        public TweenLocalScale(Transform transform, Vector3 start, Vector3 end, float startTime, float endTime)
        {
            target = transform;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.localScale = Vector3.Lerp(start, end, time);
        }
    }
}