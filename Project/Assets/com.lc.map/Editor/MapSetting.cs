﻿using LCConfig;
using System.Collections.Generic;
using System.IO;
using LCConfig.Excel;
using UnityEditor;
using UnityEngine;

namespace LCMap
{
    public class MapSetting : ScriptableObject
    {
        //地图设置文件目录
        public const string MapSetingPath = "Assets/Editor/Map/Setting";

        [Header("地图配置搜索路径")]
        [SerializeField]
        public string MapSearchPath = "Assets/Editor/Map/Maps";

        [Header("地图导出保存路径")]
        [SerializeField]
        public string MapExportSavePath = "Assets/Resources/Config/Map/";
        
        [Header("演员模板")]
        [SerializeField]
        public GameObject ActorTemplate = null;

        #region Static

        private static MapSetting setting;
        public static MapSetting Setting
        {
            get
            {
                if (setting == null)
                {
                    string[] tileAssetPath = new string[] { MapSetingPath };
                    string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", tileAssetPath);
                    foreach (var guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        MapSetting asset = AssetDatabase.LoadAssetAtPath<MapSetting>(path);
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

        [MenuItem("地图/设置", true)]
        public static bool CheckHasSetting()
        {
            return Setting==null;
        }

        [MenuItem("地图/设置")]
        public static void CreateSetting()
        {
            if (!Directory.Exists(MapEditorDef.MapSetingPath))
            {
                Directory.CreateDirectory(MapEditorDef.MapSetingPath);
            }
            MapSetting setting = CreateInstance<MapSetting>();
            setting.name = "地图设置";
            AssetDatabase.CreateAsset(setting, MapEditorDef.MapSetingPath + "/地图设置.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = setting;
        }

        public static List<ActorCnf> GetActorCnfs()
        {
            return ExcelReadCtrl.GetConfig<ActorCnf>();
        }

        public static Dictionary<string, List<ActorCnf>> GetActorGroups()
        {
            Dictionary<string, List<ActorCnf>> actorMap = new Dictionary<string, List<ActorCnf>>();
            actorMap.Add("All",ExcelReadCtrl.GetConfig<ActorCnf>());
            return actorMap;
        }

        public static string GetMapModelSavePath(string mapId)
        {
            return setting.MapExportSavePath + ConfigDef.GetCnfName("Map_" + mapId);
        }

        public static List<ED_MapCom> GetAllMaps()
        {
            List<ED_MapCom> maps = new List<ED_MapCom>();
            string[] tileAssetPath = new string[] { Setting.MapSearchPath };
            string[] guids = AssetDatabase.FindAssets("t:Prefab", tileAssetPath);
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject mapGo = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (mapGo.GetComponent<ED_MapCom>() != null)
                {
                    maps.Add(mapGo.GetComponent<ED_MapCom>());
                }
            }
            return maps;
        }

        #endregion
    }
} 