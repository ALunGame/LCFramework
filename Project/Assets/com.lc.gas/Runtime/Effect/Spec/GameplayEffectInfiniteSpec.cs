using Demo;
using Demo.Com;

namespace LCGAS
{
    /// <summary>
    /// 永久的效果
    /// </summary>
    public class GameplayEffectInfiniteSpec : GameplayEffectSpec
    {
        private TimerInfo periodTimer;

        public GameplayEffectInfiniteSpec(AbilitySystemCom pSourceCom, AbilitySystemCom pTargetCom, AbilitySystemCom pOwnerCom, GameplayEffect pGameplayEffect) : base(pSourceCom, pTargetCom, pOwnerCom, pGameplayEffect)
        {
        }
        
        public override void OnStart()
        {
            //永久没有周期，就是一个立即
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
            GameLocate.TimerServer.StopTimer(periodTimer);
        }


    }
}