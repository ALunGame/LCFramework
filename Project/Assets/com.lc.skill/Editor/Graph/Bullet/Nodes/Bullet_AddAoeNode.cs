using LCNode;

namespace LCSkill.BulletGraph
{
    /// <summary>
    /// 击中添加Aoe
    /// </summary>
    [NodeMenuItem("添加Aoe")]
    public class Bullet_HitAddAoeNode : Bullet_HitFuncNode
    {
        public override string Title { get => $"添加Aoe:{addAoe.id}"; set => base.Title = value; }

        [NodeValue("添加Aoe")]
        public AddAoeModel addAoe = new AddAoeModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override BulletHitFunc CreateFunc()
        {
            BulletHitAddAoeFunc func = new BulletHitAddAoeFunc();
            func.addAoe = addAoe;
            return func;
        }
    }
}