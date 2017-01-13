using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityLib.DataBinding;
using System.Reflection;
using System.ComponentModel;

namespace UnityLib.UI
{
    public class ToggleItem : Item
    {
        Text label;
        Toggle toggle;

        public string Label { get { if (label == null) return null; return label.text; } set { if (label == null) return; label.text = value; } }

        public override void Init()
        {
            label = builder.BuildLabel();
            toggle = builder.BuildToggle();

            label.resizeTextForBestFit = true;
            label.rectTransform.anchorMin = new Vector2(0, 0);
            label.rectTransform.anchorMax = new Vector2(0.5f, 1);
            label.rectTransform.pivot = new Vector2(0, 0.5f);

            var fieldRectTransform = toggle.GetComponent<RectTransform>();
            fieldRectTransform.anchorMin = new Vector2(0.5f, 0);
            fieldRectTransform.anchorMax = new Vector2(0.6f, 1);
            fieldRectTransform.pivot = new Vector2(1, 0.5f);
            toggle.targetGraphic = GetComponent<Image>();

            label.transform.SetParent(transform, false);
            toggle.transform.SetParent(transform, false);
        }

        public void Bind(INotifyPropertyChanged o, PropertyInfo property)
        {
            var binding = toggle.gameObject.AddComponent<TogglePropertyBinding>();
            binding.Bind(o, property, toggle);
        }
    }
}