using LCMap;
using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// Aoe生命周期造成伤害
    /// </summary>
    public class AoeLifeCycleAddBuffFunc : AoeLifeCycleFunc
    {
        public AddBuffModel addBuff;

        public override void Execute(AoeObj aoe)
        {
        }
    }

    /// <summary>
    /// Aoe演员进入造成伤害
    /// </summary>
    public class AoeActorEnterAddBuffFunc : AoeActorEnter
    {
        public AddBuffModel addBuff;

        public override void Execute(AoeObj aoe, List<ActorObj> actors)
        {

        }
    }

    /// <summary>
    /// Aoe演员离开造成伤害
    /// </summary>
    public class AoeActorLeaveAddBuffFunc : AoeActorLeave
    {
        public AddBuffModel addBuff;

        public override void Execute(AoeObj aoe, List<ActorObj> actors)
        {

        }
    }
}