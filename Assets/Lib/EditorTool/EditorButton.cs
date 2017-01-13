using System;

namespace UnityLib.EditorTool
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EditorButton : Attribute
    {
        public string name = null;

        public EditorButton()
        {
        }

        public EditorButton(string name)
        {
            this.name = name;
        }
    }
}