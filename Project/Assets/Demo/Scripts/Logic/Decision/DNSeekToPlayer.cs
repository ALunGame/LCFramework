using Demo.Com;
using Demo.Config;
using Demo.Info;
using LCECS;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.DecNode
{
    /// <summary>
    /// 寻路玩家
    /// </summary>
    public class DNSeekToPlayer : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            MapSensor mapSensor = ECSLayerLocate.Info.GetSensor<MapSensor>(SensorType.Map);

            //发送请求
            //ParamData paramData = workData.GetReqParam((int)BevType.SeekPath);
            //paramData.SetVect2Int(mapSensor.GetPlayerMapPos());
            //ECSLayerLocate.Request.PushRequest(workData.MEntity.GetHashCode(), (int)BevType.SeekPath);
        }

        //获得玩家寻路点
        private Vector2Int GetPlayerSeekPoint(EntityWorkData workData,Vector2Int playerPos)
        {
            SeekPathCom seekPathCom = workData.MEntity.GetCom<SeekPathCom>();
            Vector2Int curPos       = seekPathCom.CurrPos;
            //MapData mapData         = TempConfig.GetMapData(seekPathCom.MapPos);

            //找到玩家周围可寻路的点
            List<Vector2Int> canSeekPointList = new List<Vector2Int>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <=1; j++)
                {
                    Vector2Int pos = new Vector2Int(playerPos.x + i, playerPos.y + j);
                    //if (!mapData.ObstaclePos.Contains(pos)&& !pos.Equals(playerPos))
                    //{
                    //    canSeekPointList.Add(pos);
                    //}
                }
            }

            Vector2Int resPoint = playerPos;
            //根据优先级调整位置
            //1，距离
            float distance = 9999;
            for (int i = 0; i < canSeekPointList.Count; i++)
            {
                float tmpDis = Vector2Int.Distance(playerPos, canSeekPointList[i]);
                if (tmpDis<=distance)
                {
                    resPoint = canSeekPointList[i];
                }
            }

            return resPoint;
        }
    }
}
