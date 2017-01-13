using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace UnityLib.UI
{
    [Serializable]
    public class UIBuilder
    {
        public UIStyles styles;

        public UIBuilder()
        {
            styles = new UIStyles();
        }

        #region build options
        public FieldItem BuildFieldItem()
        {
            var go = NewElement("FieldItem");
            var fieldItem = go.AddComponent<FieldItem>();
            fieldItem.builder = this;
            return fieldItem;
        }

        public SliderItem BuildSliderItem()
        {
            var go = NewElement("SliderItem");
            var sliderItem = go.AddComponent<SliderItem>();
            sliderItem.builder = this;
            return sliderItem;
        }

        public ToggleItem BuildToggleItem()
        {
            var go = NewElement("ToggleItem");
            var toggleItem = go.AddComponent<ToggleItem>();
            toggleItem.builder = this;
            return toggleItem;
        }

        public Panel BuildPanel()
        {
            var panelObject = NewElement("Panel");
            var panel = panelObject.AddComponent<Panel>();
            panel.builder = this;

            return panel;
        }

        public InputField BuildInputField()
        {
            var go = NewElement("InputField");
            var inputField = go.AddComponent<InputField>();
            var content = BuildLabel();
            inputField.textComponent = content;

            content.transform.SetParent(go.transform);
            return inputField;
        }

        public Slider BuildSlider()
        {
            var go = NewElement("Slider");
            var slider = go.AddComponent<Slider>();

            var background = BuildImage("Background");
            background.color = styles.SliderBackgroundColor;
            background.transform.SetParent(go.transform);

            var fillRect = BuildImage("Fill");
            fillRect.color = styles.SliderFillColor;
            fillRect.rectTransform.anchorMax = new Vector2(0.5f, 1);
            fillRect.transform.SetParent(go.transform);
            slider.fillRect = fillRect.rectTransform;

            //force slider to update the fill rect container
            fillRect.gameObject.SetActive(true);

            return slider;
        }

        public Toggle BuildToggle()
        {
            var go = NewElement("Toggle");
            var toggle = go.AddComponent<Toggle>();

            var background = BuildImage("Background");
            background.color = styles.ToggleBackgroundColor;
            background.transform.SetParent(go.transform);
            toggle.targetGraphic = background;

            var checkmark = BuildImage("CheckMark");
            checkmark.color = styles.ToggleCheckmarkColor;
            checkmark.transform.SetParent(go.transform);
            checkmark.rectTransform.anchorMin = styles.TogglePadding.AnchorMin;
            checkmark.rectTransform.anchorMax = styles.TogglePadding.AnchorMax;
            toggle.graphic = checkmark;

            return toggle;
        }

        public Text BuildLabel(string name = "")
        {
            if (name == "")
                name = "Label";
            var go = NewElement(name);
            var text = go.AddComponent<Text>();
            text.color = Color.white;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

            return text;
        }

        public Image BuildImage(string name = "")
        {
            var go = NewElement(name == "" ? "Image" : name);
            var image = go.AddComponent<Image>();
            image.color = Color.white;

            return image;
        }

        public GameObject NewElement(string name)
        {
            var go = new GameObject(name);
            var rectTransform = go.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.sizeDelta = new Vector2(0, 0);
            rectTransform.anchoredPosition = new Vector2(0, 0);
            return go;
        }
        #endregion
    }

    public enum LayoutType
    {
        Vertical,
        Horizonatal
    }

    public enum ItemType
    {
        Custom,
        Field,
        Label
    }
}