using System;
using LCToolkit;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LCGAS
{
    public class GameplayEffectSpec
    {
        #region Static

        /// <summary>
        /// 尝试应用GE
        /// </summary>
        /// <param name="pTargetCom">目标</param>
        /// <param name="pGameplayEffect"></param>
        /// <returns></returns>
        public static bool CheckAddGameplayEffect(AbilitySystemCom pTargetCom, GameplayEffect pGameplayEffect)
        {
            //没有通过添加条件
            if (pGameplayEffect.tag.addConTag != null && !pGameplayEffect.tag.addConTag.CheckAbilitySystemCom(pTargetCom))
            {
                return false;
            }
            
            return true;
        }
        
        #endregion
        
        /// <summary>
        /// 配置数据
        /// </summary>
        public GameplayEffect Model { get; private set; }

        /// <summary>
        /// 来源
        /// </summary>
        public AbilitySystemCom SourceCom { get; private set; }
        
        /// <summary>
        /// 目标
        /// </summary>
        public AbilitySystemCom TargetCom { get; private set; }
        
        /// <summary>
        /// 持有者
        /// </summary>
        public AbilitySystemCom OwnerCom { get; private set; }

        public GameplayEffectSpec(AbilitySystemCom pSourceCom, AbilitySystemCom pTargetCom,AbilitySystemCom pOwnerCom, GameplayEffect pGameplayEffect)
        {
            SourceCom = pSourceCom;
            TargetCom = pTargetCom;
            OwnerCom = pOwnerCom;
            Model = pGameplayEffect;
        }

        /// <summary>
        /// 开始GE
        /// </summary>
        public void Start()
        {
            OnStart();
        }

        public virtual void OnStart()
        {
            
        }

        /// <summary>
        /// 结束GE
        /// </summary>
        /// <param name="pIsNormal">正常结束</param>
        public void Stop(bool pIsNormal)
        {
            //自身执行完毕
            OnStop(pIsNormal);

            //结束后的检测
            if (Model.expiration == null)
            {
                //TODO:执行特定Tag的GE
            }
        }
        
        public virtual void OnStop(bool pIsNormal)
        {
            
        }

        /// <summary>
        /// 激活
        /// 1，执行属性计算
        /// 2，广播GC
        /// </summary>
        protected void Active()
        {
            if (!CheckActive())
            {
                return;
            }
            
            //计算属性
            if (Model.modifiers.IsLegal())
            {
                for (int i = 0; i < Model.modifiers.Count; i++)
                {
                    GameplayEffectModifier modifier = Model.modifiers[i];
                    modifier.Calc(this);
                }
            }
            
            //TODO:广播GC
        }

        #region Check

        /// <summary>
        /// 是否可以激活，激活才可以改变属性
        /// </summary>
        /// <returns></returns>
        public bool CheckActive()
        {
            //没有通过添加条件
            if (Model.rate == null)
            {
                return true;
            }
            
            //检测标签
            if (Model.rate.conditionTag !=null && !Model.rate.conditionTag.CheckAbilitySystemCom(TargetCom))
            {
                return false;
            }
            
            //自定义函数
            if (Model.rate.checkFuns.IsLegal())
            {
                for (int i = 0; i < Model.rate.checkFuns.Count; i++)
                {
                    if (!GameplayEffectCheckActiveFuncMap.Check(this, Model.rate.checkFuns[i]))
                    {
                        return false;
                    }
                }
            }

            if (Model.rate.rateValue>=100)
            {
                return true;
            }

            int value = Random.Range(0, 101);
            
            return Model.rate.rateValue >= value;
        }

        #endregion

        #region Get

        public AbilitySystemCom GetAbilityComByType(TargetType pType)
        {
            if (pType == TargetType.Owner)
            {
                return OwnerCom;
            }
            else if (pType == TargetType.Target)
            {
                return TargetCom;
            }
            else if (pType == TargetType.Source)
            {
                return SourceCom;
            }
            return null;
        }
        

        #endregion
    }
}