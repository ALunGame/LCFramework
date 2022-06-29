using Demo.Com;
using Demo.System;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using LCMap;
using UnityEngine;

namespace Demo.Behavior
{
    /// <summary>
    /// 移动到存储演员处
    /// </summary>
    public class BEV_ACT_MoveToStorageActor : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            //组件
            WorkerCom workerCom = workData.MEntity.GetCom<WorkerCom>();
            WayPointMoveCom wayPointMoveCom = workData.MEntity.GetCom<WayPointMoveCom>();

            if (workerCom.managerActor == null)
            {
                wayPointMoveCom.currRoadCnf = null;
                return;
            }

            ActorObj buildingActor = workerCom.managerActor.Entity.GetCom<ManagerCom>().buildingActor;
            ActorObj actor         = LCMap.MapLocate.Map.GetActor(wData.Uid);
            ActorCnf actorCnf      = LCConfig.Config.ActorCnf[actor.Id];

            TransformCom targetTransformCom = buildingActor.Entity.GetCom<TransformCom>();
            TransformCom transformCom = workData.MEntity.GetCom<TransformCom>();
            if (Vector2.Distance(targetTransformCom.GetPos(), transformCom.GetPos()) <= actorCnf.interactiveRange)
            {
                wayPointMoveCom.currRoadCnf = null;
                return;
            }

            if (WayPointMoveSystem.RoadCnf.CalcRoadPos(buildingActor.transform.position, out var endPos))
            {
                wData.AddBlackboardValue(BEV_BlackboardKey.InteractiveActorUid, buildingActor.Uid);
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
