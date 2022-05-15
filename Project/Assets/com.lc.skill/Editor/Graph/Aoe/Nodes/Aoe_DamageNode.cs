using LCNode;
using System.Collections.Generic;

namespace LCSkill.AoeGraph
{
    /// <summary>
    /// 生命周期伤害函数
    /// </summary>
    [NodeMenuItem("伤害")]
    public class Aoe_LifeCycleDamageNode : Aoe_LifeCycleFuncNode
    {
        public override string Title { get => "造成伤害"; set => base.Title = value; }

        [NodeValue("伤害")]
        public DamageModel damage = new DamageModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
            if (damage.damages==null)
            {
                damage.damages = new List<DamageInfo>();
            }
            if (damage.addBuffs == null)
            {
                damage.addBuffs = new List<AddBuffModel>();
            }
        }

        public override AoeLifeCycleFunc CreateFunc()
        {
            AoeLifeCycleDamageFunc func = new AoeLifeCycleDamageFunc();
            func.damage = damage;
            return func;
        }
    }

    /// <summary>
    /// Aoe演员进入造成伤害
    /// </summary>
    [NodeMenuItem("伤害")]
    public class Aoe_ActorEnterDamageNode : Aoe_ActorEnterFuncNode
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

        public override AoeActorEnter CreateFunc()
        {
            AoeActorEnterDamageFunc func = new AoeActorEnterDamageFunc();
            func.damage = damage;
            return func;
        }
    }

    /// <summary>
    /// Aoe演员进入造成伤害
    /// </summary>
    [NodeMenuItem("伤害")]
    public class Aoe_ActorLeaveDamageNode : Aoe_ActorLeaveFuncNode
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

        public override AoeActorLeave CreateFunc()
        {
            AoeActorLeaveDamageFunc func = new AoeActorLeaveDamageFunc();
            func.damage = damage;
            return func;
        }
    }
}