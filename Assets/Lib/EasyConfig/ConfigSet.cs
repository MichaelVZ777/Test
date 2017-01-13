using System.ComponentModel;

namespace UnityLib.EasyConfig
{
    public class ConfigSet
    {
        public string name;
        public INotifyPropertyChanged o;
        public ConfigProperty[] properties;
    }
}