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
    /// 移动到采集演员处
    /// </summary>
    public class BEV_ACT_MoveToCollectActor : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            //组件
            CollectCom collectCom = workData.MEntity.GetCom<CollectCom>();
            WayPointMoveCom wayPointMoveCom = workData.MEntity.GetCom<WayPointMoveCom>();
            List<ActorObj> actors = MapLocate.Map.GetActors(collectCom.collectActorId);

            if (actors.Count <= 0)
            {
                wayPointMoveCom.currRoadCnf = null;
                return;
            }

            ActorObj actor = LCMap.MapLocate.Map.GetActor(wData.Uid);
            ActorCnf actorCnf = LCConfig.Config.ActorCnf[actor.Id];
            TransformCom targetTransformCom = actors[0].Entity.GetCom<TransformCom>();
            TransformCom transformCom = workData.MEntity.GetCom<TransformCom>();
            if (Vector2.Distance(targetTransformCom.GetPos(), transformCom.GetPos()) <= actorCnf.interactiveRange)
            {
                wayPointMoveCom.currRoadCnf = null;
                return;
            }

            if (WayPointMoveSystem.RoadCnf.CalcRoadPos(actors[0].transform.position, out var endPos))
            {
                wData.AddBlackboardValue(BEV_BlackboardKey.InteractiveActorUid, actors[0].Uid);
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
