using System.Collections;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 技能服务器
    /// 实现业务层接口
    /// </summary>
    public class SkillServer
    {
        public float GetSkillDeltaTime()
        {
            return Time.fixedDeltaTime;
        }

        /// <summary>
        /// 学习一个技能
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="skillModel">技能配置</param>
        /// <param name="level">技能等级</param>
        public void LearnSkill(SkillCom target,SkillModel skillModel, int level = 1)
        {
            target.LearnSkill(skillModel, level);
        }

        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="skillId">技能Id</param>
        /// <returns></returns>
        public bool ReleaseSkill(SkillCom target, int skillId)
        {
            return target.ReleaseSkill(skillId);
        }
    }
}