using Demo.Com;
using Demo.Help;
using Demo.Info;
using LCECS;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Data;
using LCToolkit;
using System.Collections.Generic;
using UnityEngine;

namespace DecNode.Premise
{
    public class PMEnterAttack : NodePremise
    {
        public override bool OnMakeTrue(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            EnemyCom enemyCom = workData.MEntity.GetCom<EnemyCom>();
            SeekPathCom seekPathCom = workData.MEntity.GetCom<SeekPathCom>();

            MapSensor mapSensor = ECSLayerLocate.Info.GetSensor<MapSensor>(SensorType.Map);

            bool value = MapHelp.CheckPointInArea(seekPathCom.CurrPos, enemyCom.AttackArea, mapSensor.GetPlayerMapPos());
            return value;
        }
    }
}
