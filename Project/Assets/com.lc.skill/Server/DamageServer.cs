using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCSkill
{
    /// <summary>
    /// 伤害服务
    /// 业务层接口
    /// </summary>
    public class DamageServer
    {
        //临时
        //TODO
        private DamageCom damageCom = new DamageCom();

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
            damageCom.AddDamageInfo(attacker, target, damage, angle, criticalRate, tags);
        }
    }
}
