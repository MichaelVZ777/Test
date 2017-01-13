using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Reflection;
using UnityLib.DataBinding;
using System.ComponentModel;

namespace UnityLib.UI
{
    public class FieldItem : Item
    {
        Text label;
        InputField inputField;

        public string Label { get { if (label == null) return null; return label.text; } set { if (label == null) return; label.text = value; } }

        public override void Init()
        {
            label = builder.BuildLabel();
            inputField = builder.BuildInputField();

            label.resizeTextForBestFit = true;
            label.rectTransform.anchorMin = new Vector2(0, 0);
            label.rectTransform.anchorMax = new Vector2(0.5f, 1);
            label.rectTransform.pivot = new Vector2(0, 0.5f);

            var fieldRectTransform = inputField.GetComponent<RectTransform>();
            fieldRectTransform.anchorMin = new Vector2(0.5f, 0);
            fieldRectTransform.anchorMax = new Vector2(1, 1);
            fieldRectTransform.pivot = new Vector2(1, 0.5f);
            inputField.textComponent.supportRichText = false;
            inputField.targetGraphic = GetComponent<Image>();

            label.transform.SetParent(transform, false);
            inputField.transform.SetParent(transform, false);

            AddBackground();
        }

        public void SetPlaceholder(string text)
        {
            var placeholder = inputField.placeholder as Text;
            if (placeholder != null)
                placeholder.text = text;
            else
            {
                var newPlaceholder = builder.BuildLabel();
                newPlaceholder.text = text;
                newPlaceholder.transform.SetParent(inputField.transform);
            }
        }

        public void AddBackground()
        {
            var background = inputField.gameObject.AddComponent<Image>();
            background.color = styles.FieldBackgroundColor;
        }

        public void Bind(INotifyPropertyChanged o, PropertyInfo property)
        {
            var binding = inputField.gameObject.AddComponent<InputFieldPropertyBinding>();
            binding.Bind(o, property, inputField);
        }
    }
}