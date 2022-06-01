using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 伤害类型，不同伤害类型计算方式不同
    /// </summary>
    public enum DamageType
    {
        /// <summary>
        /// 普通伤害
        /// </summary>
        Normal,     
    }

    public class DamageInfo
    {
        /// <summary>
        /// 伤害值
        /// </summary>
        [Header("伤害值")]
        public int damage;

        /// <summary>
        /// 伤害类型
        /// </summary>
        [Header("伤害类型")]
        public DamageType type = DamageType.Normal;

        /// <summary>
        /// 是否命中
        /// 大于1就是命中
        /// </summary>
        [Header("是否命中>1")]
        public float hitRate;

        /// <summary>
        /// 是否暴击
        /// 大于1就是暴击
        /// </summary>
        [Header("是否暴击>1")]
        public float criticalRate;
    }

    /// <summary>
    /// 伤害配置
    /// </summary>
    public struct DamageModel
    {
        /// <summary>
        /// 伤害信息
        /// </summary>
        [Header("伤害信息")]
        public List<DamageInfo> damages;

        /// <summary>
        /// 伤害接受添加的Buff
        /// </summary>
        [Header("受到伤害添加的Buff")]
        public List<AddBuffModel> addBuffs;

        public static DamageModel Null 
        { 
            get 
            {   
                DamageModel model = new DamageModel();
                model.addBuffs = new List<AddBuffModel>();
                model.damages = new List<DamageInfo>();
                return model; 
            }
        }
    }

    /// <summary>
    /// 任何伤害的产生都要创建这个对象，治疗只是反向的伤害
    /// </summary>
    public class AddDamageInfo
    {
        /// <summary>
        /// 攻击者
        /// </summary>
        public SkillCom attacker;

        /// <summary>
        /// 目标
        /// </summary>
        public SkillCom target;

        /// <summary>
        /// 伤害值，无论是伤害还是治疗都是这个
        /// </summary>
        public DamageModel model;

        /// <summary>
        /// 伤害的角度，作为伤害打向角色的入射角度
        /// </summary>
        public float angle;

        public AddDamageInfo(SkillCom attacker, SkillCom target, DamageModel model, float angle)
        {
            this.attacker = attacker;
            this.target = target;
            this.model = model;
            this.angle = angle;
        }

        public override string ToString()
        {
            return string.Format("{0}->{1}",attacker.EntityCnfId, target.EntityCnfId);
        }
    }
}
