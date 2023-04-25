namespace LCGAS
{
    public enum AttributeValueType
    {
        /// <summary>
        /// 基础值
        /// </summary>
        Base,
        /// <summary>
        /// 当前值
        /// </summary>
        Curr,
        /// <summary>
        /// 奖励值 当前值 - 基础值
        /// </summary>
        Bonus,
    }

    /// <summary>
    /// 属性运算值
    /// </summary>
    public class GameplayEffectModifierMagnitude
    {

        public ModifierMagnitude magnitude;

        public float GetValue(GameplayEffectSpec pEffectSpec)
        {
            if (magnitude == null)
            {
                return 0;
            }

            return magnitude.GetValue(pEffectSpec);
        }
    }


    public abstract class ModifierMagnitude
    {
        public abstract float GetValue(GameplayEffectSpec pEffectSpec);
    }
    
    /// <summary>
    /// 直接一个浮点数
    /// </summary>
    public class GameplayEffectModifierMagnitudeFloat : ModifierMagnitude
    { 
        public float value;
        
        public override float GetValue(GameplayEffectSpec pEffectSpec)
        {
            return value;
        }
    }
    
    /// <summary>
    /// 读取来源或者目标的属性作为值
    /// </summary>
    public class GameplayEffectModifierMagnitudeAttribute : ModifierMagnitude
    {
        /// <summary>
        /// 读取的值
        /// </summary>
        public ModifierAttr readAttr = new ModifierAttr();
        
        public override float GetValue(GameplayEffectSpec pEffectSpec)
        {
            return readAttr.GetValue(pEffectSpec);
        }
    }
    
    /// <summary>
    /// 自定义值获取
    /// </summary>
    public abstract class ModifierMagnitudeCustom : ModifierMagnitude
    {
    }
    
    /// <summary>
    /// 自定义值获取
    /// </summary>
    public class GameplayEffectModifierMagnitudeByUI : ModifierMagnitudeCustom
    {
        public float value;

        public override float GetValue(GameplayEffectSpec pEffectSpec)
        {
            return 0;
        }
    }
}