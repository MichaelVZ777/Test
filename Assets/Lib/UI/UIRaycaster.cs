using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UnityLib.UI
{
    public class UIRaycaster
    {
        public static List<RaycastResult> RaycastScreenCenter()
        {
            var cursor = new PointerEventData(EventSystem.current);
            cursor.position = new Vector2(Screen.width / 2, Screen.height / 2);
            var objectsHit = new List<RaycastResult>();
            EventSystem.current.RaycastAll(cursor, objectsHit);

            return objectsHit;
        }

        public static List<RaycastResult> RaycastMousePosition()
        {
            var cursor = new PointerEventData(EventSystem.current);
            cursor.position = Input.mousePosition;
            var objectsHit = new List<RaycastResult>();
            EventSystem.current.RaycastAll(cursor, objectsHit);

            return objectsHit;
        }
    }
}