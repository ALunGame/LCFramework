using LCNode;

namespace LCSkill.BulletGraph
{
    /// <summary>
    /// 击中添加Bullet
    /// </summary>
    [NodeMenuItem("添加Bullet")]
    public class Bullet_HitAddBulletNode : Bullet_HitFuncNode
    {
        public override string Title { get => $"添加Bullet:{addBullet.id}"; set => base.Title = value; }

        [NodeValue("添加Bullet")]
        public AddBulletModel addBullet = new AddBulletModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override BulletHitFunc CreateFunc()
        {
            BulletHitAddBulletFunc func = new BulletHitAddBulletFunc();
            func.addBullet = addBullet;
            return func;
        }
    }
}