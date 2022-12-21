using LCNode;

namespace LCSkill.AoeGraph
{
    /// <summary>
    /// 生命周期添加Bullet函数
    /// </summary>
    [NodeMenuItem("添加Bullet")]
    public class Aoe_LifeCycleAddBulletNode : Aoe_LifeCycleFuncNode
    {
        public override string Title { get => $"添加Bullet:{addBullet.id}"; set => base.Title = value; }

        [NodeValue("添加Bullet")]
        public AddBulletModel addBullet = new AddBulletModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override AoeLifeCycleFunc CreateFunc()
        {
            AoeLifeCycleAddBulletFunc func = new AoeLifeCycleAddBulletFunc();
            func.addBullet = addBullet;
            return func;
        }
    }

    /// <summary>
    /// Aoe演员进入添加Bullet
    /// </summary>
    [NodeMenuItem("添加Bullet")]
    public class Aoe_ActorEnterAddBulletNode : Aoe_ActorEnterFuncNode
    {
        public override string Title { get => $"添加Bullet:{addBullet.id}"; set => base.Title = value; }

        [NodeValue("添加Bullet")]
        public AddBulletModel addBullet = new AddBulletModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override AoeActorEnter CreateFunc()
        {
            AoeActorEnterAddBulletFunc func = new AoeActorEnterAddBulletFunc();
            func.addBullet = addBullet;
            return func;
        }
    }

    /// <summary>
    /// Aoe演员进入添加Bullet
    /// </summary>
    [NodeMenuItem("添加Bullet")]
    public class Aoe_ActorLeaveAddBulletNode : Aoe_ActorLeaveFuncNode
    {
        public override string Title { get => $"添加Bullet:{addBullet.id}"; set => base.Title = value; }

        [NodeValue("添加Bullet")]
        public AddBulletModel addBullet = new AddBulletModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override AoeActorLeave CreateFunc()
        {
            AoeActorLeaveAddBulletFunc func = new AoeActorLeaveAddBulletFunc();
            func.addBullet = addBullet;
            return func;
        }
    }
}