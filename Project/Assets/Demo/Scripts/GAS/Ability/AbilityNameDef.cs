using LCSkill;

namespace LCGAS
{
    public static class AbilityNameDef
    {
        public static string Skill;

        static AbilityNameDef()
        {
            Skill = typeof(SkillAbility).FullName;
        }
    }
}