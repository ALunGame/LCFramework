using LCMap;
using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// Aoe生命周期创建Bullet
    /// </summary>
    public class AoeLifeCycleAddBulletFunc : AoeLifeCycleFunc
    {
        public AddBulletModel addBullet;

        public override void Execute(AoeObj aoe)
        {
        }
    }

    /// <summary>
    /// Aoe演员进入创建Bullet
    /// </summary>
    public class AoeActorEnterAddBulletFunc : AoeActorEnter
    {
        public AddBulletModel addBullet;

        public override void Execute(AoeObj aoe, List<ActorObj> actors)
        {

        }
    }

    /// <summary>
    /// Aoe演员离开创建Bullet
    /// </summary>
    public class AoeActorLeaveAddBulletFunc : AoeActorLeave
    {
        public AddBulletModel addBullet;

        public override void Execute(AoeObj aoe, List<ActorObj> actors)
        {

        }
    }
}