using UnityEngine;
using System.Collections.Generic;
using UnityLib.UI;
using System.ComponentModel;
using UnityLib.EditorTool;

namespace UnityLib.EasyConfig
{
    public class EasyConfigManager : SingletonMonoManager<EasyConfigManager>
    {
        public Panel panel;
        public AutoCanvas autoCanvas;
        public List<ConfigSet> configs;
        bool initialized;

        void OnEnable()
        {
            CheckForClassChange();
        }

        void Init()
        {
            configs = new List<ConfigSet>();
            initialized = true;
        }

        public void CheckForClassChange()
        {
            if (!initialized)
                Init();

            configs = new List<ConfigSet>();

            var parsedObjects = FindObjectsOfType<MonoBehaviour>();
            var parsedINPC = new List<INotifyPropertyChanged>();

            foreach (var unityObject in parsedObjects)
            {
                var inpc = unityObject as INotifyPropertyChanged;
                if (inpc != null)
                    parsedINPC.Add(inpc);
            }

            foreach (var o in parsedINPC)
            {
                var configSet = GetConfigSet(o);
                if (configSet != null)
                    configs.Add(configSet);
            }
            BuildUI();
        }

        public ConfigSet GetConfigSet(INotifyPropertyChanged o)
        {
            var properties = new List<ConfigProperty>();

            //look for attribute
            foreach (var property in o.GetType().GetProperties())
                foreach (var attribute in property.GetCustomAttributes(true))
                {
                    var configSlider = attribute as ConfigSlider;
                    if (configSlider != null)
                    {
                        var sliderConfigProperty = new SliderConfigProperty();
                        sliderConfigProperty.customName = configSlider.customName;
                        sliderConfigProperty.min = configSlider.min;
                        sliderConfigProperty.max = configSlider.max;
                        sliderConfigProperty.o = o;
                        sliderConfigProperty.property = property;
                        properties.Add(sliderConfigProperty);
                        LoadPlayerPref(o, property.Name);
                        continue;
                    }

                    var config = attribute as Config;
                    if (config != null)
                    {
                        if (property.PropertyType == typeof(bool))
                        {
                            var toggleConfigProperty = new ToggleConfigProperty();
                            toggleConfigProperty.customName = config.customName;
                            toggleConfigProperty.o = o;
                            toggleConfigProperty.property = property;
                            properties.Add(toggleConfigProperty);
                            LoadPlayerPref(o, property.Name);
                            continue;
                        }

                        var configProperty = new ConfigProperty();
                        configProperty.customName = config.customName;
                        configProperty.o = o;
                        configProperty.property = property;
                        properties.Add(configProperty);
                        LoadPlayerPref(o, property.Name);
                    }
                }

            //store them with their object
            if (properties.Count == 0)
                return null;

            o.PropertyChanged += OnPropertyChanged;
            var configSet = new ConfigSet();
            configSet.o = o;
            configSet.properties = properties.ToArray();

            return configSet;
        }

        [EditorButton]
        public void BuildUI()
        {
            if (panel == null)
            {
                if (autoCanvas != null)
                    autoCanvas.Destroy();

                autoCanvas = new AutoCanvas();
                autoCanvas.Init();

                panel = autoCanvas.AddPanel();
            }
            panel.SetLayout(LayoutType.Vertical);
            foreach (var configSet in configs)
                foreach (var configProperty in configSet.properties)
                    AddItem(panel, configProperty);
        }

        void AddItem(Panel panel, ConfigProperty configProperty)
        {
            var sliderProperty = configProperty as SliderConfigProperty;
            if (sliderProperty != null)
            {
                AddSliderItem(panel, sliderProperty);
                return;
            }

            var toggleProperty = configProperty as ToggleConfigProperty;
            if (toggleProperty != null)
            {
                AddToggleItem(panel, toggleProperty);
                return;
            }

            AddFieldItem(panel, configProperty);
        }

        void AddFieldItem(Panel panel, ConfigProperty configProperty)
        {
            var fieldItem = panel.AddField(GetConfigPropertyName(configProperty));
            fieldItem.Bind(configProperty.o, configProperty.property);
        }

        void AddSliderItem(Panel panel, SliderConfigProperty sliderProperty)
        {
            var sliderItem = panel.AddSlider(GetConfigPropertyName(sliderProperty));
            sliderItem.Bind(sliderProperty.o, sliderProperty.property);
        }

        void AddToggleItem(Panel panel, ToggleConfigProperty toggleConfigProperty)
        {
            var toggleItem = panel.AddToggle(GetConfigPropertyName(toggleConfigProperty));
            toggleItem.Bind(toggleConfigProperty.o, toggleConfigProperty.property);
        }

        string GetConfigPropertyName(ConfigProperty configProperty)
        {
            if (string.IsNullOrEmpty(configProperty.customName))
                return configProperty.property.Name;
            else
                return configProperty.customName;            
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SavePlayerPref(sender, e.PropertyName);
        }

        void SavePlayerPref(object o, string propertyName)
        {
            var type = o.GetType();
            var property = type.GetProperty(propertyName);
            var keyName = type.Name + property.Name;

            if (property.PropertyType == typeof(bool))
                PlayerPrefs.SetInt(keyName, (bool)property.GetValue(o, null) ? 1 : 0);
            else if (property.PropertyType == typeof(int))
                PlayerPrefs.SetInt(keyName, (int)property.GetValue(o, null));
            else if (property.PropertyType == typeof(float))
                PlayerPrefs.SetFloat(keyName, (float)property.GetValue(o, null));
            else if (property.PropertyType == typeof(string))
                PlayerPrefs.SetString(keyName, (string)property.GetValue(o, null));
        }

        bool LoadPlayerPref(object o, string propertyName)
        {
            var type = o.GetType();
            var property = type.GetProperty(propertyName);
            var keyName = type.Name + property.Name;

            if (!PlayerPrefs.HasKey(keyName))
                return false;

            if (property.PropertyType == typeof(bool))
                property.SetValue(o, PlayerPrefs.GetInt(keyName) == 1 ? true : false, null);
            else if (property.PropertyType == typeof(int))
                property.SetValue(o, PlayerPrefs.GetInt(keyName), null);
            else if (property.PropertyType == typeof(float))
                property.SetValue(o, PlayerPrefs.GetFloat(keyName), null);
            else if (property.PropertyType == typeof(string))
                property.SetValue(o, PlayerPrefs.GetString(keyName), null);

            return true;
        }

        public void DeletePlayerPref(object o)
        {
            var type = o.GetType();
            foreach (var property in type.GetProperties())
                foreach (var attribute in property.GetCustomAttributes(true))
                    if (attribute is Config)
                        PlayerPrefs.DeleteKey(type.Name + property.Name);
        }
    }
}