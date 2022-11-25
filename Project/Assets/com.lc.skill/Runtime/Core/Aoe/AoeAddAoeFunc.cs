using LCMap;
using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// Aoe生命周期创建Aoe
    /// </summary>
    public class AoeLifeCycleAddAoeFunc : AoeLifeCycleFunc
    {
        public AddAoeModel addAoe;

        public override void Execute(AoeObj aoe)
        {
        }
    }

    /// <summary>
    /// Aoe演员进入创建Aoe
    /// </summary>
    public class AoeActorEnterAddAoeFunc : AoeActorEnter
    {
        public AddAoeModel addAoe;

        public override void Execute(AoeObj aoe, List<Actor> actors)
        {

        }
    }

    /// <summary>
    /// Aoe演员离开创建Aoe
    /// </summary>
    public class AoeActorLeaveAddAoeFunc : AoeActorLeave
    {
        public AddAoeModel addAoe;

        public override void Execute(AoeObj aoe, List<Actor> actors)
        {

        }
    }
}