using Demo.Com;
using LCECS;
using LCECS.Core.Tree.Base;
using System;

namespace Demo.Decision
{
    public class DEC_PRE_CheckInputAction : NodePremise
    {
        public InputAction checkAction;
        [NonSerialized]
        private InputCom inputCom = null;

        public override bool OnMakeTrue(NodeData wData)
        {
            if (inputCom == null)
                inputCom = ECSLocate.ECS.GetWorld().GetCom<InputCom>();
            return checkAction == inputCom.CurrAction;
        }
    }
}
