using System.Collections.Generic;

namespace SafeTween
{
    public class SafeTweenManager : SingletonMonoManager<SafeTweenManager>
    {
        static List<Tweener> playing;
        static List<Tweener> adding;
        static List<Tweener> stopping;

        public override void Init()
        {
            adding = new List<Tweener>();
            playing = new List<Tweener>();
            stopping = new List<Tweener>();
        }

        public static void Stop(Tweener tweener)
        {
            if (_instance == null)
                CreateManager();
            adding.Remove(tweener);
            stopping.Add(tweener);
        }

        public static void Play(Tweener tweener)
        {
            if (_instance == null)
                CreateManager();
            adding.Add(tweener);
        }

        void Update()
        {
            foreach (var tweener in stopping)
                playing.Remove(tweener);
            stopping.Clear();

            foreach (var tweener in adding)
                if (!playing.Contains(tweener))
                    playing.Add(tweener);
            adding.Clear();

            foreach (var tweener in playing)
                tweener.Update();
        }
    }
}