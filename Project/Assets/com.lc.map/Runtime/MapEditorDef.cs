#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using LCToolkit;

namespace LCMap
{
    public static class MapEditorDef
    {
        public const int MapUidCnt = 10000;

        //地图设置文件目录
        public const string MapSetingPath = "Assets/Editor/Map/Setting";

        //演员预制体路径
        public const string ActorPrefabPath = "Assets/com.lc.map/Runtime/Asset/Prefab/ActorPrefab.prefab";

        //地图预制体路径
        public const string MapPrefabPath = "Assets/com.lc.map/Runtime/Asset/Prefab/MapPrefab.prefab";

        private static MapSetting setting;

        static MapEditorDef()
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

        public static bool CheckHasSetting()
        {
            return setting != null;
        }

        public static ED_MapCom CreateMapGo(string name)
        {
            GameObject mapGo = AssetDatabase.LoadAssetAtPath<GameObject>(MapPrefabPath);
            GameObject newGo = GameObject.Instantiate(mapGo);
            ED_MapCom mapCom = newGo.GetComponent<ED_MapCom>();
            mapCom.mapId     = int.Parse(name);
            newGo.name       = name;
            Selection.activeGameObject = newGo;
            return mapCom;
        }

        public static ED_ActorCom CreateActorGo()
        {
            GameObject actorGo = AssetDatabase.LoadAssetAtPath<GameObject>(ActorPrefabPath);
            GameObject newGo = GameObject.Instantiate(actorGo);
            return newGo.AddComponent<ED_ActorCom>();
        }

        public static List<ED_MapCom> GetAllMaps()
        {
            List<ED_MapCom> maps = new List<ED_MapCom>();
            string[] tileAssetPath = new string[] { setting.MapSearchPath };
            string[] guids = AssetDatabase.FindAssets("t:Prefab", tileAssetPath);
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject mapGo = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (mapGo.GetComponent<ED_MapCom>()!=null)
                {
                    maps.Add(mapGo.GetComponent<ED_MapCom>());
                }
            }
            return maps;
        }

        public static string GetMapSearchPath()
        {
            return setting.MapSearchPath;   
        }

        public static List<ActorAssetGroup> GetAllActorGroup()
        {
            List<ActorAssetGroup> assetGroups = new List<ActorAssetGroup>();
            string[] tileAssetPath = new string[] { setting.ActorSearchPath };
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", tileAssetPath);
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ActorAssetGroup asset = AssetDatabase.LoadAssetAtPath<ActorAssetGroup>(path);
                if (asset != null)
                {
                    assetGroups.Add(asset);
                }
            }
            return assetGroups;
        }

        public static ActorAsset GetActorAsset(string name)
        {
            List<ActorAssetGroup> assetGroups = GetAllActorGroup();
            for (int i = 0; i < assetGroups.Count; i++)
            {
                foreach (var item in assetGroups[i].actorDict)
                {
                    if (item.Value.actorName == name)
                    {
                        return item.Value;
                    }
                }
            }
            return null;
        }

        public static string GetMapModelSavePath(string mapId)
        {
            return setting.MapExportSavePath + mapId + ".txt";
        }
    }
}
#endif
