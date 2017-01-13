using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.ComponentModel;
using System.Reflection;
using UnityLib.DataBinding;

namespace UnityLib.UI
{
    public class SliderItem : Item
    {
        Text label;
        Slider slider;
        Image background;

        string _label;
        public string Label { get { if (label == null) return null; return label.text; } set { if (label == null) return; label.text = value; } }

        public override void Init()
        {
            label = builder.BuildLabel();
            slider = builder.BuildSlider();

            label.resizeTextForBestFit = true;
            label.rectTransform.anchorMin = new Vector2(0, 0);
            label.rectTransform.anchorMax = new Vector2(0.5f, 1);
            label.rectTransform.pivot = new Vector2(0, 0.5f);

            var fieldRectTransform = slider.GetComponent<RectTransform>();
            fieldRectTransform.anchorMin = new Vector2(0.5f, 0);
            fieldRectTransform.anchorMax = new Vector2(1, 1);
            fieldRectTransform.pivot = new Vector2(1, 0.5f);
            slider.targetGraphic = GetComponent<Image>();

            label.transform.SetParent(transform, false);
            slider.transform.SetParent(transform, false);
        }

        public void Bind(INotifyPropertyChanged o, PropertyInfo property)
        {
            var binding = slider.gameObject.AddComponent<SliderPropertyBinding>();
            binding.Bind(o, property, slider);
        }
    }
}