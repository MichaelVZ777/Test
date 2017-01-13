using UnityEngine;
using System;

namespace UnityLib.UI
{
    public class RectManipulator : MonoBehaviour
    {
        public event Action<Vector2> OnMove;
        public event Action<float> OnScale;
        public event Action OnRotate;

        public UIHandle moveHandle;
        public UIHandle rotateHandle;
        public UIHandle scaleHandle;

        bool isDirty;
        public bool isStickingToMouse;

        Vector2 lastPosition;

        void Start()
        {
            //LoadTransform();
            lastPosition = transform.position;
            //set anchors to buttom left
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
        }

        void OnEnable()
        {
            if (moveHandle != null)
                moveHandle.gameObject.SetActive(true);
            if (rotateHandle != null)
                rotateHandle.gameObject.SetActive(true);
            if (scaleHandle != null)
                scaleHandle.gameObject.SetActive(true);
        }

        void OnDisable()
        {
            if (moveHandle != null)
                moveHandle.gameObject.SetActive(false);
            if (rotateHandle != null)
                rotateHandle.gameObject.SetActive(false);
            if (scaleHandle != null)
                scaleHandle.gameObject.SetActive(false);
        }

        void Update()
        {
            var canvas = GetComponentInParent<Canvas>();
            var renderMode = canvas.renderMode;

            if (moveHandle != null && moveHandle.isMouseDown || isStickingToMouse)
                HandleMove(renderMode, canvas);
            if (rotateHandle != null && rotateHandle.isMouseDown)
                HandleRotate(renderMode, canvas);
            if (scaleHandle != null && scaleHandle.isMouseDown)
                HandleScale(renderMode, canvas);

            if (isDirty)
                SaveTransform();
            isDirty = false;
        }

        Vector2 localPoint;

        void HandleMove(RenderMode renderMode, Canvas canvas)
        {
            isDirty = true;

            switch (renderMode)
            {
                case RenderMode.ScreenSpaceOverlay:
                    var offset = moveHandle.transform.position - transform.position;
                    transform.position = Input.mousePosition - offset - (Vector3)moveHandle.mouseDownOffset;
                    break;
                case RenderMode.ScreenSpaceCamera:
                    var handleOffset = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, moveHandle.transform.position)
                                       - RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, transform.position);

                    ((RectTransform)transform).anchoredPosition = ((Vector2)Input.mousePosition - handleOffset) / canvas.scaleFactor;
                    break;
            }

            var delta = (Vector2)transform.position - lastPosition;
            if (delta != Vector2.zero && OnMove != null)
                OnMove(delta);
            lastPosition = transform.position;
        }

        void HandleRotate(RenderMode renderMode, Canvas canvas)
        {
            isDirty = true;
            Vector2 direction = Input.mousePosition - transform.position;

            if (renderMode == RenderMode.ScreenSpaceCamera)
                direction = (Vector2)Input.mousePosition - RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, transform.position);

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            var handleDir = rotateHandle.transform.localPosition;
            var handleAngle = Mathf.Atan2(handleDir.y, handleDir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle - handleAngle, Vector3.forward);

            if (OnRotate != null)
                OnRotate();
        }

        void HandleScale(RenderMode renderMode, Canvas canvas)
        {
            isDirty = true;
            var handleDistance = rotateHandle.transform.localPosition.magnitude * canvas.scaleFactor;
            var inputOffset = (Input.mousePosition - transform.position).magnitude;

            if (renderMode == RenderMode.ScreenSpaceCamera)
                inputOffset = ((Vector2)Input.mousePosition - RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, transform.position)).magnitude;

            var scale = inputOffset / handleDistance;
            var lastScale = transform.localScale.x;
            transform.localScale = new Vector3(scale, scale, scale);

            if (OnScale != null)
                OnScale(scale - lastScale);
        }

        Vector2 ScreenToLocal(Vector2 screenPoint)
        {
            return new Vector2(screenPoint.x * 2 - Screen.width,
                screenPoint.y * 2 - Screen.height);
        }

        Vector2 LocalToScreen(Vector2 localPosition)
        {
            return new Vector2((localPosition.x + Screen.width) / 2,
                (localPosition.y + Screen.height) / 2);
        }

        Vector2 WorldToScreen(Vector2 position, Canvas canvas)
        {
            return RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, new Vector3(position.x, position.y, canvas.planeDistance));
        }

        public void SaveTransform()
        {
            //var serializer = SerializationContext.Default.GetSerializer<RectTransformData>();
            //var data = new RectTransformData();
            //data.position = transform.position;
            //data.rotation = transform.rotation;
            //data.scale = transform.localScale.x;
            //var bytes = serializer.PackSingleObject(data);
            //var s = Convert.ToBase64String(bytes);
            //PlayerPrefs.SetString(GetInstanceID().ToString(), s);
        }

        public void LoadTransform()
        {
            //var serializer = SerializationContext.Default.GetSerializer<RectTransformData>();
            //var savedS = PlayerPrefs.GetString(GetInstanceID().ToString());
            //if (savedS == "")
            //    return;

            //var savedBytes = Convert.FromBase64String(savedS);
            //var data = serializer.UnpackSingleObject(savedBytes);
            //transform.position = data.position;
            //transform.rotation = data.rotation;
            //transform.localScale = new Vector3(data.scale, data.scale, data.scale);
        }
    }

    public class RectTransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public float scale;
    }
}