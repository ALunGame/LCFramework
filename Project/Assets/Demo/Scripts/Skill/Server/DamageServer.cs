using LCSkill;

namespace Demo
{
    public class DamageServer : IDamageServer
    {
        public void AddDamage(SkillCom attacker, SkillCom target, DamageModel damage, float angle = 0)
        {
            
        }

        public bool CalcDamage(AddDamageInfo damageInfo)
        {
            return false;
        }
    }
}
