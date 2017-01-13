using System;

namespace SafeTween
{
    public abstract class TweenPropertyBase
    {
        public Tweener tweener;
        public bool isPlaying;
        public bool isReverse;

        public float startTime;
        public float endTime;

        protected Action OnPlayCompleted;
        protected Action OnReverseCompleted;
        protected Easings.Functions function = Easings.Functions.Linear;

        public abstract bool TargetNotNull();

        public void Play()
        {
            isPlaying = true;
            tweener.Play();
        }

        public void PlayReverse()
        {
            isPlaying = true;
            tweener.PlayBackward();
        }

        public virtual void Update(float time)
        {
            SetValue(time);

            if (!isReverse && time > endTime)
                OnPlayComplete();
            if (isReverse && time < startTime)
                OnReverseComplete();
        }

        public void SetValue(float time)
        {
            if (time >= startTime && time <= endTime)
            {
                var nTime = (time - startTime) / (endTime - startTime);
                UpdateValue(Easings.Interpolate(nTime, function));
            }
        }

        public virtual void UpdateValue(float normalizedTime)
        {
            throw new NotImplementedException();
        }

        void OnPlayComplete()
        {
            UpdateValue(1);

            if (OnPlayCompleted != null)
                OnPlayCompleted();
            isPlaying = false;
        }

        void OnReverseComplete()
        {
            UpdateValue(0);

            if (OnReverseCompleted != null)
                OnReverseCompleted();
            isPlaying = false;
        }

        public virtual TweenPropertyBase OnReverseComplete(Action OnPlayReverseCompleteAction)
        {
            OnReverseCompleted = OnPlayReverseCompleteAction;
            return this;
        }

        public virtual TweenPropertyBase OnPlayComplete(Action OnPlayCompleteAction)
        {
            OnPlayCompleted = OnPlayCompleteAction;
            return this;
        }

        public TweenPropertyBase SetInterpolation(Easings.Functions function)
        {
            this.function = function;
            return this;
        }
    }
}