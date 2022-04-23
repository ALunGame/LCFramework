#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace LCConfig
{
    public class ConfigSetting : ScriptableObject
    {
        //配置设置文件目录
        public const string SetingPath = "Assets/Editor/Config/Setting";

        private static ConfigSetting setting;
        public static ConfigSetting Setting 
        { 
            get {
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

        [Header("C#生成代码路径")]
        [SerializeField]
        public string CSharpGenPath = "Assets/Editor/Map/Actors";

        [Header("配置文件搜索路径")]
        [SerializeField]
        public string ConfigSearchPath = "Assets/Editor/Map/Maps";

        [Header("配置文件导出路径")]
        [SerializeField]
        public string ConfigExportPath = "Assets/Resources/Config/Map/";
    }
}

#endif