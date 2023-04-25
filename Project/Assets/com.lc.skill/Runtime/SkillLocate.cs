using LCToolkit.Server;
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
    }

    public class SkillLogServer : BaseLogServer
    {
        public override string LogTag => "Skill";
    }
}
