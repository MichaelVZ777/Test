﻿using UnityEngine;

namespace UnityLib.CameraTool
{
    public class VRMouseLook : SingletonMono<VRMouseLook>
    {
#if UNITY_EDITOR
        public bool autoRecenterPitch = true;
        public bool autoRecenterRoll = true;
        public KeyCode RollKey = KeyCode.LeftControl;

        Transform vrCameraTransform;
        Transform rotationTransform;
        Transform forwardTransform;

        private float mouseX = 0;
        private float mouseY = 0;
        private float mouseZ = 0;

        void Awake()
        {
            transform.localRotation = Quaternion.identity;

            // get the vr camera so we can align our forward with it
            Camera vrCamera = gameObject.GetComponentInChildren<Camera>();
            vrCameraTransform = vrCamera.transform;

            // create a hierarchy to enable us to additionally rotate the vr camera
            rotationTransform = new GameObject("VR Mouse Look (Rotation)").GetComponent<Transform>();
            forwardTransform = new GameObject("VR Mouse Look (Forward)").GetComponent<Transform>();
            rotationTransform.SetParent(transform.parent, false);
            forwardTransform.SetParent(rotationTransform, false);
            transform.SetParent(forwardTransform, false);
        }

        void Update()
        {
            bool rolled = false;
            bool pitched = false;
            if (Input.GetMouseButton(1))
            {
                pitched = true;
                mouseX += Input.GetAxis("Mouse X") * 5;
                if (mouseX <= -180)
                {
                    mouseX += 360;
                }
                else if (mouseX > 180)
                {
                    mouseX -= 360;
                }
                mouseY -= Input.GetAxis("Mouse Y") * 2.4f;
                mouseY = Mathf.Clamp(mouseY, -85, 85);
            }
            else if (Input.GetKey(RollKey))
            {
                rolled = true;
                mouseZ += Input.GetAxis("Mouse X") * 5;
                mouseZ = Mathf.Clamp(mouseZ, -85, 85);
            }
            if (!rolled && autoRecenterRoll)
                mouseZ = Mathf.Lerp(mouseZ, 0, Time.deltaTime / (Time.deltaTime + 0.1f));

            if (!pitched && autoRecenterPitch)
                mouseY = Mathf.Lerp(mouseY, 0, Time.deltaTime / (Time.deltaTime + 0.1f));

            forwardTransform.localRotation = Quaternion.Inverse(Quaternion.Euler(0.0f, vrCameraTransform.localRotation.eulerAngles.y, 0.0f));
            rotationTransform.localRotation = Quaternion.Euler(0, vrCameraTransform.localRotation.eulerAngles.y, 0.0f) * Quaternion.Euler(mouseY, mouseX, mouseZ);
        }

        public void LookAtTarget(Transform target)
        {
            if (!Application.isPlaying)
                transform.LookAt(target.position);
        }
#endif
    }
}