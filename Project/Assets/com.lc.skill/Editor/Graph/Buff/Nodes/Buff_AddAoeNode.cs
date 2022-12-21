using LCNode;

namespace LCSkill.BuffGraph
{
    /// <summary>
    /// 生命周期添加Aoe
    /// </summary>
    [NodeMenuItem("添加Aoe")]
    public class Buff_LifeCycleAddAoeNode : Buff_LifeCycleFuncNode
    {
        public override string Title { get => $"添加Aoe:{addAoe.id}"; set => base.Title = value; }

        [NodeValue("添加Aoe")]
        public AddAoeModel addAoe = new AddAoeModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override BuffLifeCycleFunc CreateFunc()
        {
            BuffLifeCycleAddAoeFunc func = new BuffLifeCycleAddAoeFunc();
            func.addAoe = addAoe;
            return func;
        }
    }
}