using LCECS.Core;
using LCTileMap;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Com
{
    /// <summary>
    /// 地图组件
    /// </summary>
    [Com(ViewName = "地图组件", IsGlobal = true)]
    public class MapCom : BaseCom
    {
        public MapData CurrMap;
        public Dictionary<Vector2, List<Entity>> EntityDict = new Dictionary<Vector2, List<Entity>>();
        public Dictionary<Vector2, GameObject> CurrShowMapDict = new Dictionary<Vector2, GameObject>();
        public Dictionary<Vector2, GameObject> RecycleMapDict = new Dictionary<Vector2, GameObject>();

        [ComValue(ShowView = true)]
        public Vector2Int PlayerMapPos;

#if UNITY_EDITOR
        [ComValue(ShowView = true)]
        public string CurrMapName;
#endif
    }
}