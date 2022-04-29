#if UNITY_EDITOR
using LCConfig;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

        static MapEditorDef()
        {
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

    }
}
#endif
