using UnityEngine;
using System.Collections;

public class Get
{
    public static Vector2 ScreenCenter { get { return new Vector2(Screen.width / 2, Screen.height / 2); } }
    public static Vector2 ScreenSize { get { return new Vector2(Screen.width, Screen.height); } }
    public static Vector2 NormalizedMousePosition { get { return new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height); } }
}
