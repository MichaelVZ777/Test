using UnityEngine;
using System.Collections;
using System.ComponentModel;
using UnityLib.EasyConfig;
using UnityLib.EditorTool;

public class TestConfig : EasyConfigBase<TestConfig>
{
    [Config]
    public int intProperty { get; set; }

    [Config]
    public float floatProperty { get; set; }

    [Config]
    public string stringProperty { get; set; }

    [Config]
    public bool boolProperty { get; set; }

    [ConfigSlider(0, 1)]
    public float sliderProperty { get; set; }

    [SerializeField]
    public float serializeProperty { get; set; }


    [EditorButton]
    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
    }
}
