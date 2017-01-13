using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class EasyConfigTest : MonoBehaviour
{
    void Start()
    {
        GetComponent<TestConfig>().PropertyChanged += Changed;
    }

    void Changed(object sender, PropertyChangedEventArgs e)
    {
        Debug.Log(e.PropertyName);
    }
}
