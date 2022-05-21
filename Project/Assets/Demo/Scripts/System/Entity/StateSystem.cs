using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.System
{
    public class StateSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(AttributeCom), typeof(StateCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            UpdateState(comList);
        }

        private void UpdateState(List<BaseCom> comList)
        {
            AttributeCom attributeCom = GetCom<AttributeCom>(comList[0]);
            StateCom stateCom = GetCom<StateCom>(comList[1]);

            float currHp = attributeCom.AttrDict["Hp"];
            if (currHp <= 0)
            {
                stateCom.CurState = EntityState.Dead;
            }
        }
    }
}
