using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 伤害计算函数
    /// 业务层写入伤害计算规则
    /// </summary>
    public static class DamageCalcFunc
    {
        public static int DamageValue(AddDamageInfo damageInfo, bool asHeal = false)
        {
            return Mathf.CeilToInt(damageInfo.damage.Overall());
        }

        /// <summary>
        /// 执行伤害临时，业务层处理
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <param name="damageInfo">伤害</param>
        /// <param name="target">目标</param>
        /// <returns>返回是否死亡</returns>
        public static bool ExecuteDamage(SkillCom attacker, AddDamageInfo damageInfo, SkillCom target)
        {
            return false;
        }
    }
}
