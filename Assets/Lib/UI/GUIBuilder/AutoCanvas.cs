using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityLib.UI
{
    [HideInInspector]
    [System.Serializable]
    public class AutoCanvas
    {
        public Canvas canvas;
        public CanvasScaler canvasScaler;
        public GraphicRaycaster graphicRaycaster;
        public UIStyles styles;

        public AutoCanvas()
        {
            styles = new UIStyles();
        }

        public void Destroy()
        {
            if (canvas == null)
                return;

            if (Application.isPlaying)
                Object.Destroy(canvas.gameObject);
            else
                Object.DestroyImmediate(canvas.gameObject);
        }

        public void Init()
        {
            var canvasObject = new GameObject("NewCanvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            graphicRaycaster = canvasObject.AddComponent<GraphicRaycaster>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        }

        public Panel AddPanel()
        {
            return AddPanel(new UIBuilder());
        }

        public Panel AddPanel(UIBuilder builder)
        {
            var panel = builder.BuildPanel();
            panel.transform.SetParent(canvas.transform, false);
            return panel;
        }
    }
}