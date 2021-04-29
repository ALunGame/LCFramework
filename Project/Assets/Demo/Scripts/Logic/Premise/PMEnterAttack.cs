using Demo.Com;
using Demo.Help;
using Demo.Info;
using LCECS;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Data;
using LCHelp;
using System.Collections.Generic;
using UnityEngine;

namespace DecNode.Premise
{
    [NodePremise("进入攻击区域")]
    public class PMEnterAttack : NodePremise
    {
        public override bool OnMakeTrue(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            EnemyCom enemyCom = workData.MEntity.GetCom<EnemyCom>();
            SeekPathCom seekPathCom = workData.MEntity.GetCom<SeekPathCom>();

            MapSensor mapSensor = ECSLayerLocate.Info.GetSensor<MapSensor>(SensorType.Map);

            bool value = MapHelp.CheckPointInArea(seekPathCom.CurrPos, enemyCom.AttackArea, mapSensor.GetPlayerMapPos());

#if UNITY_EDITOR
            AddAttackArea(workData.MEntity.GetHashCode(), seekPathCom.CurrPos + seekPathCom.MapPos, enemyCom.AttackArea);
#endif

            return value;
        }

#if UNITY_EDITOR
        private Dictionary<int, Rect> EntityAttackRect = new Dictionary<int, Rect>();

        private void DrawAttackArea()
        {
            foreach (var item in EntityAttackRect.Values)
            {
                EDGizmos.DrawRect(item, Color.red);
            }
        }

        private void AddAttackArea(int id, Vector2Int point, Vector2Int size)
        {
            if (EntityAttackRect.ContainsKey(id))
            {
                EntityAttackRect[id] = LCRect.GetMidRcet(point, size);
            }
            else
            {
                EntityAttackRect.Add(id, LCRect.GetMidRcet(point, size));
            }
        }
#endif
    }
}
