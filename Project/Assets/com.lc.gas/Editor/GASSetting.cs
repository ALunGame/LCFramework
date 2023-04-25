using System.IO;
using LCJson;
using LCToolkit;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    public class GASSetting : ScriptableObject
    {
        public const string SetingPath = "Assets/Editor/GAS/TagsSetting";
        public const string FileName = "GASSetting.json";

        public bool OpenTag = false;
        public bool OpenRate = false;
        public bool OpenPeriod = false;
        public bool OpenDuration = false;
        public bool OpenModifiers = false;

        public Color testColor = Color.black;
        
        #region Static

        private static GASSetting setting;
        public static GASSetting Setting
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
                        GASSetting asset = AssetDatabase.LoadAssetAtPath<GASSetting>(path);
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
        
        [MenuItem("Gameplay/GAS/Setting")]
        public static void CreateSetting()
        {
            GASSetting setting = Setting;
            if (setting == null)
            {
                if (!Directory.Exists(SetingPath))
                {
                    Directory.CreateDirectory(SetingPath);
                }
                setting = CreateInstance<GASSetting>();
                setting.name = "GASSetting";
                AssetDatabase.CreateAsset(setting, SetingPath + "/GASSetting.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Selection.activeObject = setting;
            }
            else
            {
                Selection.activeObject = setting;
            }
        }
        
        [MenuItem("Gameplay/GAS/Test")]
        public static void OpenTast()
        {
            GameplayEffect effect = new GameplayEffect();
            InspectorExtension.DrawObjectInInspector(effect);
        }

        #endregion
    }
}