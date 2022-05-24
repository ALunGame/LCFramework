namespace LCSkill
{
    /// <summary>
    /// Buff生命周期造成伤害
    /// </summary>
    public class BuffLifeCycleDamageFunc : BuffLifeCycleFunc
    {
        public DamageModel damage;

        public override void Execute(BuffObj buff, int modifyStack = 0)
        {
            SkillLocate.Damage.AddDamage(buff.originer, buff.ower, damage);
        }
    }

    #region 伤害流程

    /// <summary>
    /// 攻击他人时追加伤害
    /// </summary>
    public class BuffHurtDamageFunc : BuffHurtFunc
    {
        public DamageModel damage;

        //重置伤害还是累加
        public bool damageSet;

        //自己受到伤害
        public bool damageSelf;

        public override void Execute(BuffObj buff, ref AddDamageInfo damageInfo, SkillCom target)
        {
            if (damageSet)
                damageInfo.model = DamageModel.Null;
            if (damageSelf)
            {
                SkillLocate.Damage.AddDamage(buff.originer, damageInfo.attacker, damage);
            }
            else
            {
                SkillLocate.Damage.AddDamage(damageInfo.attacker, damageInfo.target, damage);
            }
        }
    }

    /// <summary>
    /// 被攻击时追加伤害
    /// </summary>
    public class BuffBeHurtDamageFunc : BuffBeHurtFunc
    {
        public DamageModel damage;

        //重置伤害还是累加
        public bool damageSet;

        //自己受到伤害
        public bool damageSelf;

        public override void Execute(BuffObj buff, ref AddDamageInfo damageInfo, SkillCom attacker)
        {
            if (damageSet)
                damageInfo.model = DamageModel.Null;
            if (damageSelf)
            {
                SkillLocate.Damage.AddDamage(buff.originer, damageInfo.target, damage);
            }
            else
            {
                SkillLocate.Damage.AddDamage(damageInfo.target, attacker, damage);
            }
        }
    }

    /// <summary>
    /// 击杀目标执行时追加伤害
    /// </summary>
    public class BuffKilledDamageFunc : BuffKilledFunc
    {
        public DamageModel damage;

        public override void Execute(BuffObj buff, AddDamageInfo damageInfo, SkillCom target)
        {
            SkillLocate.Damage.AddDamage(buff.originer, damageInfo.attacker, damage);
        }
    }

    /// <summary>
    /// 击杀目标执行时追加伤害
    /// </summary>
    public class BuffBeKilledDamageFunc : BuffBeKilledFunc
    {
        public DamageModel damage;

        public override void Execute(BuffObj buff, AddDamageInfo damageInfo, SkillCom attacker)
        {
            SkillLocate.Damage.AddDamage(buff.originer, damageInfo.attacker, damage);
        }
    }

    #endregion
}