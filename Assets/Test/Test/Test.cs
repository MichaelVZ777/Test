using UnityEngine;
using UnityLib.EditorTool;

public class Test : MonoBehaviour
{
    [EditorButton]
    public void Run()
    {
        throw new System.Exception();
    }
}
