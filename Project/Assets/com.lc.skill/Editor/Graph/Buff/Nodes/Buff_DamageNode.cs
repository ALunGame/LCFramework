using LCNode;
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

    /// <summary>
    /// 生命周期伤害函数
    /// </summary>
    [NodeMenuItem("伤害")]
    public class Buff_HurtDamageNode : Buff_HurtFuncNode
    {
        public override string Title { get => "造成伤害"; set => base.Title = value; }

        [NodeValue("伤害")]
        public DamageModel damage = new DamageModel();

        [NodeValue("重置伤害还是累加")]
        public bool damageSet = false;

        [NodeValue("自己受到伤害")]
        public bool damageSelf = false;

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

        public override BuffHurtFunc CreateFunc()
        {
            BuffHurtDamageFunc func = new BuffHurtDamageFunc();
            func.damage = damage;
            func.damageSelf = damageSelf;
            func.damageSet = damageSet;
            return func;
        }
    }

    /// <summary>
    /// 生命周期伤害函数
    /// </summary>
    [NodeMenuItem("伤害")]
    public class Buff_BeHurtDamageNode : Buff_BeHurtFuncNode
    {
        public override string Title { get => "造成伤害"; set => base.Title = value; }

        [NodeValue("伤害")]
        public DamageModel damage = new DamageModel();

        [NodeValue("重置伤害还是累加")]
        public bool damageSet = false;

        [NodeValue("自己受到伤害")]
        public bool damageSelf = false;

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

        public override BuffBeHurtFunc CreateFunc()
        {
            BuffBeHurtDamageFunc func = new BuffBeHurtDamageFunc();
            func.damage = damage;
            func.damageSelf = damageSelf;
            func.damageSet = damageSet;
            return func;
        }
    }

    /// <summary>
    /// 生命周期伤害函数
    /// </summary>
    [NodeMenuItem("伤害")]
    public class Buff_KilledDamageNode : Buff_KilledFuncNode
    {
        public override string Title { get => "造成伤害"; set => base.Title = value; }

        public override string Tooltip { get => "此阶段只能对攻击者造成伤害"; set => base.Tooltip = value; }

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

        public override BuffKilledFunc CreateFunc()
        {
            BuffKilledDamageFunc func = new BuffKilledDamageFunc();
            func.damage = damage;
            return func;
        }
    }

    /// <summary>
    /// 生命周期伤害函数
    /// </summary>
    [NodeMenuItem("伤害")]
    public class Buff_BeKilledDamageNode : Buff_BeKilledFuncNode
    {
        public override string Title { get => "造成伤害"; set => base.Title = value; }

        public override string Tooltip { get => "此阶段只能对攻击者造成伤害"; set => base.Tooltip = value; }

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

        public override BuffBeKilledFunc CreateFunc()
        {
            BuffBeKilledDamageFunc func = new BuffBeKilledDamageFunc();
            func.damage = damage;
            return func;
        }
    }
}