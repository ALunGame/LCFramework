using LCConfig;
using System.Collections;
using UnityEngine;

namespace LCSkill
{
    public static class SkillDef
    {
        public const string CnfRootPath = "Assets/Demo/Asset/Config/";

        /// <summary>
        /// Skill根目录
        /// </summary>
        public const string SkillRootPath = CnfRootPath + "Skill/";

        public static string GetSkillCnfName(string id)
        {
            return ConfigDef.GetCnfNoExName("Skill_" + id);
        }

        public static string GetSkillCnfPath(string id)
        {
            return SkillRootPath + ConfigDef.GetCnfName("Skill_" + id);
        }

        /// <summary>
        /// Skill根目录
        /// </summary>
        public const string TimelineRootPath = CnfRootPath + "Sk_Timeline/";

        public static string GetTimelineCnfName(string id)
        {
            return ConfigDef.GetCnfNoExName("Sk_Timeline_" + id);
        }

        public static string GetTimelineCnfPath(string id)
        {
            return TimelineRootPath + ConfigDef.GetCnfName("Sk_Timeline_" + id);
        }

        /// <summary>
        /// Buff根目录
        /// </summary>
        public const string BuffRootPath = CnfRootPath + "Buff/";

        public static string GetBuffCnfName(string id)
        {
            return ConfigDef.GetCnfNoExName("Buff_" + id);
        }

        public static string GetBuffCnfPath(string id)
        {
            return SkillRootPath + ConfigDef.GetCnfName("Buff_" + id);
        }


        /// <summary>
        /// Aoe根目录
        /// </summary>
        public const string AoeRootPath = CnfRootPath + "Aoe/";

        public static string GetAoeCnfName(string id)
        {
            return ConfigDef.GetCnfNoExName("Aoe_" + id);
        }

        public static string GetAoeCnfPath(string id)
        {
            return SkillRootPath + ConfigDef.GetCnfName("Aoe_" + id);
        }

        /// <summary>
        /// Bullet根目录
        /// </summary>
        public const string BulletRootPath = CnfRootPath + "Bullet/";

        public static string GetBulletCnfName(string id)
        {
            return ConfigDef.GetCnfNoExName("Bullet_" + id);
        }

        public static string GetBulletCnfPath(string id)
        {
            return SkillRootPath + ConfigDef.GetCnfName("Bullet_" + id);
        }

    }
}