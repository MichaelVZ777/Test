using UnityEngine;
using System.Collections;
using UnityLib.Drawing;
using System;

namespace UnityLib.UI
{
    public class MouseController : MonoBehaviour
    {
        public event Action<Vector2, Vector2> OnRightDrag;
        public event Action<float> OnScroll;

        public bool normalizedOffset = true;

        bool dragging;
        Vector2 mouseDownPosition;
        Vector2 lastMousePosition;
        public Vector2 frameOffset { get; private set; }

        public void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                dragging = true;
                mouseDownPosition = Input.mousePosition;
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(1))
            {
                dragging = false;
            }

            if (dragging)
            {
                var totalOffset = mouseDownPosition - (Vector2)Input.mousePosition;
                frameOffset = lastMousePosition - (Vector2)Input.mousePosition;

                if (normalizedOffset)
                {
                    totalOffset /= Screen.height;
                    frameOffset /= Screen.height;
                }
                
                if (OnRightDrag != null)
                    OnRightDrag(totalOffset, frameOffset);
                lastMousePosition = Input.mousePosition;
            }

            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0 && OnScroll != null)
                OnScroll(scroll);
        }
    }
}