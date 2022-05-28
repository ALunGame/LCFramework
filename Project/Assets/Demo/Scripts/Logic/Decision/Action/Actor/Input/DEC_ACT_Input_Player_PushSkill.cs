using Demo.Com;
using LCECS;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using System;

namespace Demo.Decision
{
    public class DEC_ACT_Input_Player_PushSkill : NodeAction
    {
        [NonSerialized]
        private ParamData paramData = new ParamData();
        [NonSerialized]
        private InputCom inputCom = null;

        protected override void OnEnter(NodeData wData)
        {
            if (inputCom == null)
                inputCom = ECSLocate.ECS.GetWorld().GetCom<InputCom>();
            paramData.SetString(inputCom.Param.GetString());
            EntityWorkData workData = wData as EntityWorkData;
            ECSLayerLocate.Request.PushRequest(workData.MEntity.GetHashCode(), RequestId.PushSkill, paramData);
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
            workData.ChangeRequestId(RequestId.None);
        }
    }
}
