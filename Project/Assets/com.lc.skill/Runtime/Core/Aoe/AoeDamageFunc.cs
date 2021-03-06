using LCMap;
using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// Aoe生命周期造成伤害
    /// </summary>
    public class AoeLifeCycleDamageFunc : AoeLifeCycleFunc
    {
        public DamageModel damage;

        public override void Execute(AoeObj aoe)
        {
            SkillLocate.Damage.AddDamage(aoe.ower, aoe.ower, damage);
        }
    }

    /// <summary>
    /// Aoe演员进入造成伤害
    /// </summary>
    public class AoeActorEnterDamageFunc : AoeActorEnter
    {
        public DamageModel damage;

        public override void Execute(AoeObj aoe, List<ActorObj> actors)
        {
            for (int i = 0; i < actors.Count; i++)
            {
                SkillCom targetCom = LCECS.ECSLocate.ECS.GetEntity(actors[i].Uid).GetCom<SkillCom>();
                SkillLocate.Damage.AddDamage(aoe.ower, targetCom, damage);
            }
        }
    }

    /// <summary>
    /// Aoe演员离开造成伤害
    /// </summary>
    public class AoeActorLeaveDamageFunc : AoeActorLeave
    {
        public DamageModel damage;

        public override void Execute(AoeObj aoe, List<ActorObj> actors)
        {
            for (int i = 0; i < actors.Count; i++)
            {
                SkillCom targetCom = LCECS.ECSLocate.ECS.GetEntity(actors[i].Uid).GetCom<SkillCom>();
                SkillLocate.Damage.AddDamage(aoe.ower, targetCom, damage);
            }
        }
    }
}