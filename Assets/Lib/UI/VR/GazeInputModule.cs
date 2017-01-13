#if UNITY_5_3_OR_NEWER

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.VR;
using System.ComponentModel;

namespace UnityLib.UI
{
    public class GazeInputModule : PointerInputModule, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public enum Mode { Click = 0, Gaze };
        public Mode mode;

        public float gazeTime = 2;
        public float gazeProgress { get; set; }

        public RaycastResult CurrentRaycast;

        private PointerEventData pointerEventData;
        private GameObject currentLookAtHandler;
        bool clicked;

        public override void Process()
        {
            HandleLook();
            HandleSelection();
        }

        void HandleLook()
        {
            if (pointerEventData == null)
            {
                pointerEventData = new PointerEventData(eventSystem);
            }
            // fake a pointer always being at the center of the screen
            pointerEventData.position = new Vector2(VRSettings.eyeTextureWidth / 2, VRSettings.eyeTextureHeight / 2);

#if UNITY_EDITOR
            pointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);
#endif

            pointerEventData.delta = Vector2.zero;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerEventData, raycastResults);
            CurrentRaycast = pointerEventData.pointerCurrentRaycast = FindFirstRaycast(raycastResults);
            ProcessMove(pointerEventData);
        }

        void HandleSelection()
        {
            if (pointerEventData.pointerEnter != null)
            {
                // if the ui receiver has changed, reset the gaze delay timer
                GameObject handler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(pointerEventData.pointerEnter);
                if (currentLookAtHandler != handler)
                {
                    currentLookAtHandler = handler;
                    ResetClickState();
                }
                else
                    gazeProgress += Time.deltaTime / gazeTime;

                // if we have a handler and it's time to click, do it now
                if (!clicked && currentLookAtHandler != null &&
                    ((mode == Mode.Gaze && gazeProgress > 1) ||
                    (mode == Mode.Click && Input.GetMouseButtonUp(0))))
                {
                    clicked = true;
                    ExecuteEvents.ExecuteHierarchy(currentLookAtHandler, pointerEventData, ExecuteEvents.pointerClickHandler);
                }
            }
            else
            {
                currentLookAtHandler = null;
                ResetClickState();
            }
        }

        void ResetClickState()
        {
            gazeProgress = 0;
            clicked = false;
        }
    }
}

#endif