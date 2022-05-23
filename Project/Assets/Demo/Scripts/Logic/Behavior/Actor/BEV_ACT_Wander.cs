using Demo.Com;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;

namespace Demo.Behavior
{
    public class BEV_ACT_Wander : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            //参数
            float wanderRange = workData.GetParam().GetFloat();
            //组件
            WanderCom wanderCom = workData.MEntity.GetCom<WanderCom>();
            wanderCom.WanderRange = wanderRange;
            wanderCom.Enable();
        }

        protected override int OnRunning(NodeData wData)
        {
            return NodeState.EXECUTING;
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
            EntityWorkData workData = wData as EntityWorkData;
            //组件
            WanderCom wanderCom = workData.MEntity.GetCom<WanderCom>();
            wanderCom.Disable();
        }
    }
}
