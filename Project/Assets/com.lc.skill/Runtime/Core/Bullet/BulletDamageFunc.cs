using LCMap;
using System.Collections;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// Bullet击中造成伤害
    /// </summary>
    public class BulletHitDamageFunc : BulletHitFunc
    {
        public DamageModel damage;

        public override void Execute(BulletObj bullet, ActorObj actor)
        {
            SkillCom targetCom = LCECS.ECSLocate.ECS.GetEntity(actor.Uid).GetCom<SkillCom>();
            SkillLocate.Damage.AddDamage(bullet.ower, targetCom, damage);
        }
    }
}