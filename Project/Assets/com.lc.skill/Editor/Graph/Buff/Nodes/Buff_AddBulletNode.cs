using LCNode;

namespace LCSkill.BuffGraph
{
    /// <summary>
    /// 生命周期添加Bullet
    /// </summary>
    [NodeMenuItem("添加Bullet")]
    public class Buff_LifeCycleAddBulletNode : Buff_LifeCycleFuncNode
    {
        public override string Title { get => $"添加Bullet:{addBullet.id}"; set => base.Title = value; }

        [NodeValue("添加Bullet")]
        public AddBulletModel addBullet = new AddBulletModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override BuffLifeCycleFunc CreateFunc()
        {
            BuffLifeCycleAddBulletFunc func = new BuffLifeCycleAddBulletFunc();
            func.addBullet = addBullet;
            return func;
        }
    }
}