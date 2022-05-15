using LCNode;
using LCSkill.BuffGraph;
using System.Collections.Generic;

namespace LCSkill.BuffGraph
{
    /// <summary>
    /// 生命周期伤害函数
    /// </summary>
    [NodeMenuItem("伤害")]
    public class Buff_LifeCycleDamageNode : Buff_LifeCycleFuncNode
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

        public override BuffLifeCycleFunc CreateFunc()
        {
            BuffLifeCycleDamageFunc func = new BuffLifeCycleDamageFunc();
            func.damage = damage;
            return func;
        }
    }
}