using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using UnityEngine.UI;

namespace UnityLib.DataBinding
{
    public class TextBinding : MonoBehaviour
    {
        //INotifyPropertyChanged o;
        //PropertyInfo property;
        public Text label;

        public void Bind(INotifyPropertyChanged o, PropertyInfo property)
        {
            print("binding");
            //this.o = o;
            //this.property = property;

            o.PropertyChanged += OnPropertyChanged;
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //print(((TestClass)sender).TestProperty.ToString());
            //print(e.PropertyName);
        }
    }
}