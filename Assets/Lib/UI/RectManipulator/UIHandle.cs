using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityLib.UI
{
    [RequireComponent(typeof(Image))]
    public class UIHandle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action OnMouseDown;
        public event Action OnMouseUp;
        public event Action OnMouseEnter;
        public event Action OnMouseExit;


        public Color normal = Color.white;
        public Color hightlight = Color.green;
        public Color pressed = Color.yellow;

        public Vector2 mouseDownOffset { get; private set; }
        public bool isMouseDown { get; private set; }
        public bool isMouseInside { get; private set; }
        public bool controlColor = true;

        public void OnPointerDown(PointerEventData eventData)
        {
            isMouseDown = true;
            mouseDownOffset = Input.mousePosition - transform.position;
            SetColor(pressed);

            if (OnMouseDown != null)
                OnMouseDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isMouseDown = false;

            if (isMouseInside)
                SetColor(hightlight);
            else
                SetColor(normal);

            if (OnMouseUp != null)
                OnMouseUp();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isMouseInside = true;
            if (!isMouseDown)
                SetColor(hightlight);

            if (OnMouseEnter != null)
                OnMouseEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMouseInside = false;
            if (!isMouseDown)
                SetColor(normal);

            if (OnMouseExit != null)
                OnMouseExit();
        }

        void SetColor(Color color)
        {
            if (controlColor)
                GetComponent<Image>().color = color;
        }
    }
}