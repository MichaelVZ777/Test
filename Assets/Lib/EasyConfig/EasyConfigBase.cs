using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityLib.EditorTool;

namespace UnityLib.EasyConfig
{
    public class EasyConfigBase<T> : SingletonMono<T>, INotifyPropertyChanged, ISerializationCallbackReceiver where T : MonoBehaviour
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [HideInInspector]
        [SerializeField]
        byte[] dataBytes;

        void Awake()
        {
            var manager = EasyConfigManager.Instance;
        }

        [EditorButton]
        public void DeletePlayerPref()
        {
            EasyConfigManager.Instance.DeletePlayerPref(this);
        }

        public void OnAfterDeserialize()
        {
            var container = ProtoBufSerializer.DeserializeFromBytes<PropertyDataContainer>(dataBytes);

            foreach (var property in GetType().GetProperties())
                foreach (var attribute in property.GetCustomAttributes(true))
                    if (attribute is Config)
                        switch (property.PropertyType.Name)
                        {
                            case "Int32":
                                if (container.intDict.ContainsKey(property.Name))
                                    property.SetValue(this, container.intDict[property.Name], null);
                                break;
                            case "Single":
                                if (container.floatDict.ContainsKey(property.Name))
                                    property.SetValue(this, container.floatDict[property.Name], null);
                                break;
                            case "String":
                                if (container.stringDict.ContainsKey(property.Name))
                                    property.SetValue(this, container.stringDict[property.Name], null);
                                break;
                            case "Boolean":
                                if (container.boolDict.ContainsKey(property.Name))
                                    property.SetValue(this, container.boolDict[property.Name], null);
                                break;
                        }
        }

        public void OnBeforeSerialize()
        {
            var container = new PropertyDataContainer();

            foreach (var property in GetType().GetProperties())
                foreach (var attribute in property.GetCustomAttributes(true))
                    if (attribute is Config)
                        switch (property.PropertyType.Name)
                        {
                            case "Int32":
                                container.intDict.Add(property.Name, (int)property.GetValue(this, null));
                                break;
                            case "Single":
                                container.floatDict.Add(property.Name, (float)property.GetValue(this, null));
                                break;
                            case "String":
                                container.stringDict.Add(property.Name, (string)property.GetValue(this, null));
                                break;
                            case "Boolean":
                                container.boolDict.Add(property.Name, (bool)property.GetValue(this, null));
                                break;
                        }

            //save if there are chanages
            var newBytes = ProtoBufSerializer.SerializeToBytes(container);

            if (dataBytes.Length != newBytes.Length)
                SetBytes(newBytes);
            else for (int i = 0; i < dataBytes.Length; i++)
                    if (dataBytes[i] != newBytes[i])
                    {
                        SetBytes(newBytes);
                        return;
                    }
        }

        void SetBytes(byte[] newBytes)
        {
            dataBytes = newBytes;
#if UNITY_EDITOR
            if (!Application.isPlaying)
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
        }

        [ProtoContract]
        public class PropertyDataContainer
        {
            [ProtoMember(1)]
            public Dictionary<string, string> stringDict;
            [ProtoMember(2)]
            public Dictionary<string, int> intDict;
            [ProtoMember(3)]
            public Dictionary<string, float> floatDict;
            [ProtoMember(4)]
            public Dictionary<string, bool> boolDict;

            public PropertyDataContainer()
            {
                stringDict = new Dictionary<string, string>();
                intDict = new Dictionary<string, int>();
                floatDict = new Dictionary<string, float>();
                boolDict = new Dictionary<string, bool>();
            }
        }
    }
}