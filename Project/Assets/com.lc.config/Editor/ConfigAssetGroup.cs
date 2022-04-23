using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

namespace LCConfig
{
    [CreateAssetMenu(fileName = "配置表", menuName = "配置组/配置表", order = 4)]
    public class ConfigAssetGroup : ScriptableObject
    {
        //配置类型
        [SerializeField]
        [HideInInspector]
        public string configTypeFullName;

        public List<ConfigAsset> GetAllAsset()
        {
            List<ConfigAsset> assets = new List<ConfigAsset>();
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(this));
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] is ConfigAsset)
                {
                    ConfigAsset asset = (ConfigAsset)objs[i];
                    asset.cnfTypeFullName = configTypeFullName;
                    assets.Add(asset);
                }
            }
            return assets;
        }

        public bool CheckHasAsset(string name)
        {
            List<ConfigAsset> assets = GetAllAsset();
            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i].name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CreateAsset(string name)
        {
            if (CheckHasAsset(name))
            {
                Debug.LogError($"创建资源失败，重复资源>>{name}");
                return false;
            }
            ConfigAsset asset = CreateInstance<ConfigAsset>();
            asset.name = name;
            asset.cnfTypeFullName = configTypeFullName;
            AssetDatabase.AddObjectToAsset(asset, this);
            EditorUtility.SetDirty(asset);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return true;
        }

        public void RemoveAsset(ConfigAsset asset)
        {
            DestroyImmediate(asset, true);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
