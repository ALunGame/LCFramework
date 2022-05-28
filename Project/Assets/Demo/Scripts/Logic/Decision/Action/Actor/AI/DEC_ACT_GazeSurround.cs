using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using System;
using UnityEngine;

namespace Demo.Decision
{
    /// <summary>
    /// 注视环绕
    /// </summary>
    public class DEC_ACT_GazeSurround : NodeAction
    {
        class GazeSurroundData : NodeContext
        {
            internal float Timer;

            public GazeSurroundData()
            {
                Timer = 0;
            }
        }

        [NonSerialized]
        private ParamData paramData = new ParamData();

        public Vector2 gazeRange;
        public float gazeTimer;

        protected override void OnEnter(NodeData wData)
        {
            if (!wData.Blackboard.ContainsKey(DEC_PRE_CheckEnemyInAttackRange.EnemyInAttackRangeKey))
                return;

            GazeSurroundData context = GetContext<GazeSurroundData>(wData);
            context.Timer = NodeTime.TotalTime;

            int followEntityUid = (int)wData.Blackboard[DEC_PRE_CheckEnemyInAttackRange.EnemyInAttackRangeKey];
            paramData.SetInt(followEntityUid);
            paramData.SetVect2(gazeRange);

            EntityWorkData workData = wData as EntityWorkData;
            LCECS.ECSLayerLocate.Request.PushRequest(workData.MEntity.GetHashCode(), LCECS.RequestId.GazeSurround, paramData);
        }

        protected override int OnRunning(NodeData wData)
        {
            GazeSurroundData context = GetContext<GazeSurroundData>(wData);
            if (NodeTime.TotalTime - context.Timer > gazeTimer)
                return NodeState.FINISHED;
            return NodeState.EXECUTING;
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
            GazeSurroundData context = GetContext<GazeSurroundData>(wData);
            context.Timer = 0;
            EntityWorkData workData = wData as EntityWorkData;
            workData.ChangeRequestId(LCECS.RequestId.None);
        }
    }
}
