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

        }
    }
}