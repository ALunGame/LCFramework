using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;

namespace Demo.System
{
    public class HealthSystem : BaseSystem
    {
        private DayNightCom dayNightCom;
        private bool needCostFood = false;

        protected override List<Type> RegListenComs()
        {
            dayNightCom = LCECS.ECSLocate.ECS.GetWorld().GetCom<DayNightCom>();
            dayNightCom.RegStageChange((stage) =>
            {
                if (stage == DayNightStage.NightFull)
                    needCostFood = true;
            });
            return new List<Type>() {typeof(BasePropertyCom)};
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            BasePropertyCom basePropertyCom = GetCom<BasePropertyCom>(comList[0]);
            HandleCostFood(basePropertyCom);
        }

        private void HandleCostFood(BasePropertyCom basePropertyCom)
        {
            if (needCostFood)
            {
                if (basePropertyCom.Food.Curr > 0)
                {
                    basePropertyCom.Food.Curr--;
                }
                else
                {
                    basePropertyCom.Hp.Curr--;
                }
            }
        }
    }
}
