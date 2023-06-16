using System.Collections.Generic;
using MemoryPack;
using UnityEngine;

namespace AStar
{
    public enum MapRoadType
    {
        Normal,
        
        /// <summary>
        /// 阻挡
        /// </summary>
        Obstruct = 999,
    }
    
    [MemoryPackable]
    public partial class RoadCnf
    {
        public Vector2Int tilePos;
        public Vector2 roadPos;
        public MapRoadType roadType;
    }

    [MemoryPackable]
    public partial class MapAreaRoadCnf
    {
        public int areaId;
        public List<RoadCnf> roads = new List<RoadCnf>();
    }

    [MemoryPackable]
    public partial class MapRoadCnf
    {
        public int mapId;
        public List<MapAreaRoadCnf> areaRoads = new List<MapAreaRoadCnf>();
    }

    public class RoadInfo
    {
        public MapRoadType roadType;

        public RoadInfo(MapRoadType pRoadType)
        {
            roadType = pRoadType;
        }

        public static RoadInfo ObsInfo = new RoadInfo(MapRoadType.Obstruct);
        public static RoadInfo NormalInfo = new RoadInfo(MapRoadType.Normal);
    }
}