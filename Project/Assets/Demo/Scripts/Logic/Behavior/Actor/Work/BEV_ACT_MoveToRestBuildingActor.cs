using Demo.Com;
using Demo.System;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using LCMap;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Behavior
{
    /// <summary>
    /// 移动到休息建筑
    /// </summary>
    public class BEV_ACT_MoveToRestBuildingActor : NodeAction
    {

        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            //组件
            WayPointMoveCom wayPointMoveCom = workData.MEntity.GetCom<WayPointMoveCom>();
            List<ActorObj> homeActors = new List<ActorObj>();
            foreach (var item in MapLocate.Map.GetActors(typeof(BuildingCom).FullName))
            {
                BuildingCom buildingCom = item.Entity.GetCom<BuildingCom>();
                if (buildingCom.buildingType == BuildingType.Village_Rest)
                    homeActors.Add(item);
            }

            if (homeActors.Count <= 0)
            {
                wayPointMoveCom.currRoadCnf = null;
                return;
            }

            ActorObj selHomeActor = homeActors[Random.Range(0, homeActors.Count)];

            ActorObj actor      = LCMap.MapLocate.Map.GetActor(wData.Uid);
            ActorCnf actorCnf   = LCConfig.Config.ActorCnf[actor.Id];

            TransformCom targetTransformCom = selHomeActor.Entity.GetCom<TransformCom>();
            TransformCom transformCom = workData.MEntity.GetCom<TransformCom>();
            if (Vector2.Distance(targetTransformCom.GetPos(), transformCom.GetPos()) <= actorCnf.interactiveRange)
            {
                wayPointMoveCom.currRoadCnf = null;
                return;
            }

            if (WayPointMoveSystem.RoadCnf.CalcRoadPos(selHomeActor.transform.position, out var endPos))
            {
                wayPointMoveCom.SetWayPointTarget(endPos);
            }
        }

        protected override int OnRunning(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            WayPointMoveCom wayPointMoveCom = workData.MEntity.GetCom<WayPointMoveCom>();
            if (wayPointMoveCom.CheckIsFinish())
            {
                return NodeState.FINISHED;
            }
            return NodeState.EXECUTING;
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
            EntityWorkData workData = wData as EntityWorkData;
            WayPointMoveCom wayPointMoveCom = workData.MEntity.GetCom<WayPointMoveCom>();
            wayPointMoveCom.Clear();
        }
    }
}