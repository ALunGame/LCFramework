#if UNITY_EDITOR
using UnityEngine;

namespace LCMap
{
    public class MapSetting : ScriptableObject
    {
        [Header("演员配置搜索路径")]
        [SerializeField]
        public string ActorSearchPath = "Assets/Editor/Map/Actors";

        [Header("地图配置搜索路径")]
        [SerializeField]
        public string MapSearchPath = "Assets/Editor/Map/Maps";

        [Header("地图导出保存路径")]
        [SerializeField]
        public string MapExportSavePath = "Assets/Resources/Config/Map/";
    }
} 
#endif