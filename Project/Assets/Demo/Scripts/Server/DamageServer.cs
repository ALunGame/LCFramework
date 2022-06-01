using LCSkill;
using UnityEngine;

namespace Demo
{
    public class DamageServer : IDamageServer
    {
        public void AddDamage(SkillCom attacker, SkillCom target, DamageModel damage, float angle = 0)
        {
            DamageCom damageCom = LCECS.ECSLocate.ECS.GetWorld().GetCom<DamageCom>();
            damageCom.AddDamageInfo(attacker, target, damage, angle);
        }

        public bool CalcDamage(AddDamageInfo damageInfo)
        {
            return false;
        }
    }
}
