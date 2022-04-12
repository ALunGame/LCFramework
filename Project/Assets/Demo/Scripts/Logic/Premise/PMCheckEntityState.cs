using Demo.Com;
using LCECS.Core.Tree.Base;
using LCECS.Data;

namespace Premise
{
    public class PMCheckEntityState : NodePremise
    {
        public EntityState entityState = EntityState.Normal;
        public override bool OnMakeTrue(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            StateCom stateCom = workData.MEntity.GetCom<StateCom>();

            if (stateCom == null)
            {
                return false;
            }

            return stateCom.CurState == entityState;
        }
    }
}
