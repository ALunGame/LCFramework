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
        void LearnSkill(SkillCom target, string skillId, int level = 1);

        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="skillId">技能Id</param>
        /// <returns>释放是否成功</returns>
        bool ReleaseSkill(SkillCom target, string skillId);

        /// <summary>
        /// 创建一个Aoe
        /// </summary>
        /// <param name="ower">Aoe拥有者</param>
        /// <param name="addAoe"></param>
        void CreateAoe(SkillCom ower, AddAoeModel addAoe);

        /// <summary>
        /// 创建一个Buff
        /// </summary>
        /// <param name="ower">Buff拥有者</param>
        /// <param name="target">Buff目标</param>
        /// <param name="addBuff"></param>
        /// <returns></returns>
        void CreateBuff(SkillCom ower, SkillCom target, AddBuffModel addBuff);

        /// <summary>
        /// 创建一个Bullet
        /// </summary>
        /// <param name="ower">Bullet拥有者</param>
        /// <returns></returns>
        void CreateBullet(SkillCom ower, AddBulletModel addBullet);
    }
}
