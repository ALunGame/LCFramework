namespace LCSkill
{
    /// <summary>
    /// 技能服务定位器
    /// </summary>
    public static class SkillLocate
    {
        //临时
        public static SkillLogServer Log = new SkillLogServer();

        public static SkillServer Skill = new SkillServer();    

        public static DamageServer Damage = new DamageServer();    
    }
}
