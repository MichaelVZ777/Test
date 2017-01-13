using UnityEngine;

namespace UnityLib.Utility
{
    public class Billboard : MonoBehaviour
    {
        public Vector3 center;
        public Camera billboardCamera;
        public bool allowPitch;

        public void Update()
        {
            if (billboardCamera != null)
                transform.LookAt(transform.position + billboardCamera.transform.rotation * Vector3.back,
                    billboardCamera.transform.rotation * Vector3.up);
            else
                transform.LookAt(center, Vector3.up);

            //flip it
            transform.forward = -transform.forward;

            var rotation = transform.localRotation.eulerAngles;

            if (!allowPitch)
                transform.localRotation = Quaternion.Euler(0, rotation.y - 180, rotation.z);
        }
    }
}