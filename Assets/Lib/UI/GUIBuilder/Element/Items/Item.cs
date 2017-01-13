using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.ComponentModel;

namespace UnityLib.UI
{
    public abstract class Item : MonoBehaviour, INotifyPropertyChanged, ILayoutElement
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public abstract void Init();
        public int height;
        public UIBuilder builder;
        public UIStyles styles { get { return overrideStyles == null ? builder.styles : overrideStyles; } }
        public UIStyles overrideStyles;

        public void SetFullWidth(int height)
        {
            var rectTransform = GetComponent<RectTransform>();
            var layoutElement = gameObject.AddComponent<LayoutElement>();
            layoutElement.minHeight = height;
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }

        public void CalculateLayoutInputHorizontal()
        {
        }

        public void CalculateLayoutInputVertical()
        {
        }

        public float minWidth { get { return height; } }

        public float preferredWidth { get { return height; } }

        public float flexibleWidth { get { return height; } }

        public float minHeight { get { return height; } }

        public float preferredHeight { get { return height; } }

        public float flexibleHeight { get { return height; } }

        public int layoutPriority { get { return height; } }
    }
}