using UnityEngine;
using SafeTween;
using UnityLib.UI;

namespace UnityLib.CameraTool
{
    public class CameraControl : MonoBehaviour
    {
        public MouseController controller;
        public Camera cam;
        public float scrollSensitivity;
        public float minHeight;
        public float maxHeight;

        float frustumHeight;
        float lastFrustumHeight;
        Vector3 targetPosition;

        void Awake()
        {
            cam = GetComponent<Camera>();
            targetPosition = transform.position;
            CalculateFrustomHeight(targetPosition.y);
        }

        void OnEnable()
        {
            controller.OnRightDrag += OnRightDrag;
            controller.OnScroll += OnScroll;
        }

        void OnDisable()
        {
            controller.OnRightDrag -= OnRightDrag;
            controller.OnScroll -= OnScroll;
        }

        void OnRightDrag(Vector2 totalOffset, Vector2 frameOffset)
        {
            var position = transform.position;
            transform.position = new Vector3(position.x + frameOffset.x * frustumHeight, position.y, position.z + frameOffset.y * frustumHeight);
            targetPosition = transform.position;
        }

        void OnScroll(float scroll)
        {
            targetPosition.y -= scroll * targetPosition.y * 0.01f * scrollSensitivity;
            targetPosition.y = Mathf.Clamp(targetPosition.y, minHeight, maxHeight);
            var offset = (Vector2)Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2);
            offset /= Screen.height;

            CalculateFrustomHeight(targetPosition.y);
            var frustumDelta = lastFrustumHeight - frustumHeight;
            lastFrustumHeight = frustumHeight;

            targetPosition = new Vector3(targetPosition.x + offset.x * frustumDelta, targetPosition.y,
               targetPosition.z + offset.y * frustumDelta);

            transform.MoveLocalPosition(targetPosition, 0.2f);
        }

        void CalculateFrustomHeight(float cameraHeight)
        {
            frustumHeight = 2.0f * cameraHeight * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }
    }
}