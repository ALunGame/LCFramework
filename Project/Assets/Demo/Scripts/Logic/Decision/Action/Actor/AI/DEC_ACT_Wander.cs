using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using System;

namespace Demo.Decision
{
    /// <summary>
    /// 徘徊
    /// </summary>
    public class DEC_ACT_Wander : NodeAction
    {
        //徘徊范围
        public float WanderRange = 0;

        [NonSerialized]
        private ParamData paramData = new ParamData();
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            paramData.SetFloat(WanderRange);

            LCECS.ECSLayerLocate.Request.PushRequest(workData.MEntity.GetHashCode(), LCECS.RequestId.Wander, paramData);
        }

        protected override int OnRunning(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            if (workData.CurrReqId == LCECS.RequestId.Wander)
            {
                return NodeState.EXECUTING;
            }
            return NodeState.FINISHED;
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
            EntityWorkData workData = wData as EntityWorkData;
            workData.ChangeRequestId(LCECS.RequestId.None);
        }
    }
}
