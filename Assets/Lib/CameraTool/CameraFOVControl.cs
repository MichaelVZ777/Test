using UnityEngine;
using SafeTween;
using UnityLib.UI;

namespace UnityLib.CameraTool
{
    public class CameraFOVControl : MonoBehaviour
    {
        public MouseController controller;
        public Camera cam;
        public float scrollSensitivity;
        public float maxFOV = 165;

        float targetFOV = 50;
        float frustumHeight;
        float lastFrustumHeight;
        Vector3 targetPosition;

        void Awake()
        {
            cam = GetComponent<Camera>();
            targetPosition = transform.position;
            CalculateFrustomHeight();
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
            targetFOV -= scroll * scrollSensitivity;
            targetFOV = Mathf.Clamp(targetFOV, 1, maxFOV);

            cam.TweenFOV(targetFOV, 0.2f);
            var offset = (Vector2)Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2);
            offset /= Screen.height;

            frustumHeight = 2.0f * transform.position.y * Mathf.Tan(targetFOV * 0.5f * Mathf.Deg2Rad);
            var frustumDelta = lastFrustumHeight - frustumHeight;
            lastFrustumHeight = frustumHeight;

            targetPosition = new Vector3(targetPosition.x + offset.x * frustumDelta, targetPosition.y,
               targetPosition.z + offset.y * frustumDelta);

            transform.MoveLocalPosition(targetPosition, 0.2f);
        }

        void CalculateFrustomHeight()
        {
            frustumHeight = 2.0f * transform.position.y * Mathf.Tan(targetFOV * 0.5f * Mathf.Deg2Rad);
        }
    }
}