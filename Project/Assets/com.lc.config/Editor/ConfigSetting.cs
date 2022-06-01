#if UNITY_EDITOR
using LCToolkit;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using LCJson;

namespace LCConfig
{
    public class ConfigSetting : ScriptableObject
    {
        //配置设置文件目录
        public const string SetingPath = "Assets/Editor/Config/Setting";

        #region Static

        private static ConfigSetting setting;
        public static ConfigSetting Setting
        {
            get
            {
                if (setting == null)
                {
                    string[] tileAssetPath = new string[] { SetingPath };
                    string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", tileAssetPath);
                    foreach (var guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        ConfigSetting asset = AssetDatabase.LoadAssetAtPath<ConfigSetting>(path);
                        if (asset != null)
                        {
                            setting = asset;
                            break;
                        }
                    }
                }
                return setting;
            }
        }

        [MenuItem("配置/设置", true)]
        public static bool CheckHasSetting()
        {
            return Setting == null;
        }

        [MenuItem("配置/设置")]
        public static void CreateSetting()
        {
            if (!Directory.Exists(SetingPath))
            {
                Directory.CreateDirectory(SetingPath);
            }
            ConfigSetting setting = CreateInstance<ConfigSetting>();
            setting.name = "配置设置";
            AssetDatabase.CreateAsset(setting, SetingPath + "/配置设置.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = setting;
        }

        [MenuItem("配置/创建", true)]
        public static bool CheckCreate()
        {
            return Setting != null;
        }
        [MenuItem("配置/创建")]
        public static void Create()
        {
            MiscHelper.Input("输入配置名", (string x) =>
            {
                ConfigAssetGroup config = CreateInstance<ConfigAssetGroup>();
                config.name = x;
                AssetDatabase.CreateAsset(config, ConfigSetting.Setting.ConfigSearchPath + x + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Selection.activeObject = config;
            });
        }

        [MenuItem("配置/导出", true)]
        public static bool CheckExport()
        {
            return Setting != null;
        }
        [MenuItem("配置/导出")]
        public static void Export()
        {
            string[] tileAssetPath = new string[] { Setting.ConfigSearchPath };
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", tileAssetPath);

            Dictionary<string, List<IConfig>> configDict = new Dictionary<string, List<IConfig>>();

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ConfigAssetGroup config = AssetDatabase.LoadAssetAtPath<ConfigAssetGroup>(path);
                if (string.IsNullOrEmpty(config.configTypeName))
                {
                    Debug.LogError($"p配置导出失败，没有对应配置类》》》{config.name}");
                    return;
                }
                List<IConfig> configs = new List<IConfig>();
                if (config != null)
                {
                    foreach (var item in config.GetAllAsset())
                    {
                        List<IConfig> datas = item.Load();
                        if (datas!=null)
                        {
                            configs.AddRange(datas);
                        }
                    }
                }
                if (configDict.ContainsKey(config.configTypeName))
                    configDict[config.configTypeName].AddRange(configs);
                else
                    configDict.Add(config.configTypeName, configs);
            }

            foreach (var item in configDict)
            {
                string filePath = Setting.ConfigExportPath + ConfigDef.GetCnfName(item.Key);
                IOHelper.WriteText(JsonMapper.ToJson(item.Value), filePath);
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        [MenuItem("配置/生成配置代码", true)]
        public static bool CheckGenCode()
        {
            return Setting != null;
        }
        [MenuItem("配置/生成配置代码")]
        public static void GenCode()
        {
            foreach (var item in ReflectionHelper.GetChildTypes<IConfig>())
            {
                GenTBConfigCode.GenCode(item);
            }
            GenConfigMappingCode.GenCode();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 获得配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetConfigAssets<T>() where T : IConfig
        {
            List<T> assets = new List<T>();
            string[] tileAssetPath = new string[] { Setting.ConfigSearchPath };
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", tileAssetPath);
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ConfigAssetGroup config = AssetDatabase.LoadAssetAtPath<ConfigAssetGroup>(path);
                if (string.IsNullOrEmpty(config.configTypeName))
                {
                    Debug.LogError($"p配置导出失败，没有对应配置类》》》{config.name}");
                    continue;
                }
                if (config.configTypeFullName == typeof(T).FullName)
                {
                    foreach (var item in config.GetAllAsset())
                    {
                        List<IConfig> datas = item.Load();
                        if (datas != null)
                        {
                            for (int i = 0; i < datas.Count; i++)
                            {
                                assets.Add((T)datas[i]);
                            }
                        }
                    }
                }
            }
            return assets;
        }

        #endregion

        [Header("配置文件搜索路径")]
        [SerializeField]
        public string ConfigSearchPath = "Assets/Editor/Config/";

        [Header("配置文件导出路径")]
        [SerializeField]
        public string ConfigExportPath = "Assets/Demo/Asset/Config/";
    }
}

#endif