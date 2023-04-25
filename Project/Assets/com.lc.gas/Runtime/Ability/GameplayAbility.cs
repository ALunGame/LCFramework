using System.Collections.Generic;
using LCToolkit;

namespace LCGAS
{
    public class InternalGameplayAbility
    {
        /// <summary>
        /// 该GA的标签。
        /// </summary>
        public GameplayTagContainer tags = new GameplayTagContainer();
        
        /// <summary>
        /// 激活条件标签
        /// </summary>
        public CommonConditionTag conditionTag = new CommonConditionTag();

        /// 激活GA时，赋予ASC所选GA 标签
        /// </summary>
        public GameplayTagContainer activeOwnedTags = new GameplayTagContainer();
        
        /// <summary>
        /// 激活该GA时，打断其他拥有所选标签的GA。
        /// </summary>
        public GameplayTagContainer cancelTags = new GameplayTagContainer();
        
        /// <summary>
        /// 激活该GA时，阻止激活拥有所选标签的GA（已经激活的不会被中断）。
        /// </summary>
        public GameplayTagContainer blockTags = new GameplayTagContainer();

        /// <summary>
        /// 能力消耗
        /// 1，Instant类型，立即生效
        /// </summary>
        public GameplayEffectName cost = new GameplayEffectName();
        
        /// <summary>
        /// 能力冷却
        /// 1，Has Duration 的效果
        /// 2，在这个效果内会带有改效果的Tag，通过Tag来限制不会再次触发
        /// </summary>
        public GameplayEffectName cooldown = new GameplayEffectName();
        
        /// <summary>
        /// 能力触发标签
        /// 1，通过拥有者捕获 带有该标签的 GameplayEvent 事件 来触发能力
        /// </summary>
        public GameplayTagContainer triggerTags = new GameplayTagContainer();
    }
    
    /// <summary>
    /// 描述演员的能力
    /// 0，他是一个模板类
    /// 1，演员的行为或者技能
    /// 2，跳跃也可以也是一种能力
    /// 3，基本的UI交互和移动行为 建议不做成能力
    /// </summary>
    public abstract class GameplayAbility : InternalGameplayAbility
    {
        public abstract GameplayAbilitySpec CreateSpec(AbilitySystemCom pOwnerCom);
    }
}