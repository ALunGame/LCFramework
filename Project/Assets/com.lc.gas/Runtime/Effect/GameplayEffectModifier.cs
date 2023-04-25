namespace LCGAS
{
    /// <summary>
    /// 属性运算符
    /// </summary>
    public enum ModifierOp
    {
        /// <summary>
        /// 加
        /// </summary>
        Add,
        /// <summary>
        /// 乘
        /// </summary>
        Multiply,
        /// <summary>
        /// 覆盖
        /// </summary>
        Cover,
    }

    public class ModifierAttr
    {
        /// <summary>
        /// 修改的属性名
        /// </summary>
        public AttributeName attrName = new AttributeName();

        /// <summary>
        /// 属性值类型
        /// </summary>
        public AttributeValueType attributeValueType;

        /// <summary>
        /// 目标
        /// </summary>
        public TargetType targetType;

        public float GetValue(GameplayEffectSpec pSpec)
        {
            AbilitySystemCom com = pSpec.GetAbilityComByType(targetType);
            if (com == null)
            {
                LCGAS.GASLocate.Log.LogError("ModifierAttr.GetValue失败,没有对应组件",attrName,pSpec.Model.name);
                return 0;
            }

            return com.GetValue(attrName.Name, attributeValueType);
        }

        public bool SetValue(GameplayEffectSpec pSpec, float pValue)
        {
            AbilitySystemCom com = pSpec.GetAbilityComByType(targetType);
            if (com == null)
            {
                LCGAS.GASLocate.Log.LogError("ModifierAttr.SetValue失败,没有对应组件",attrName,pSpec.Model.name);
                return false;
            }
            return com.SetValue(attrName.Name, pValue);
        }
    }
    
    /// <summary>
    /// 用于修改属性
    /// </summary>
    public class GameplayEffectModifier
    {
        #region Static

        public static float GetAttrValue(AbilitySystemCom pCom, string pAttrName, AttributeValueType pValueType)
        {
            if (pValueType == AttributeValueType.Base)
            {
                return pCom.Attr.GetBaseValue(pAttrName);
            }
            else if (pValueType == AttributeValueType.Curr)
            {
                return pCom.Attr.GetCurrValue(pAttrName);
            }
            else if (pValueType == AttributeValueType.Bonus)
            {
                return pCom.Attr.GetCurrValue(pAttrName) - pCom.Attr.GetBaseValue(pAttrName);
            }

            return 0;
        }


        #endregion

        /// <summary>
        /// 读取的属性
        /// </summary>
        public ModifierAttr readAttr = new ModifierAttr();

        /// <summary>
        /// 运算符
        /// </summary>
        public ModifierOp op;

        /// <summary>
        /// 运算值
        /// </summary>
        public GameplayEffectModifierMagnitude magnitude = new GameplayEffectModifierMagnitude();
        
        /// <summary>
        /// 设置的属性
        /// </summary>
        public ModifierAttr setAttr = new ModifierAttr();

        /// <summary>
        /// 来源标签条件
        /// </summary>
        public CommonConditionTag sourceTags = new CommonConditionTag();
        
        /// <summary>
        /// 目标标签条件
        /// </summary>
        public CommonConditionTag tagetTags = new CommonConditionTag();


        /// <summary>
        /// 计算
        /// </summary>
        /// <returns></returns>
        public bool Calc(GameplayEffectSpec pGameplayEffectSpec)
        {
            if (sourceTags!=null && !sourceTags.CheckAbilitySystemCom(pGameplayEffectSpec.SourceCom))
            {
                return false;
            }
            
            if (tagetTags!=null && !tagetTags.CheckAbilitySystemCom(pGameplayEffectSpec.TargetCom))
            {
                return false;
            }

            if (magnitude == null)
            {
                GASLocate.Log.LogError("属性计算出错,没有要计算的值",pGameplayEffectSpec.Model.name);
                return false;
            }

            //计算
            AbilitySystemCom targetCom = pGameplayEffectSpec.TargetCom;
            
            //读取的值
            float readValue = readAttr.GetValue(pGameplayEffectSpec);
            
            //计算值
            float magnitudeValue = magnitude.GetValue(pGameplayEffectSpec);

            float resValue = 0;
            if (op == ModifierOp.Add)
            {
                resValue = readValue + magnitudeValue;
            }
            else if (op == ModifierOp.Multiply)
            {
                resValue = readValue * magnitudeValue;
            }
            else if (op == ModifierOp.Cover)
            {
                resValue = magnitudeValue;
            }
            
            //设置
            return setAttr.SetValue(pGameplayEffectSpec, resValue);
        }
    }
}