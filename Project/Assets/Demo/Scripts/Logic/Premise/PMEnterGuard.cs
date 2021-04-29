using Demo.Com;
using Demo.Info;
using LCECS;
using LCECS.Core.Tree.Base;
using LCECS.Data;
using LCHelp;
using System.Collections.Generic;
using UnityEngine;

namespace DecNode.Premise
{
    [NodePremise("进入警戒区域")]
    public class PMEnterGuard : NodePremise
    {
        public override bool OnMakeTrue(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            EnemyCom enemyCom = workData.MEntity.GetCom<EnemyCom>();
            SeekPathCom seekPathCom = workData.MEntity.GetCom<SeekPathCom>();

            MapSensor mapSensor = ECSLayerLocate.Info.GetSensor<MapSensor>(SensorType.Map);

            bool value = LCRect.CheckPointInMidRect(seekPathCom.CurrPos, enemyCom.GuardArea, mapSensor.GetPlayerMapPos());
#if UNITY_EDITOR
            AddGuardArea(workData.MEntity.GetHashCode(), seekPathCom.CurrPos+seekPathCom.MapPos, enemyCom.GuardArea);
#endif

            return value;
        }

#if UNITY_EDITOR
        private Dictionary<int, Rect> EntityGuardRect = new Dictionary<int, Rect>();
        private void DrawGuardArea()
        {
            foreach (var item in new List<Rect>(EntityGuardRect.Values))
            {
                EDGizmos.DrawRect(item, Color.yellow);
            }
        }

        private void AddGuardArea(int id,Vector2Int point,Vector2Int size)
        {
            if (EntityGuardRect.ContainsKey(id))
            {
                EntityGuardRect[id] = LCRect.GetMidRcet(point, size);
            }
            else
            {
                EntityGuardRect.Add(id,LCRect.GetMidRcet(point, size));
            }
        }
#endif
    }

}