using System.Reflection;
using System.ComponentModel;

namespace UnityLib.EasyConfig
{
    public class ConfigProperty
    {
        public INotifyPropertyChanged o;
        public PropertyInfo property;
        public string customName;
    }

    public class SliderConfigProperty : ConfigProperty
    {
        public float min;
        public float max;
    }

    public class ToggleConfigProperty : ConfigProperty
    {
        public string toggleGroupName;
    }
}