using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using System;

namespace Demo.Decision
{
    public class DEC_ACT_PushSkill : NodeAction
    {
        public string skillId;

        [NonSerialized]
        private ParamData paramData = new ParamData();

        protected override void OnEnter(NodeData wData)
        {
            paramData.SetString(skillId);
            EntityWorkData workData = wData as EntityWorkData;
            LCECS.ECSLayerLocate.Request.PushRequest(workData.MEntity.GetHashCode(), LCECS.RequestId.PushSkill, paramData);
        }

        protected override int OnRunning(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            if (workData.CurrReqId == LCECS.RequestId.PushSkill)
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
