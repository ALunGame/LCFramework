using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCSkill
{
    public static class LCSkillLocate
    {
        public const string SkillJsonPath = "./Assets/Resources/Config/SkillJson.txt";
        public static bool ReLoadData = false;

        private static LCSkillServer skillServer;

        public static void Init()
        {
            skillServer = new LCSkillServer();
        }

        public static SkillJson GetSkillInfo(int skillId)
        {
            return skillServer.GetSkillInfo(skillId);
        }
    }
}
