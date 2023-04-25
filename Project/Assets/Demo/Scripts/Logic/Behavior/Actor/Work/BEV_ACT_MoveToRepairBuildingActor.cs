using Demo.Com;
using Demo.System;
using LCECS.Core;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using LCMap;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Behavior
{
    public class BEV_ACT_MoveToRepairBuildingActor : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            // EntityWorkData workData = wData as EntityWorkData;
            //
            // //组件
            // WayPointMoveCom wayPointMoveCom = workData.MEntity.GetCom<WayPointMoveCom>();
            // List<Actor> buildingActors = new List<Actor>();
            // foreach (var item in ActorMediator.GetActors(typeof(BuildingCom).FullName))
            // {
            //     BasePropertyCom propertyCom = item.GetCom<BasePropertyCom>();
            //     if (propertyCom.Hp.Curr < propertyCom.Hp.Max)
            //         buildingActors.Add(item);
            // }
            //
            // if (buildingActors.Count <= 0)
            // {
            //     wayPointMoveCom.currRoadCnf = null;
            //     return;
            // }
            //
            // Actor actor              = LCMap.ActorMediator.GetActor(wData.Uid);
            // ActorCnf actorCnf           = LCConfig.Config.ActorCnf[actor.Id];
            // TransCom transformCom   = workData.MEntity.GetCom<TransCom>();
            //
            // Actor selBuildingActor   = buildingActors[Random.Range(0, buildingActors.Count)];
            // TransCom targetTransformCom = selBuildingActor.GetCom<TransCom>();
            //
            // if (Vector2.Distance(targetTransformCom.Pos, transformCom.Pos) <= actorCnf.interactiveRange)
            // {
            //     wayPointMoveCom.currRoadCnf = null;
            //     return;
            // }
            //
            // if (WayPointMoveSystem.RoadCnf.CalcRoadPos(targetTransformCom.Pos, out var endPos))
            // {
            //     wData.AddBlackboardValue(BEV_BlackboardKey.InteractiveActorUid, selBuildingActor.Uid);
            //     wayPointMoveCom.SetWayPointTarget(endPos);
            // }
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
