using LCECS.Core;
using System;
using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// 伤害组件
    /// 存储游戏中所有的伤害信息
    /// </summary>
    [Serializable]
    public class DamageCom : BaseCom
    {
        [NonSerialized]
        private List<AddDamageInfo> damages = new List<AddDamageInfo>();

        public IReadOnlyList<AddDamageInfo> Damages { get => damages; }

        /// <summary>
        /// 添加一条伤害信息
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <param name="target">目标</param>
        /// <param name="damage">伤害数据</param>
        /// <param name="angle">伤害角度</param>
        public void AddDamageInfo(SkillCom attacker, SkillCom target, DamageModel damage, float angle)
        {
            damages.Add(new AddDamageInfo(attacker,target,damage,angle));
        }

        /// <summary>
        /// 删除一条伤害信息
        /// </summary>
        /// <param name="damageInfo"></param>
        public void RemoveDamageInfo(AddDamageInfo damageInfo)
        {
            for (int i = 0; i < damages.Count; i++)
            {
                if (damages[i].Equals(damageInfo))
                {
                    damages.RemoveAt(i);
                }
            }
        }
    }
}
