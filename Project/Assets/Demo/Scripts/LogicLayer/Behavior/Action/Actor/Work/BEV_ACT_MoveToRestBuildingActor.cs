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
    /// <summary>
    /// 移动到休息建筑
    /// </summary>
    public class BEV_ACT_MoveToRestBuildingActor : NodeAction
    {

        protected override void OnEnter(NodeData wData)
        {
            // EntityWorkData workData = wData as EntityWorkData;
            //
            // //组件
            // WayPointMoveCom wayPointMoveCom = workData.MEntity.GetCom<WayPointMoveCom>();
            // List<Actor> homeActors = new List<Actor>();
            // foreach (var item in ActorMediator.GetActors(typeof(BuildingCom).FullName))
            // {
            //     BuildingCom buildingCom = item.GetCom<BuildingCom>();
            //     if (buildingCom.buildingType == BuildingType.Village_Rest)
            //         homeActors.Add(item);
            // }
            //
            // if (homeActors.Count <= 0)
            // {
            //     wayPointMoveCom.currRoadCnf = null;
            //     return;
            // }
            //
            // Actor selHomeActor = homeActors[Random.Range(0, homeActors.Count)];
            //
            // Actor actor      = LCMap.ActorMediator.GetActor(wData.Uid);
            // ActorCnf actorCnf   = LCConfig.Config.ActorCnf[actor.Id];
            //
            // TransCom targetTransformCom = selHomeActor.GetCom<TransCom>();
            // TransCom transformCom = workData.MEntity.GetCom<TransCom>();
            // if (Vector2.Distance(targetTransformCom.Pos, transformCom.Pos) <= actorCnf.interactiveRange)
            // {
            //     wayPointMoveCom.currRoadCnf = null;
            //     return;
            // }
            //
            // if (WayPointMoveSystem.RoadCnf.CalcRoadPos(selHomeActor.Pos, out var endPos))
            // {
            //     wayPointMoveCom.SetWayPointTarget(endPos);
            // }
        }

        protected override int OnRunning(NodeData wData)
        {
            // EntityWorkData workData = wData as EntityWorkData;
            // WayPointMoveCom wayPointMoveCom = workData.MEntity.GetCom<WayPointMoveCom>();
            // if (wayPointMoveCom.CheckIsFinish())
            // {
            //     return NodeState.FINISHED;
            // }
            return NodeState.EXECUTING;
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
            // EntityWorkData workData = wData as EntityWorkData;
            // WayPointMoveCom wayPointMoveCom = workData.MEntity.GetCom<WayPointMoveCom>();
            // wayPointMoveCom.Clear();
        }
    }
}