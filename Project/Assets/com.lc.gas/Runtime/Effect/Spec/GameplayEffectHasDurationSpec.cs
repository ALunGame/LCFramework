using System.Collections.Generic;
using Demo;
using Demo.Com;

namespace LCGAS
{
    /// <summary>
    /// 持续的效果
    /// </summary>
    public class GameplayEffectHasDurationSpec  : GameplayEffectSpec
    {
        private TimerInfo durationTimer;
        private TimerInfo periodTimer;
        
        public GameplayEffectHasDurationSpec(AbilitySystemCom pSourceCom, AbilitySystemCom pTargetCom, AbilitySystemCom pOwnerCom, GameplayEffect pGameplayEffect) : base(pSourceCom, pTargetCom, pOwnerCom, pGameplayEffect)
        {
        }
        
        public override void OnStart()
        {
            if (Model.duration == null)
            {
                LCGAS.GASLocate.Log.LogError("持续效果出错，没有持续配置",Model.name);
                return;
            }

            //持续时间
            durationTimer = GameLocate.TimerServer.WaitForSeconds(Model.duration.GetValue(this), () =>
            {
                TargetCom.StopGameplayEffect(this, true);
            });
            
            //持续没有周期，就是一个立即
            GameplayEffectPeriod period = Model.period;
            if (period == null)
            {
                Active();
                return;
            }
            
            //周期激活
            periodTimer = GameLocate.TimerServer.LoopSecond(period.period, -1, () =>
            {
                Active();
            });
            
            //开始立即激活
            if (period.executeOnActive)
            {
                Active();
            }
        }

        public override void OnStop(bool pIsNormal)
        {
            GameLocate.TimerServer.StopTimer(durationTimer);
            GameLocate.TimerServer.StopTimer(periodTimer);

            if (pIsNormal)
            {
                if (Model.stack != null)
                {
                    StackExpirationPolicy expirationPolicy = Model.stack.expirationPolicy;
                    if (expirationPolicy == StackExpirationPolicy.ClearEntireStack)
                    {
                        List<GameplayEffectSpec> currSpecs = OwnerCom.EffectDict[Model.name];
                        for (int i = currSpecs.Count -1; i >= 0; i--)
                        {
                            OwnerCom.StopGameplayEffect(currSpecs[i], false);
                        }
                    }
                    if (expirationPolicy == StackExpirationPolicy.RefreshDuration)
                    {
                        List<GameplayEffectSpec> currSpecs = OwnerCom.EffectDict[Model.name];
                        foreach (GameplayEffectSpec currSpec in currSpecs)
                        {
                            ((GameplayEffectHasDurationSpec)currSpec).RefreshDuration();
                        }
                    }
                }
            }
        }
        
        public void RefreshDuration()
        {
            GameLocate.TimerServer.StopTimer(durationTimer);
            durationTimer = GameLocate.TimerServer.WaitForSeconds(Model.duration.GetValue(this), () =>
            {
                TargetCom.StopGameplayEffect(this, true);
            });
        }


    }
}