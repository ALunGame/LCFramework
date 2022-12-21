using LCNode;

namespace LCSkill.AoeGraph
{
    /// <summary>
    /// 生命周期添加Aoe函数
    /// </summary>
    [NodeMenuItem("添加Aoe")]
    public class Aoe_LifeCycleAddAoeNode : Aoe_LifeCycleFuncNode
    {
        public override string Title { get => $"添加Aoe:{addAoe.id}"; set => base.Title = value; }

        [NodeValue("添加Aoe")]
        public AddAoeModel addAoe = new AddAoeModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override AoeLifeCycleFunc CreateFunc()
        {
            AoeLifeCycleAddAoeFunc func = new AoeLifeCycleAddAoeFunc();
            func.addAoe = addAoe;
            return func;
        }
    }

    /// <summary>
    /// Aoe演员进入添加Aoe
    /// </summary>
    [NodeMenuItem("添加Aoe")]
    public class Aoe_ActorEnterAddAoeNode : Aoe_ActorEnterFuncNode
    {
        public override string Title { get => $"添加Aoe:{addAoe.id}"; set => base.Title = value; }

        [NodeValue("添加Aoe")]
        public AddAoeModel addAoe = new AddAoeModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override AoeActorEnter CreateFunc()
        {
            AoeActorEnterAddAoeFunc func = new AoeActorEnterAddAoeFunc();
            func.addAoe = addAoe;
            return func;
        }
    }

    /// <summary>
    /// Aoe演员进入添加Aoe
    /// </summary>
    [NodeMenuItem("添加Aoe")]
    public class Aoe_ActorLeaveAddAoeNode : Aoe_ActorLeaveFuncNode
    {
        public override string Title { get => $"添加Aoe:{addAoe.id}"; set => base.Title = value; }

        [NodeValue("添加Aoe")]
        public AddAoeModel addAoe = new AddAoeModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override AoeActorLeave CreateFunc()
        {
            AoeActorLeaveAddAoeFunc func = new AoeActorLeaveAddAoeFunc();
            func.addAoe = addAoe;
            return func;
        }
    }
}