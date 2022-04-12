using Demo.Com;
using LCECS.Core.Tree.Base;
using LCECS.Data;

namespace Premise
{
    public class PMCheckIsDead : NodePremise
    {
        public override bool OnMakeTrue(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            StateCom stateCom = workData.MEntity.GetCom<StateCom>();

            if (stateCom == null)
            {
                return false;
            }

            return stateCom.CurState == EntityState.Dead;
        }
    }
}
