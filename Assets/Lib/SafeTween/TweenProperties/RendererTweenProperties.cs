using UnityEngine;

namespace SafeTween
{
    public class TweenMaterialColor : TweenPropertyUnityGeneric<Renderer>
    {
        Color start;
        Color end;
        string colorName;

        public TweenMaterialColor(Renderer renderer, string colorName, Color start, Color end, float startTime, float endTime)
        {
            target = renderer;
            this.colorName = colorName;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            target.material.SetColor(colorName, Color.Lerp(start, end, time));
        }
    }

    public class TweenMaterialColorAlpha : TweenPropertyUnityGeneric<Renderer>
    {
        float start;
        float end;
        string colorName;

        public TweenMaterialColorAlpha(Renderer renderer, string colorName, float start, float end, float startTime, float endTime)
        {
            target = renderer;
            this.colorName = colorName;
            this.start = start;
            this.end = end;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override void UpdateValue(float time)
        {
            var color = target.material.GetColor(colorName);
            color.a = Mathf.Lerp(start, end, time);
            target.material.SetColor(colorName, color);
        }
    }
}