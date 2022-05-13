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
        public int damage;
        
        /// <summary>
        /// 伤害类型
        /// </summary>
        public DamageType type = DamageType.Normal;

        /// <summary>
        /// 是否命中
        /// 大于1就是命中
        /// </summary>
        public float hitRate;

        /// <summary>
        /// 是否暴击
        /// 大于1就是暴击
        /// </summary>
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
        public List<DamageInfo> damages;

        /// <summary>
        /// 伤害接受添加的Buff
        /// </summary>
        public List<AddBuffModel> addBuffs;
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
    }

    /// <summary>
    /// 游戏中伤害值
    /// </summary>
    public struct Damage
    {
        /// <summary>
        /// 普通伤害（治疗）
        /// </summary>
        public int normal;

        public Damage(int normal)
        {
            this.normal = normal;
        }
    
        /// <summary>
        /// 统计伤害
        /// </summary>
        /// <param name="asHeal">是否当做治疗来统计</param>
        /// <returns></returns>
        public int Overall(bool asHeal = false)
        {
            return (asHeal == false) ?
                (Mathf.Max(0, normal)):
                (Mathf.Min(0, normal));
        }

        public static Damage operator +(Damage a, Damage b)
        {
            return new Damage(
                a.normal + b.normal
            );
        }

        public static Damage operator *(Damage a, float b)
        {
            return new Damage(
                Mathf.RoundToInt(a.normal * b)
            );
        }
    }
}
