using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LCSkill
{
    public enum DamageInfoTag
    {
        directDamage = 0,   //直接伤害
        periodDamage = 1,   //间歇性伤害
        reflectDamage = 2,  //反噬伤害
        directHeal = 10,    //直接治疗
        periodHeal = 11,    //间歇性治疗
        monkeyDamage = 9999    //这个类型的伤害在目前这个demo中没有意义，只是告诉你可以随意扩展，仅仅比string严肃些。
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
        /// 伤害标签
        /// </summary>
        public DamageInfoTag[] tags;

        /// <summary>
        /// 伤害值，无论是伤害还是治疗都是这个
        /// </summary>
        public Damage damage;

        /// <summary>
        /// 是否暴击
        /// 这是一系列Buff计算后得到的总暴击率，大于1就代表这次暴击了
        /// </summary>
        public float criticalRate;

        /// <summary>
        /// 是否命中
        /// 这是一系列Buff计算后得到的总命中率，大于1就代表这次命中了，暴击和命中不是一回事
        /// </summary>
        public float hitRate;

        /// <summary>
        /// 伤害的角度，作为伤害打向角色的入射角度
        /// </summary>
        public float angle;

        /// <summary>
        /// 伤害接受添加的Buff
        /// </summary>
        public List<AddBuffInfo> addBuffs = new List<AddBuffInfo>();

        /// <summary>
        /// 最终伤害
        /// </summary>
        /// <param name="asHeal">是不是治疗</param>
        /// <returns></returns>
        public int DamageValue(bool asHeal)
        {
            return DamageCalcFunc.DamageValue(this, asHeal);
        }

        /// <summary>
        /// 根据tag判断那些是治疗
        /// </summary>
        /// <returns></returns>
        public bool IsHeal()
        {
            for (int i = 0; i < this.tags.Length; i++)
            {
                if (tags[i] == DamageInfoTag.directHeal || tags[i] == DamageInfoTag.periodHeal)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 添加Buff信息
        /// </summary>
        /// <param name="addBuffInfo"></param>
        public void AddBuff(AddBuffInfo addBuffInfo)
        {
            this.addBuffs.Add(addBuffInfo);
        }
        public AddDamageInfo(SkillCom attacker, SkillCom target, Damage damage, float angle, float criticalRate, DamageInfoTag[] tags)
        {
            this.attacker = attacker;
            this.target = target;
            this.damage = damage;
            this.criticalRate = criticalRate;
            this.angle = angle;
            this.tags = new DamageInfoTag[tags.Length];
            for (int i = 0; i < tags.Length; i++)
            {
                this.tags[i] = tags[i];
            }
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
