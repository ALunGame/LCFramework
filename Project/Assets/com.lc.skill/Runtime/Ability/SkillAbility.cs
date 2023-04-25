using LCGAS;

namespace LCSkill
{
    /// <summary>
    /// 技能释放能力
    /// </summary>
    public class SkillAbility : GameplayAbility
    {
        public override GameplayAbilitySpec CreateSpec(AbilitySystemCom pOwnerCom)
        {
            SkillAbilitySpec spec = new SkillAbilitySpec(pOwnerCom, this);
            return spec;
        }
    }
}