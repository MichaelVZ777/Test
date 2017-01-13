using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using UnityEngine.UI;

namespace UnityLib.DataBinding
{
    public class InputFieldPropertyBinding : MonoBehaviour
    {
        InputField inputField;
        INotifyPropertyChanged o;
        PropertyInfo property;

        public void Bind(INotifyPropertyChanged o, PropertyInfo property, InputField inputField)
        {
            this.o = o;
            this.property = property;
            this.inputField = inputField;
            if (property.PropertyType == typeof(float))
                inputField.contentType = InputField.ContentType.DecimalNumber;
            else if (property.PropertyType == typeof(int))
            {
                inputField.contentType = InputField.ContentType.IntegerNumber;
                inputField.characterLimit = 11;
            }

            inputField.onEndEdit.AddListener(SetProperty);
            inputField.text = property.GetValue(o, null).ToString();

            o.PropertyChanged += OnPropertyChanged;
        }

        void OnEnable()
        {
            if (inputField != null)
                inputField.onEndEdit.AddListener(SetProperty);
        }

        void OnDisable()
        {
            if (inputField != null)
                inputField.onEndEdit.RemoveListener(SetProperty);
        }

        public void SetProperty(string v)
        {
            switch (inputField.contentType)
            {
                case InputField.ContentType.IntegerNumber:
                    int i = 0;
                    if (int.TryParse(v, out i))
                        SetProperty(i);
                    break;
                case InputField.ContentType.DecimalNumber:
                    float f = 0;
                    if (float.TryParse(v, out f))
                        SetProperty(f);
                    break;
                default:
                    property.SetValue(o, v, null);
                    break;
            }
        }

        public void SetProperty(float v)
        {
            property.SetValue(o, v, null);
        }

        public void SetProperty(int v)
        {
            property.SetValue(o, v, null);
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == property.Name)
            {
                inputField.onEndEdit.RemoveListener(SetProperty);
                inputField.text = property.GetValue(o, null).ToString();
                inputField.onEndEdit.AddListener(SetProperty);
            }
        }
    }
}