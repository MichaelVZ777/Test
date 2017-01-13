using UnityEngine;
using System.Collections;
using System;

namespace UnityLib.EasyConfig
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Config : Attribute
    {
       public string customName;

        public Config()
        {
        }

        public Config(string customName)
        {
            this.customName = customName;
        }
    }

    public class ConfigSlider : Config
    {
        public float min, max;

        public ConfigSlider(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}