using System.Collections.Generic;
using LCMap;
using LCToolkit;
using Map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo.Logic
{
    /// <summary>
    /// 寻路部分信息
    /// </summary>
    public class SeekPartInfo
    {
        
    }
    
    /// <summary>
    /// 寻路结果
    /// </summary>
    public class SeekResultInfo
    {
        public List<SeekPartInfo> partInfos = new List<SeekPartInfo>();
    }
    
    public class MapSeekPathLogic : BaseServerLogicModule<MapServer>
    {
        private Dictionary<int, MapRoadCnf> mapRoadCnfDict = new Dictionary<int, MapRoadCnf>();

        private Dictionary<MapArea, Tilemap> areaPathTileMapDict = new Dictionary<MapArea, Tilemap>();
        
        public override void OnInit()
        {
            foreach (MapArea area in server.areaDict.Values)
            {
                if (area.AreaEnvGo.transform.Find("TileMap/Path",out Transform pathTrans))
                {
                    areaPathTileMapDict.Add(area,pathTrans.GetComponent<Tilemap>());
                }
            }
        }

        public override void OnClear()
        {
            areaPathTileMapDict.Clear();
        }
        
        
        public SeekResultInfo FindPath(Vector2Int pCurrPos, Vector2Int pTargetPos)
        {
            int currMapId = MapLocate.Map.CurrMapId;
            MapRoadCnf cnf = mapRoadCnfDict[currMapId];

            
            // if (!cnf.Exist(pCurrPos))
            // {
            //     MapLocate.Log.LogError("FindPath失败，当前点是阻挡",pCurrPos);
            //     return null;
            // }
            //
            // if (!cnf.Exist(pTargetPos))
            // {
            //     MapLocate.Log.LogError("FindPath失败，目标点是阻挡",pTargetPos);
            //     return null;
            // }
            //
            // SeekResultInfo resultInfo = new SeekResultInfo();

            return null;

        }
    }
}