namespace LCSkill
{
    public interface ISkillServer
    {
        /// <summary>
        /// 学习一个技能
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="skillModel">技能配置</param>
        /// <param name="level">技能等级</param>
        void LearnSkill(SkillCom target, SkillModel skillModel, int level = 1);

        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="skillId">技能Id</param>
        /// <returns>释放是否成功</returns>
        bool ReleaseSkill(SkillCom target, int skillId);
    }
}
