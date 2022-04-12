using LCECS.Core;
using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// 伤害组件
    /// 存储游戏中所有的伤害信息
    /// </summary>
    [Com(GroupName = "技能相关", ViewName = "伤害组件")]
    public class DamageCom : BaseCom
    {
        private List<AddDamageInfo> damages = new List<AddDamageInfo>();

        public IReadOnlyList<AddDamageInfo> Damages { get => damages; }

        /// <summary>
        /// 添加一条伤害信息
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <param name="target">目标</param>
        /// <param name="damage">伤害数据</param>
        /// <param name="angle">伤害角度</param>
        /// <param name="criticalRate">暴击几率</param>
        /// <param name="tags">伤害标签</param>
        public void AddDamageInfo(SkillCom attacker, SkillCom target, Damage damage, float angle, float criticalRate, DamageInfoTag[] tags)
        {
            damages.Add(new AddDamageInfo(attacker,target,damage,angle,criticalRate,tags));
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
