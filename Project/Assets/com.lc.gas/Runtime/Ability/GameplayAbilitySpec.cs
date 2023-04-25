using System.Collections.Generic;
using LCToolkit;
using UnityEngine;

namespace LCGAS
{
    /// <summary>
    /// 根据GameplayAbility生成的能力示例
    /// </summary>
    public class GameplayAbilitySpec
    {
        /// <summary>
        /// 配置数据
        /// </summary>
        public GameplayAbility Model { get; private set; }
        
        /// <summary>
        /// 持有者
        /// </summary>
        public AbilitySystemCom OwnerCom { get; private set; }
        
        /// <summary>
        /// 激活状态
        /// </summary>
        public bool IsActive { get; private set; }

        public GameplayAbilitySpec(AbilitySystemCom pOwnerCom, GameplayAbility pModel)
        {
            Model = pModel;
            OwnerCom = pOwnerCom;
        }

        /// <summary>
        /// 尝试激活
        /// </summary>
        /// <returns></returns>
        public bool TryActive(params object[] pParams)
        {
            if (!CheckCanActive())
            {
                EndActive();
                return false;
            }

            IsActive = true;
            PreActive(pParams);
            Active(pParams);
            return true;
        }

        /// <summary>
        /// 准备激活阶段
        /// </summary>
        protected virtual void PreActive(params object[] pParams)
        {
            
        }
        
        /// <summary>
        /// 激活
        /// </summary>
        protected void Active(params object[] pParams)
        {
            //冷却
            if (Model.cooldown.NotNull())
            {
                OwnerCom.AddGameplayEffect(Model.cooldown, null);
            }

            //消耗
            if (Model.cost.NotNull())
            {
                OwnerCom.AddGameplayEffect(Model.cost, null);
            }
            
            //添加标签
            OwnerCom.AddTag(Model.activeOwnedTags);

            OnActive(pParams);
        }

        /// <summary>
        /// 当激活时
        /// </summary>
        protected virtual void OnActive(params object[] pParams)
        {
            
        }

        /// <summary>
        /// 结束激活
        /// </summary>
        public void EndActive()
        {
            if (!IsActive)
            {
                return;   
            }
            
            IsActive = false;
            
            //冷却
            if (Model.cooldown.NotNull())
            {
                List<GameplayEffectSpec> coolDownSpecs = OwnerCom.GetGameplayEffects(Model.cooldown.Name);
                if (coolDownSpecs.IsLegal())
                {
                    for (int i = 0; i < coolDownSpecs.Count; i++)
                    {
                        OwnerCom.StopGameplayEffect(coolDownSpecs[i], false);
                    }
                }
            }

            //消耗
            if (Model.cost.NotNull())
            {
                List<GameplayEffectSpec> costSpecs = OwnerCom.GetGameplayEffects(Model.cost.Name);
                if (costSpecs.IsLegal())
                {
                    for (int i = 0; i < costSpecs.Count; i++)
                    {
                        OwnerCom.StopGameplayEffect(costSpecs[i], false);
                    }
                }
            }
            
            //删除标签
            OwnerCom.RemoveTag(Model.activeOwnedTags);
            
            OnEndActive();
        }

        /// <summary>
        /// 当结束激活时
        /// </summary>
        protected virtual void OnEndActive()
        {
            
        }

        #region Check

        public virtual bool CheckCanActive()
        {
            return !IsActive
                   && CheckGameplayTags()
                   && CheckBlockTags()
                   && CheckCost()
                   && CheckCooldown();
        }

        public virtual bool CheckBlockTags()
        {
            //检测锁定标签
            List<GameplayAbilitySpec> activeAbilitys = OwnerCom.GetActiveGameplayAbilitys();
            foreach (GameplayAbilitySpec activeAbility in activeAbilitys)
            {
                if (activeAbility.Model.blockTags.NotNull())
                {
                    if (activeAbility.Model.blockTags.HasAny(Model.tags))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public virtual bool CheckGameplayTags()
        {
            if (Model.conditionTag == null)
            {
                return true;
            }
            return Model.conditionTag.CheckAbilitySystemCom(OwnerCom);
        }
        
        public virtual bool CheckCost()
        {
            if (Model.cost == null)
            {
                return true;
            }

            //临时产生的Spec
            GameplayEffectSpec costSpec = Model.cost.CreateSpec(null,OwnerCom,OwnerCom);
            if (costSpec.Model.type != GameplayEffectType.Instand)
            {
                LCGAS.GASLocate.Log.LogWarning("消耗不是立即的，直接跳过！！",GetType().Name);
                return true;
            }

            if (!costSpec.Model.modifiers.IsLegal())
            {
                return true;
            }
            
            for (var i = 0; i < costSpec.Model.modifiers.Count; i++)
            {
                GameplayEffectModifier modifier = costSpec.Model.modifiers[i];

                // 只关注Add操作
                if (modifier.op != ModifierOp.Add) continue;

                float costValue = modifier.magnitude.GetValue(costSpec);

                float readValue = modifier.readAttr.GetValue(costSpec);
                
                if (readValue+costValue<0) return false;

            }
            return true;
        }
        
        public virtual bool CheckCooldown()
        {
            if (Model.cooldown == null)
            {
                return true;
            }

            //还有冷却GE生效
            if (OwnerCom.GetGameplayEffectCnt(Model.cooldown.Name) > 0)
            {
                return false;
            }

            return OwnerCom.CheckCanAddGameplayEffect(Model.cooldown.GetModel());
        }
        
        #endregion
    }
}