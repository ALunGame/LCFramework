using Demo.Com;
using LCECS.Core.Tree.Base;
using LCECS.Data;

namespace Premise
{
    [NodePremise("是否处于被攻击打断")]
    public class PMCheckIsBreak : NodePremise
    {
        public override bool OnMakeTrue(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            StateCom stateCom = workData.MEntity.GetCom<StateCom>();

            if (stateCom == null)
            {
                return false;
            }

            return stateCom.CurState == EntityState.Stop;
        }
    }
}
