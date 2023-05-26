using System.Collections.Generic;
using UnityEngine;
using MemoryPack;

namespace Map
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

    public class TbMapRoadCnf : Dictionary<int, Dictionary<int, MapRoadCnf>>
    {
        public bool Exist(int posX,int posY)
        {
            if (!ContainsKey(posX))
                return false;
            if (!this[posX].ContainsKey(posY))
                return false;
            return true;
        }

        public bool Exist(Vector2Int pos)
        {
            return Exist(pos.x,pos.y);
        }

        public bool CalcRoadPos(Vector3 pos,out Vector2Int roadPos)
        {
            int posX = (int)Mathf.RoundToInt(pos.x);
            int posY = (int)(pos.y - 0.3);
            roadPos = new Vector2Int(posX, posY);
            if (Exist(roadPos))
            {
                return true;
            }
            for (int x = -1; x < 1; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    Vector2Int nearPos = new Vector2Int(x, y) + roadPos;
                    if (Exist(nearPos))
                    {
                        roadPos = nearPos;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
