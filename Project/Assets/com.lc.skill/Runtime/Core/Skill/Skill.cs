using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 条件关系
    /// </summary>
    public enum ConditionRelateType
    {
        AND,
        OR,
    }

    /// <summary>
    /// 技能条件
    /// </summary>
    public abstract class SkillCondition
    {
        private ConditionRelateType _type;
        private bool checkValue = true;
        public SkillCondition nextCondition;

        public SkillCondition()
        {

        }

        public SkillCondition(bool checkValue, ConditionRelateType nextRelate)
        {
            this.checkValue = checkValue;
            this._type = nextRelate;
        }

        /// <summary>
        /// 条件是否满足
        /// </summary>
        /// <returns></returns>
        public bool IsTrue()
        {
            bool resValue = OnMakeTrue();
            resValue = checkValue == resValue ? true : false;
            if (nextCondition != null)
            {
                switch (_type)
                {
                    case ConditionRelateType.AND:
                        return resValue && nextCondition.IsTrue();
                    case ConditionRelateType.OR:
                        return resValue || nextCondition.IsTrue();
                }
                return resValue && nextCondition.IsTrue();
            }
            else
            {
                return resValue;
            }
        }

        protected abstract bool OnMakeTrue();
    }

    /// <summary>
    /// 技能消耗
    /// </summary>
    public class SkillCost
    {
        /// <summary>
        /// 消耗类型
        /// </summary>
        public int costType;
        
        /// <summary>
        /// 消耗数量
        /// </summary>
        public int costValue;
    }

    /// <summary>
    /// 配置的Skill数据结构
    /// </summary>
    public struct SkillModel
    {
        /// <summary>
        /// 技能Id
        /// </summary>
        public string id;

        /// <summary>
        /// 技能名
        /// </summary>
        public string name;

        /// <summary>
        /// 技能条件
        /// </summary>
        public SkillCondition condition;

        /// <summary>
        /// 技能消耗
        /// </summary>
        public List<SkillCost> costs;

        /// <summary>
        /// 技能效果
        /// </summary>
        public string timeline;

        /// <summary>
        /// 学会技能时，获得的Buff
        /// </summary>
        public List<AddBuffModel> addBuffs;
    } 

    /// <summary>
    /// 运行时创建的技能对象
    /// </summary>
    public class SkillObj
    {
        public SkillModel model;

        /// <summary>
        /// 技能等级
        /// </summary>
        public int level;

        /// <summary>
        /// 冷却时间
        /// </summary>
        public float coldDown;

        public SkillObj(SkillModel model,int level)
        {
            this.model = model;
            this.level = level;
            this.coldDown = 0;
        }
    }
}
