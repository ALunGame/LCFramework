using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using LCMap;

namespace Demo.System
{
    public class HealthSystem : BaseSystem
    {
        private DayNightCom dayNightCom;
        private bool needCostFood = false;

        protected override List<Type> RegContainListenComs()
        {
            //dayNightCom = LCECS.ECSLocate.ECS.GetWorld().GetCom<DayNightCom>();
            //dayNightCom.RegStageChange((stage) =>
            //{
            //    if (stage == DayNightStage.NightFull)
            //        needCostFood = true;
            //});
            return new List<Type>() {typeof(BasePropertyCom),typeof(ActorDisplayCom)};
        }

        protected override void OnAddCheckComs(List<BaseCom> comList)
        {
            BasePropertyCom propertyCom = GetCom<BasePropertyCom>(comList[0]);
            OnHpChange(propertyCom, propertyCom.Hp.Curr);
            propertyCom.Hp.RegChange((int currHp) =>
            {
                OnHpChange(propertyCom,currHp);
            });
        }

        protected override void OnRemoveCheckComs(List<BaseCom> comList)
        {
            
            base.OnRemoveCheckComs(comList);
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            //BasePropertyCom basePropertyCom = GetCom<BasePropertyCom>(comList[0]);
            //HandleCostFood(basePropertyCom);
        }

        private void HandleCostFood(BasePropertyCom basePropertyCom)
        {
            if (needCostFood)
            {
                // if (basePropertyCom.Food.Curr > 0)
                // {
                //     basePropertyCom.Food.Curr--;
                // }
                // else
                // {
                //     basePropertyCom.Hp.Curr--;
                // }
            }
        }

        #region 生命值

        private void OnHpChange(BasePropertyCom propertyCom,int currHp)
        {
            string stateName = "EmptyHp";
            if (currHp<=0)
            {
                stateName = "EmptyHp";
            }
            else
            {
                float rate = currHp / (float)propertyCom.Hp.Total;
                if (rate <= 0.3f)
                    stateName = "EmptyHp";
                else if(rate <= 0.6f)
                    stateName = "HalfHp";
                else
                    stateName = "FullHp";
            }
            
            ActorDisplayCom displayCom = LCECS.ECSLocate.ECS.GetEntity(propertyCom.EntityUid).GetCom<ActorDisplayCom>();
            if (displayCom.HasState(stateName))
            {
                displayCom.SetState(stateName);
            }
        }

        #endregion
    }
}
