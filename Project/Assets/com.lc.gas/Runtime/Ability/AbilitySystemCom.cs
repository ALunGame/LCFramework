using System;
using System.Collections.Generic;
using LCECS.Core;
using LCToolkit;
using UnityEngine;

namespace LCGAS
{
    /// <summary>
    /// 能力系统组件
    /// 1，使用技能 GameplayAbility
    /// 2，声明属性 AttributeSet
    /// 3，处理效果 GameplayEffect
    /// 4，
    /// </summary>
    public class AbilitySystemCom : BaseCom
    {
        /// <summary>
        /// 自身标签
        /// </summary>
        public GameplayTagContainer Tag { get; private set; }
        
        /// <summary>
        /// 属性
        /// </summary>
        public AttributeSetBase Attr { get; private set; }
        
        /// <summary>
        /// 效果列表
        /// </summary>
        public Dictionary<string,GameplayAbilitySpec> AbilityDict { get; private set; }
        
        /// <summary>
        /// 效果列表
        /// </summary>
        public Dictionary<string,List<GameplayEffectSpec>> EffectDict { get; private set; }

        [NonSerialized]
        private Action OnTagChange;

        protected override void OnAwake(Entity pEntity)
        {
            Tag = new GameplayTagContainer();
            Attr = new AttributeSetBase();
            AbilityDict = new Dictionary<string, GameplayAbilitySpec>();
            EffectDict = new Dictionary<string, List<GameplayEffectSpec>>();
        }

        protected override void OnDestroy()
        {
            OnTagChange = null;
        }

        #region Tag

        public void AddTag(GameplayTagContainer pTagContainer)
        {
            Tag.Add(pTagContainer);
            OnTagChange?.Invoke();
        }

        public void RemoveTag(GameplayTagContainer pTagContainer)
        {
            Tag.Remove(pTagContainer);
            OnTagChange?.Invoke();
        }

        public void RegTagChangeEvent(Action pTagChangeEvent)
        {
            OnTagChange += pTagChangeEvent;
        }

        public void RemoveTagChangeEvent(Action pTagChangeEvent)
        {
            OnTagChange -= pTagChangeEvent;
        }

        #endregion

        #region Attribute

        public float GetValue(string pAttrName, AttributeValueType pValueType = AttributeValueType.Base)
        {
            if (!Attr.HasAttr(pAttrName))
            {
                LCGAS.GASLocate.Log.LogError("获得属性值出错,没有对应属性",pAttrName);
                return 0;
            }

            if (pValueType == AttributeValueType.Base)
            {
                return Attr.GetBaseValue(pAttrName);
            }
            else if (pValueType == AttributeValueType.Curr)
            {
                return Attr.GetCurrValue(pAttrName);
            }
            else if (pValueType == AttributeValueType.Bonus)
            {
                return Attr.GetCurrValue(pAttrName) - Attr.GetBaseValue(pAttrName);;
            }
            return 0;
        }

        public bool SetValue(string pAttrName, float pValue, AttributeValueType pValueType = AttributeValueType.Base)
        {
            if (!Attr.HasAttr(pAttrName))
            {
                LCGAS.GASLocate.Log.LogError("设置属性值出错,没有对应属性",pAttrName);
                return false;
            }

            if (pValueType == AttributeValueType.Base)
            {
                return Attr.SetBaseValue(pAttrName,pValue);
            }
            else if (pValueType == AttributeValueType.Curr)
            {
                return Attr.SetCurrValue(pAttrName,pValue);
            }
            else if (pValueType == AttributeValueType.Bonus)
            {
                LCGAS.GASLocate.Log.LogError("设置属性值出错,不可以设置Bonus类型",pAttrName);
                return false;
            }
            return false;
        }

        #endregion

        #region GA

        /// <summary>
        /// 获得正在激活的GA
        /// </summary>
        /// <returns></returns>
        public List<GameplayAbilitySpec> GetActiveGameplayAbilitys()
        {
            List<GameplayAbilitySpec> activeAbilitys = new List<GameplayAbilitySpec>();

            foreach (GameplayAbilitySpec gaSpec in AbilityDict.Values)
            {
                if (gaSpec.IsActive)
                {
                    activeAbilitys.Add(gaSpec);
                }
            }

            return activeAbilitys;
        }

        /// <summary>
        /// 添加GA
        /// </summary>
        /// <param name="pAbility"></param>
        /// <returns></returns>
        public bool AddGameplayAbility(GameplayAbility pAbility)
        {
            string fullName = pAbility.GetType().FullName;
            if (!AbilityDict.ContainsKey(fullName))
            {
                LCGAS.GASLocate.Log.LogError("添加GA失败，重复的GA",fullName);
                return false;
            }

            GameplayAbilitySpec spec = pAbility.CreateSpec(this);
            AbilityDict.Add(fullName,spec);
            return true;
        }

        /// <summary>
        /// 尝试激活GA
        /// </summary>
        /// <returns></returns>
        public bool TryActiveGameplayAbility(string pAbilityName,params object[] pParams)
        {
            if (!AbilityDict.ContainsKey(pAbilityName))
            {
                LCGAS.GASLocate.Log.LogError("尝试激活GA失败，没有该GA",pAbilityName);
                return false;
            }

            GameplayAbilitySpec spec = AbilityDict[pAbilityName];
            if (spec.TryActive(pParams))
            {
                //打断正在激活的GA
                List<GameplayAbilitySpec> activeAbilitys = GetActiveGameplayAbilitys();
                foreach (GameplayAbilitySpec activeAbility in activeAbilitys)
                {
                    if (activeAbility.Model.tags.HasAny(spec.Model.cancelTags))
                    {
                        EndActiveGameplayAbility(activeAbility);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 通过标签尝试激活GA
        /// </summary>
        /// <param name="pTags"></param>
        /// <param name="pParams"></param>
        /// <returns></returns>
        public bool TryActiveGameplayAbilityByTag(GameplayTagContainer pTags,params object[] pParams)
        {
            bool success = false;
            foreach (string attrName in AbilityDict.Keys)
            {
                GameplayAbilitySpec spec = AbilityDict[attrName];
                if (spec.Model.tags.HasAnyExact(pTags))
                {
                    if (TryActiveGameplayAbility(attrName))
                    {
                        success = true;
                    }
                }
            }

            return success;
        }
        
        /// <summary>
        /// 停止激活GA
        /// </summary>
        /// <param name="pSpec"></param>
        public void EndActiveGameplayAbility(GameplayAbilitySpec pSpec)
        {
            if (pSpec.IsNull())
            {
                return;
            }
            pSpec.EndActive();
        }
        
        /// <summary>
        /// 删除GA
        /// </summary>
        /// <param name="pAbilitySpec"></param>
        /// <returns></returns>
        public bool RemoveGameplayAbility(GameplayAbilitySpec pAbilitySpec)
        {
            string fullName = pAbilitySpec.Model.GetType().FullName;
            if (!AbilityDict.ContainsKey(fullName))
            {
                LCGAS.GASLocate.Log.LogError("删除GA失败，没有该GA",fullName);
                return false;
            }

            AbilityDict.Remove(fullName);
            return true;
        }


        
        #endregion
        
        #region GE

        private void AddEffect(GameplayEffectSpec pEffectSpec)
        {
            GameplayEffect model = pEffectSpec.Model;
            if (!EffectDict.ContainsKey(model.name))
                EffectDict.Add(model.name,new List<GameplayEffectSpec>());
            if (EffectDict[model.name].Contains(pEffectSpec))
            {
                LCGAS.GASLocate.Log.LogError("AddEffect出错，重复的GE",model.name);
                return;
            }
            EffectDict[model.name].Add(pEffectSpec);
        }
        
        /// <summary>
        /// 添加GE
        /// 1,检测堆叠问题
        /// </summary>
        /// <param name="pGameplayEffectSpec"></param>
        private void AddGameplayEffectSpec(GameplayEffectSpec pGameplayEffectSpec)
        {
            GameplayEffect model = pGameplayEffectSpec.Model;
            //没有堆叠策略,没有上限
            if (model.stack == null)
            {
                AddEffect(pGameplayEffectSpec);
            }
            else
            {
                int currCnt = GetGameplayEffectCnt(model.name);
                //溢出策略
                if (currCnt >= model.stack.limitCnt)
                {
                    if (model.overflow != null)
                    {
                        //默认刷新持续时间
                        if (!model.overflow.denyOverflowActive)
                        {
                            List<GameplayEffectSpec> currSpecs = EffectDict[model.name];
                            foreach (GameplayEffectSpec currSpec in currSpecs)
                            {
                                ((GameplayEffectHasDurationSpec)currSpec).RefreshDuration();
                            }
                        }

                        //清空所有
                        if (model.overflow.clearStackOnOverflow)
                        {
                            List<GameplayEffectSpec> currSpecs = EffectDict[model.name];
                            for (int i = currSpecs.Count -1; i >= 0; i--)
                            {
                                StopGameplayEffect(currSpecs[i], false);
                            }
                        }
                        
                        //添加新的GE
                        if (model.overflow.addEffectNames.IsLegal())
                        {
                            for (int i = 0; i < model.overflow.addEffectNames.Count; i++)
                            {
                                AddGameplayEffectByName(pGameplayEffectSpec.SourceCom,
                                    model.overflow.addEffectNames[i]);
                            }
                        }
                    }
                }
                else
                {
                    //数据
                    AddEffect(pGameplayEffectSpec);
                    
                    //刷新
                    if (model.stack.addRefreshDuration)
                    {
                        List<GameplayEffectSpec> currSpecs = EffectDict[model.name];
                        foreach (GameplayEffectSpec currSpec in currSpecs)
                        {
                            ((GameplayEffectHasDurationSpec)currSpec).RefreshDuration();
                        }
                    }
                    
                    //TODO:发送GE层数改变的消息
                }
            }
        }
        
        private void RemoveEffect(GameplayEffectSpec pEffectSpec)
        {
            GameplayEffect model = pEffectSpec.Model;
            if (!EffectDict.ContainsKey(model.name))
                return;
            if (!EffectDict[model.name].Contains(pEffectSpec))
            {
                LCGAS.GASLocate.Log.LogError("RemoveEffect出错，没有该GE",model.name);
                return;
            }
            EffectDict[model.name].Remove(pEffectSpec);
        }
        
        private void RemoveGameplayEffectSpec(GameplayEffectSpec pGameplayEffectSpec)
        {
            GameplayEffect model = pGameplayEffectSpec.Model;
            RemoveEffect(pGameplayEffectSpec);
            //TODO:发送GE层数改变的消息
        }
        

        // public GameplayEffectSpec GetGameplayEffectSpec()
        // {
        //     
        // }




        //获得添加的目标
        private AbilitySystemCom GetAddEffectCom(GameplayEffect pEffect,AbilitySystemCom pSourceCom)
        {
            //没有堆叠策略,没有上限
            if (pEffect.stack == null)
            {
                return this;
            }

            return pEffect.stack.type == TargetType.Source ? pSourceCom : this;
        }

        /// <summary>
        /// 检查是否可以添加这个GE
        /// </summary>
        /// <param name="pEffect"></param>
        /// <returns></returns>
        public virtual bool CheckCanAddGameplayEffect(GameplayEffect pEffect)
        {
            if (!GameplayEffectSpec.CheckAddGameplayEffect(this,pEffect))
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// 通过GE名添加一个GE
        /// </summary>
        /// <param name="pSourceCom"></param>
        /// <param name="pEffectName"></param>
        /// <returns></returns>
        public bool AddGameplayEffectByName(AbilitySystemCom pSourceCom,string pEffectName)
        {
            //TODO:添加通过名字查找GE的方法
            return AddGameplayEffect(null, pSourceCom);
        }
        
        /// <summary>
        /// 添加一个GE
        /// </summary>
        /// <param name="pSourceCom">GE来源</param>
        /// <returns></returns>
        public bool AddGameplayEffect(GameplayEffectName pEffectName, AbilitySystemCom pSourceCom)
        {
            GameplayEffect effect = pEffectName.GetModel();
            if (!CheckCanAddGameplayEffect(effect))
            {
                return false;
            }

            AbilitySystemCom ownerCom = GetAddEffectCom(effect, pSourceCom) ?? this;
            GameplayEffectSpec effectSpec = pEffectName.CreateSpec(pSourceCom,this,ownerCom);
            effectSpec.Start();
            return true;
        }

        /// <summary>
        /// 删除一个GE
        /// </summary>
        /// <returns></returns>
        public void RemoveGameplayEffect(GameplayEffectName pEffectName)
        {
            List<GameplayEffectSpec> ges = GetGameplayEffects(pEffectName.Name);
            if (ges.IsLegal())
            {
                foreach (GameplayEffectSpec ge in ges)
                {
                    StopGameplayEffect(ge, false);
                }
            }
        }

        /// <summary>
        /// 停止一个GE
        /// </summary>
        /// <param name="pEffectSpec"></param>
        /// <param name="pNormalStop">正常停止，持续时间结束</param>
        /// <returns></returns>
        public bool StopGameplayEffect(GameplayEffectSpec pEffectSpec, bool pNormalStop)
        {
            //立即改变直接停止这玩意不存
            if (pEffectSpec.Model.type == GameplayEffectType.Instand)
            {
                pEffectSpec.Stop(true);
                return true;
            }
            
            //永久改变都应该是打断
            if (pEffectSpec.Model.type == GameplayEffectType.Infinite)
            {
                //删除数据
                if (!EffectDict.ContainsKey(pEffectSpec.Model.name))
                {
                    LCGAS.GASLocate.Log.LogError("停止一个永久GE出错，没有对应数据",pEffectSpec.Model.name);
                    return false;
                }

                RemoveGameplayEffectSpec(pEffectSpec);
                pEffectSpec.Stop(false);
                return true;
            }

            
            //持续
            if (pEffectSpec.Model.type == GameplayEffectType.HasDuration)
            {
                //删除数据
                if (!EffectDict.ContainsKey(pEffectSpec.Model.name))
                {
                    LCGAS.GASLocate.Log.LogError("停止一个持续GE出错，没有对应数据",pEffectSpec.Model.name);
                    return false;
                }
                RemoveGameplayEffectSpec(pEffectSpec);
                pEffectSpec.Stop(pNormalStop);
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// 获得堆叠数量
        /// </summary>
        /// <param name="pEffectName"></param>
        /// <returns></returns>
        public int GetGameplayEffectCnt(string pEffectName)
        {
            if (!EffectDict.ContainsKey(pEffectName))
                return 0;
            return EffectDict[pEffectName].Count;
        }

        /// <summary>
        /// 获得GE
        /// </summary>
        /// <param name="pEffectName"></param>
        /// <returns></returns>
        public List<GameplayEffectSpec> GetGameplayEffects(string pEffectName)
        {
            if (!EffectDict.ContainsKey(pEffectName))
                return null;
            return EffectDict[pEffectName];
        }

        #endregion

        #region GameplayEvent

        

        #endregion
    }
}