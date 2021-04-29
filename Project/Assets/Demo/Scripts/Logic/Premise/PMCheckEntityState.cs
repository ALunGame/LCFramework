using Demo.Com;
using LCECS.Core.Tree.Base;
using LCECS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
