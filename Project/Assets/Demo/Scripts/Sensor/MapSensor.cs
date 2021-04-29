using Demo.Com;
using LCECS;
using LCECS.Layer.Info;
using LCTileMap;
using UnityEngine;

namespace Demo.Info
{
    /// <summary>
    /// 地图信息
    /// </summary>
    [WorldSensor(SensorType.Map)]
    public class MapSensor : ISensor
    {
        private MapCom mapCom;

        private void Gollect()
        {
            if (mapCom == null)
            {
                mapCom = ECSLocate.ECS.GetGlobalSingleCom<MapCom>();
            }
        }

        /// <summary>
        /// 获得玩家位置转换的地图位置
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetPlayerMapPos()
        {
            Gollect();
            return mapCom.PlayerMapPos;
        }
    }
}