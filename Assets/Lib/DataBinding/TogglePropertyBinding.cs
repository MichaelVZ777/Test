using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using UnityEngine.UI;

namespace UnityLib.DataBinding
{
    public class TogglePropertyBinding : MonoBehaviour
    {
        Toggle toggle;
        INotifyPropertyChanged o;
        PropertyInfo property;

        public void Bind(INotifyPropertyChanged o, PropertyInfo property, Toggle toggle)
        {
            this.o = o;
            this.property = property;
            this.toggle = toggle;
            toggle.onValueChanged.AddListener(SetProperty);
            toggle.isOn = (bool)property.GetValue(o, null);

            o.PropertyChanged += OnPropertyChanged;
        }

        void OnEnable()
        {
            if (toggle != null)
                toggle.onValueChanged.AddListener(SetProperty);
        }

        void OnDisable()
        {
            if (toggle != null)
                toggle.onValueChanged.RemoveListener(SetProperty);
        }

        public void SetProperty(bool v)
        {
            property.SetValue(o, v, null);
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == property.Name)
            {
                toggle.onValueChanged.RemoveListener(SetProperty);
                toggle.isOn = (bool)property.GetValue(o, null);
                toggle.onValueChanged.AddListener(SetProperty);
            }
        }
    }
}