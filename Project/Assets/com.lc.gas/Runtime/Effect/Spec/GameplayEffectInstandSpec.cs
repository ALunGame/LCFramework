namespace LCGAS
{
    /// <summary>
    /// 立即激活的效果
    /// </summary>
    public class GameplayEffectInstandSpec : GameplayEffectSpec
    {
        public GameplayEffectInstandSpec(AbilitySystemCom pSourceCom, AbilitySystemCom pTargetCom, AbilitySystemCom pOwnerCom, GameplayEffect pGameplayEffect) : base(pSourceCom, pTargetCom, pOwnerCom, pGameplayEffect)
        {
        }
        
        /// <summary>
        /// 立即的效果直接激活
        /// </summary>
        public override void OnStart()
        {
            //激活
            Active();
            
            //停止
            TargetCom.StopGameplayEffect(this,true);
        }


    }
}