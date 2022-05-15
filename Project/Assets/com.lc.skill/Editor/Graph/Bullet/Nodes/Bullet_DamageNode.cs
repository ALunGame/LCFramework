using LCNode;
using System.Collections.Generic;

namespace LCSkill.BulletGraph
{
    /// <summary>
    /// 击中伤害函数
    /// </summary>
    [NodeMenuItem("伤害")]
    public class Bullet_HitDamageNode : Bullet_HitFuncNode
    {
        public override string Title { get => "造成伤害"; set => base.Title = value; }

        [NodeValue("伤害")]
        public DamageModel damage = new DamageModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
            if (damage.damages == null)
            {
                damage.damages = new List<DamageInfo>();
            }
            if (damage.addBuffs == null)
            {
                damage.addBuffs = new List<AddBuffModel>();
            }
        }

        public override BulletHitFunc CreateFunc()
        {
            BulletHitDamageFunc func = new BulletHitDamageFunc();
            func.damage = damage;
            return func;
        }
    }
}