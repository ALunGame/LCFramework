using Demo.Com;
using Demo.System;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using LCMap;
using System.Collections.Generic;

namespace Demo.Behavior
{
    public class BEV_ACT_Collect : NodeAction
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
                return;
            }
            if (WayPointMoveSystem.RoadCnf.CalcRoadPos(actors[0].transform.position, out var endPos))
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
