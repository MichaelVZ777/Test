using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UnityLib.UI
{
    public class Panel : MonoBehaviour
    {
        public UIBuilder builder;
        public int itemHeight = 20;

        public void Init(UIBuilder builder)
        {
            this.builder = builder;
        }

        public void SetLayout(LayoutType type)
        {
            HorizontalOrVerticalLayoutGroup layoutGroup = null;

            switch (type)
            {
                case LayoutType.Vertical:
                    layoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
                    layoutGroup.childForceExpandWidth = true;
                    layoutGroup.childForceExpandHeight = false;
                    break;
                case LayoutType.Horizonatal:
                    layoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
                    layoutGroup.childForceExpandWidth = false;
                    layoutGroup.childForceExpandHeight = true;
                    break;
            }
        }

        public FieldItem AddField(string name)
        {
            var field = builder.BuildFieldItem();
            field.transform.SetParent(transform, false);
            field.Init();
            field.Label = name;
            field.SetFullWidth(itemHeight);
            return field;
        }

        public SliderItem AddSlider(string name)
        {
            var slider = builder.BuildSliderItem();
            slider.transform.SetParent(transform, false);
            slider.Init();
            slider.Label = name;
            slider.SetFullWidth(itemHeight);
            return slider;
        }

        public ToggleItem AddToggle(string name)
        {
            var toggle = builder.BuildToggleItem();
            toggle.transform.SetParent(transform, false);
            toggle.Init();
            toggle.Label = name;
            toggle.SetFullWidth(itemHeight);
            return toggle;
        }
    }
}