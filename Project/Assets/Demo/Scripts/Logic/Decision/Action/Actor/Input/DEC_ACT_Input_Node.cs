using Demo.Com;
using LCECS;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using System;

namespace Demo.Decision
{
    public sealed class DEC_ACT_Input_Node : NodeAction
    {
        [NonSerialized]
        private InputCom inputCom = null;

        [NonSerialized]
        private ParamData paramData = new ParamData();

        protected override void OnEnter(NodeData wData)
        {
            if (inputCom == null)
                inputCom = ECSLocate.ECS.GetWorld().GetCom<InputCom>();
            Enter(wData);
        }

        protected override int OnRunning(NodeData wData)
        {
            return Running(wData);
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
            inputCom.ClearAction();
            Exit(wData, runningStatus);
        }

        private void Enter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            switch (inputCom.CurrAction)
            {
                case InputAction.None:
                    break;
                case InputAction.Move:
                    paramData.SetVect2(inputCom.Param.GetVect2());
                    ECSLayerLocate.Request.PushRequest(workData.MEntity.GetHashCode(), RequestId.Move, paramData);
                    return;
                case InputAction.Skill:
                    paramData.SetString(inputCom.Param.GetString());
                    ECSLayerLocate.Request.PushRequest(workData.MEntity.GetHashCode(), RequestId.PushSkill, paramData);
                    return;
                default:
                    break;
            }
        }

        private int Running(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            switch (inputCom.CurrAction)
            {
                case InputAction.None:
                    break;
                case InputAction.Move:
                    break;
                case InputAction.Skill:
                    if (workData.CurrReqId == LCECS.RequestId.PushSkill)
                        return NodeState.EXECUTING;
                    else
                        return NodeState.FINISHED;
                default:
                    break;
            }
            return NodeState.FINISHED;
        }

        private void Exit(NodeData wData, int runningStatus)
        {
            inputCom.ClearAction();
            EntityWorkData workData = wData as EntityWorkData;
            switch (inputCom.CurrAction)
            {
                case InputAction.None:
                    break;
                case InputAction.Move:
                    break;
                case InputAction.Skill:
                    workData.ChangeRequestId(RequestId.None);
                    break;
                default:
                    break;
            }
        }
    }
}
