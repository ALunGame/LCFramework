using Demo.Life.State.Content;
using LCECS.Core.Tree.Base;
using LCECS.Data;
using LCMap;

namespace Demo.Decision
{
    /// <summary>
    /// 检测是否需要工作
    /// </summary>
    public class DEC_PRE_CheckNeedDoWork : NodePremise
    {
        public override bool OnMakeTrue(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            Actor actor = workData.MEntity as Actor;

            if (actor.LifeCom.WorkContents.Count <= 0)
            {
                return false;
            }

            ActorWorkContent workContent = actor.LifeCom.WorkContents.Peek();
            return workContent.CanDoWork();
        }
    }
}