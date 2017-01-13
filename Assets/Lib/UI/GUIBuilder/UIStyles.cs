using UnityEngine;
using System.Collections;

namespace UnityLib.UI
{
    public class UIStyles
    {
        public Color TextColor = new Color(1, 1, 1, 1);


        //Field
        public Color FieldBackgroundColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        public Color FieldPlaceholderColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);

        //Slider
        public Color SliderBackgroundColor = new Color(1, 1, 1, 0.3f);
        public Color SliderFillColor = new Color(1, 1, 1, 0.5f);

        //Toggle
        public Color ToggleBackgroundColor = new Color(1, 1, 1, 0.3f);
        public Color ToggleCheckmarkColor = new Color(1, 1, 1, 1);
        public Padding TogglePadding = Padding.EvenPadding(0.3f);
    }

    public struct Padding
    {
        public float top, buttom, left, right;

        public Padding(float top, float buttom, float left, float right)
        {
            this.top = top;
            this.buttom = buttom;
            this.left = left;
            this.right = right;
        }

        public Vector2 AnchorMin
        {
            get { return new Vector2(left, buttom); }
        }

        public Vector2 AnchorMax
        {
            get { return new Vector2(1 - right, 1 - top); }
        }

        public static Padding EvenPadding(float padding)
        {
            return new Padding(padding, padding, padding, padding);
        }
    }
}