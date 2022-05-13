using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LCTimeline
{
    [Serializable]
    public class TimelineGroupPath
    {
        public string typeName = "";
        public string typeFullName = "";

        public string searchPath = "";

        public TimelineGroupPath(Type type)
        {
            this.typeName = type.Name;
            this.typeFullName = type.FullName;
        }
    }

    /// <summary>
    /// 视图设置
    /// </summary>
    public class TimelineSetting : ScriptableObject
    {
        //地图设置文件目录
        public const string SetingPath = "Assets/Editor/Timeline/Setting";

        public List<TimelineGroupPath> groupPaths = new List<TimelineGroupPath>();

        public TimelineGroupPath GetSearchPath(string typeFullName)
        {
            foreach (var item in groupPaths)
            {
                if (item.typeFullName == typeFullName)
                {
                    return item;
                }
            }
            return null;
        }

        public TimelineGroupPath GetSearchPath<T>() where T : InternalTimelineGraphGroupAsset
        {
            foreach (var item in groupPaths)
            {
                if (item.typeFullName == typeof(T).FullName)
                {
                    return item;
                }
            }
            return null;
        }

        public List<InternalTimelineGraphGroupAsset> GetGroups(string groupPath)
        {
            List<InternalTimelineGraphGroupAsset> groups = new List<InternalTimelineGraphGroupAsset>();
            string[] tileAssetPath = new string[] { groupPath };
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", tileAssetPath);
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                InternalTimelineGraphGroupAsset assetGroup = AssetDatabase.LoadAssetAtPath<InternalTimelineGraphGroupAsset>(path);
                if (assetGroup != null)
                {
                    groups.Add(assetGroup);
                }
            }
            return groups;
        }

        public List<T> GetGroupAssets<T>(string groupPath) where T : InternalTimelineGraphAsset
        {
            List<T> assets = new List<T>();
            string[] tileAssetPath = new string[] { groupPath };
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", tileAssetPath);
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BaseTimelineGraphGroupAsset<T> assetGroup = AssetDatabase.LoadAssetAtPath<BaseTimelineGraphGroupAsset<T>>(path);
                if (assetGroup != null)
                {
                    foreach (var item in assetGroup.GetAllGraph())
                    {
                        assets.Add(item as T);
                    }
                }
            }
            return assets;
        }

        public T GetAsset<T>(string groupPath, string name) where T : InternalTimelineGraphAsset
        {
            List<T> assets = GetGroupAssets<T>(groupPath);
            foreach (var item in assets)
            {
                if (item.name == name)
                {
                    return item;
                }
            }
            return null;
        }

        #region Static

        private static TimelineSetting setting;
        public static TimelineSetting Setting
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
                        TimelineSetting asset = AssetDatabase.LoadAssetAtPath<TimelineSetting>(path);
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

        [MenuItem("Timeline/设置", true)]
        public static bool CheckHasSetting()
        {
            return Setting == null;
        }

        [MenuItem("Timeline/设置")]
        public static void CreateSetting()
        {
            if (!Directory.Exists(SetingPath))
            {
                Directory.CreateDirectory(SetingPath);
            }
            TimelineSetting setting = CreateInstance<TimelineSetting>();
            setting.name = "Timeline设置";
            AssetDatabase.CreateAsset(setting, SetingPath + "/Timeline设置.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = setting;
        }

        [MenuItem("Timeline/导出所有配置", true)]
        public static bool ExportAllCheck()
        {
            return Setting != null;
        }

        [MenuItem("Timeline/导出所有配置")]
        public static void ExportAll()
        {
            Dictionary<Type, InternalTimelineGraphGroupAsset> groupDict = new Dictionary<Type, InternalTimelineGraphGroupAsset>();
            foreach (var item in Setting.groupPaths)
            {
                var list = Setting.GetGroups(item.searchPath);
                for (int i = 0; i < list.Count; i++)
                {
                    if (!groupDict.ContainsKey(list[i].GetType()))
                    {
                        groupDict.Add(list[i].GetType(), list[i]);
                    }
                }
            }
            foreach (var item in groupDict)
            {
                item.Value.OnClickExport();
            }
        }

        #endregion
    }
}
