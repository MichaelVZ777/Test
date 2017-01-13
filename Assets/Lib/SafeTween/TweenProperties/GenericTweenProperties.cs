using System;

namespace SafeTween
{
    public class TweenGeneric<T> : TweenPropertyGeneric<T> where T : new()
    {
        Action<T, float> updateFunction;

        public TweenGeneric(T o, Action<T, float> updateFunction, float startTime, float endTime)
        {
            target = o;
            this.updateFunction = updateFunction;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            updateFunction(target, time);
        }
    }
}