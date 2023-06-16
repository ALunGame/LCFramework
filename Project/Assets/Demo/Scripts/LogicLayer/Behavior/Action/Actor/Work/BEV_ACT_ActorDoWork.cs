using Demo.Life.State.Content;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using LCMap;

namespace Demo.Behavior
{
    public class BEV_ACT_ActorDoWork : NodeAction
    {
        class BEV_ACT_ActorDoWork_Context : NodeContext
        {
            public ActorWorkContent currContent;
        }
        
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData entityWorkData = wData as EntityWorkData;
            Actor actor = entityWorkData.MEntity as Actor;
            BEV_ACT_ActorDoWork_Context context = GetContext<BEV_ACT_ActorDoWork_Context>(wData);
            
            context.currContent = actor.LifeCom.WorkContents.Peek();
            context.currContent?.DoWork();
        }

        protected override int OnRunning(NodeData wData)
        {
            BEV_ACT_ActorDoWork_Context context = GetContext<BEV_ACT_ActorDoWork_Context>(wData);

            if (context.currContent == null)
            {
                return NodeState.FINISHED;
            }
            
            if (context.currContent.State == WorkState.Doing)
            {
                return NodeState.EXECUTING;
            }
            
            return NodeState.FINISHED;
        }
    }
}