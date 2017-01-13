using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;
using System.Reflection;

namespace UnityLib.DataBinding
{
    public class SliderPropertyBinding : MonoBehaviour
    {
        Slider slider;
        INotifyPropertyChanged o;
        PropertyInfo property;

        public void Bind(INotifyPropertyChanged o, PropertyInfo property, Slider slider)
        {
            this.o = o;
            this.property = property;
            this.slider = slider;

            if (property.PropertyType == typeof(int))
            {
                slider.wholeNumbers = true;
                slider.value = (int)property.GetValue(o, null);
            }
            else
                slider.value = (float)property.GetValue(o, null);            

            slider.onValueChanged.AddListener(SetProperty);
            o.PropertyChanged += OnPropertyChanged;
        }

        void OnEnable()
        {
            if (slider != null)
                slider.onValueChanged.AddListener(SetProperty);
        }

        void OnDisable()
        {
            if (slider != null)
                slider.onValueChanged.RemoveListener(SetProperty);
        }

        public void SetProperty(float v)
        {
            if (property.PropertyType == typeof(int))
                property.SetValue(o, (int)v, null);
            else
                property.SetValue(o, v, null);
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == property.Name)
            {
                slider.onValueChanged.RemoveListener(SetProperty);
                slider.value = (float)property.GetValue(o, null);
                slider.onValueChanged.AddListener(SetProperty);
            }
        }
    }
}