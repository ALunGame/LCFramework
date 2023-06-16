using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;

namespace Demo.Behavior
{
    public class BEV_ACT_StopBev : NodeAction
    {
        protected override int OnRunning(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            if (workData.CurrReqId == LCECS.RequestId.StopBev)
            {
                return NodeState.EXECUTING;
            }
            return NodeState.FINISHED;
        }
    }
}