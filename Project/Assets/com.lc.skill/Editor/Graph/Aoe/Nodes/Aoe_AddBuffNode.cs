using LCNode;

namespace LCSkill.AoeGraph
{
    /// <summary>
    /// 生命周期添加Buff函数
    /// </summary>
    [NodeMenuItem("添加Buff")]
    public class Aoe_LifeCycleAddBuffNode : Aoe_LifeCycleFuncNode
    {
        public override string Title { get => $"添加Buff:{addBuff.id}"; set => base.Title = value; }

        [NodeValue("添加Buff")]
        public AddBuffModel addBuff = new AddBuffModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override AoeLifeCycleFunc CreateFunc()
        {
            AoeLifeCycleAddBuffFunc func = new AoeLifeCycleAddBuffFunc();
            func.addBuff = addBuff;
            return func;
        }
    }

    /// <summary>
    /// Aoe演员进入添加Buff
    /// </summary>
    [NodeMenuItem("添加Buff")]
    public class Aoe_ActorEnterAddBuffNode : Aoe_ActorEnterFuncNode
    {
        public override string Title { get => $"添加Buff:{addBuff.id}"; set => base.Title = value; }

        [NodeValue("添加Buff")]
        public AddBuffModel addBuff = new AddBuffModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override AoeActorEnter CreateFunc()
        {
            AoeActorEnterAddBuffFunc func = new AoeActorEnterAddBuffFunc();
            func.addBuff = addBuff;
            return func;
        }
    }

    /// <summary>
    /// Aoe演员进入添加Buff
    /// </summary>
    [NodeMenuItem("添加Buff")]
    public class Aoe_ActorLeaveAddBuffNode : Aoe_ActorLeaveFuncNode
    {
        public override string Title { get => $"添加Buff:{addBuff.id}"; set => base.Title = value; }

        [NodeValue("添加Buff")]
        public AddBuffModel addBuff = new AddBuffModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override AoeActorLeave CreateFunc()
        {
            AoeActorLeaveAddBuffFunc func = new AoeActorLeaveAddBuffFunc();
            func.addBuff = addBuff;
            return func;
        }
    }
}