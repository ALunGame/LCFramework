using System;
using System.Collections.Generic;
using System.Linq;
using LCECS.Core;

namespace LCGAS
{
    public abstract class AttributeValue
    {
        public abstract string Name { get; }
        public float BaseValue;
        public float CurrentValue;
    }

    public class OnAttributeChangeData
    {
        public float NewValue;
        public float OldValue;

        public OnAttributeChangeData(float pNewValue, float pOldValue)
        {
            NewValue = pNewValue;
            OldValue = pOldValue;
        }
    }  
    
    public delegate void OnAttributeChange(string pAttrName, OnAttributeChangeData pChangeData);
    
    public class AttributeSetBase
    {
        private Dictionary<string, AttributeValue> attrDict = new Dictionary<string, AttributeValue>();

        private Dictionary<string, OnAttributeChange> attrChangeDelDict = new Dictionary<string, OnAttributeChange>();
        //private event Action<string,float,float> 

        /// <summary>
        /// 拥有者
        /// </summary>
        public AbilitySystemCom Ower { get; private set; }

        public void Init(AbilitySystemCom pOwer)
        {
            Ower = pOwer;
        }

        public void Clear()
        {
            attrDict.Clear();
            attrChangeDelDict.Clear();
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        public void AddAttr(AttributeValue pAttrValue)
        {
            if (attrDict.ContainsKey(pAttrValue.Name))
            {
                return;
            }
            attrDict.Add(pAttrValue.Name,pAttrValue);
        }

        /// <summary>
        /// 获得属性基础值
        /// </summary>
        /// <param name="pAttrName"></param>
        /// <returns></returns>
        public float GetBaseValue(string pAttrName)
        {
            if (!attrDict.ContainsKey(pAttrName))
            {
                return 0;
            }

            return attrDict[pAttrName].BaseValue;
        }

        /// <summary>
        /// 获得属性当前值
        /// </summary>
        /// <param name="pAttrName"></param>
        /// <returns></returns>
        public float GetCurrValue(string pAttrName)
        {
            if (!attrDict.ContainsKey(pAttrName))
            {
                return 0;
            }
            
            return attrDict[pAttrName].CurrentValue;
        }
        
        /// <summary>
        /// 设置属性基础值
        /// </summary>
        /// <param name="pAttrName"></param>
        /// <param name="pNewValue"></param>
        /// <returns></returns>
        public bool SetBaseValue(string pAttrName, float pNewValue)
        {
            if (!attrDict.ContainsKey(pAttrName))
            {
                return false;
            }
            attrDict[pAttrName].BaseValue = pNewValue;
            PostGameplayEffectExecute(pAttrName, pNewValue);
            return true;
        }

        /// <summary>
        /// 设置属性当前值
        /// </summary>
        /// <param name="pAttrName"></param>
        /// <param name="pNewValue"></param>
        /// <returns></returns>
        public bool SetCurrValue(string pAttrName, float pNewValue)
        {
            if (!attrDict.ContainsKey(pAttrName))
            {
                return false;
            }
            
            PreAttributeChange(pAttrName, ref pNewValue);
            
            float oldValue = attrDict[pAttrName].CurrentValue;
            attrDict[pAttrName].CurrentValue = pNewValue;

            if (attrChangeDelDict.ContainsKey(pAttrName))
            {
                OnAttributeChangeData changeData = new OnAttributeChangeData(oldValue, pNewValue);
                attrChangeDelDict[pAttrName]?.Invoke(pAttrName,changeData);
            }
            return true;
        }

        public bool HasAttr(string pAttrName)
        {
            return attrDict.ContainsKey(pAttrName);
        }

        /// <summary>
        /// 当任一属性当前值改变调用 Infinite和Has Duration的GE
        /// </summary>
        public virtual void PreAttributeChange(string pAttrName, ref float pNewValue)
        {
        }
        
        /// <summary>
        /// 当任一属性基础值改变后调用 Instant GE
        /// </summary>
        public virtual void PostGameplayEffectExecute(string pAttrName, float pNewValue)
        {
        }

        /// <summary>
        /// 注册属性变更
        /// </summary>
        /// <param name="pAttrName"></param>
        /// <param name="pChangeDel"></param>
        public void RegAttrChange(string pAttrName, OnAttributeChange pChangeDel)
        {
            if (!attrDict.ContainsKey(pAttrName))
            {
                attrDict.Add(pAttrName,null);
            }
            attrChangeDelDict[pAttrName] -= pChangeDel;
            attrChangeDelDict[pAttrName] += pChangeDel;
        }
        
        /// <summary>
        /// 移除属性变更
        /// </summary>
        /// <param name="pAttrName"></param>
        /// <param name="pChangeDel"></param>
        public void RemoveAttrChange(string pAttrName, OnAttributeChange pChangeDel)
        {
            if (!attrDict.ContainsKey(pAttrName))
            {
                return;
            }
            attrChangeDelDict[pAttrName] -= pChangeDel;
        }
    }
}