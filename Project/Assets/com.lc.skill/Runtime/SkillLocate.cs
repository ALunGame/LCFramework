using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 技能服务定位器
    /// </summary>
    public static class SkillLocate
    {
        public static SkillLogServer Log = new SkillLogServer();

        public static float DeltaTime = Time.fixedDeltaTime;

        public static ISkillServer Skill;    

        public static IDamageServer Damage;    
    }
}
