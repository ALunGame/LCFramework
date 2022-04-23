using System;
using System.Collections.Generic;
using UnityEngine;
using LCToolkit;

namespace LCConfig
{
    public class ConfigAsset : ScriptableObject
    {
        [EDReadOnly]
        [SerializeField]
        [Header("配置类")]
        public string cnfTypeFullName;

        [HideInInspector]
        [SerializeField]
        [TextArea(20, 20)]
        string serializedText = String.Empty;

        public Type GetCnfType()
        {
            return ReflectionHelper.GetType(cnfTypeFullName);
        }

        public IConfig CreateCnfItem()
        {
            Type type = GetCnfType();
            return (IConfig)ReflectionHelper.CreateInstance(type);
        }

        public void Save(List<IConfig> configs)
        {
            serializedText = LCJson.JsonMapper.ToJson(configs);
        }

        public List<T> Load<T>() where T : IConfig
        {
            var configs = LCJson.JsonMapper.ToObject<List<T>>(serializedText);
            if (configs == null)
            {
                configs = new List<T>();
            }
            return configs;
        }

        public List<IConfig> Load()
        {
            var configs = LCJson.JsonMapper.ToObject<List<IConfig>>(serializedText);
            return configs;
        }
    }
}
