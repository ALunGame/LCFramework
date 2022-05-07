using System;

namespace LCSkill
{
    /// <summary>
    /// 当释放一个技能时执行的函数
    /// 为了处理当技能释放时，更改技能表现，比如没有魔法还要释放，会执行没有魔法的动画
    /// </summary>
    public abstract class BuffOnFreedFunc
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="buff">buff对象</param>
        /// <param name="skill">技能对象</param>
        /// <param name="timeline">技能表现</param>
        public abstract TimelineObj Execute(BuffObj buff, SkillObj skill, TimelineObj timeline);
    }

    /// <summary>
    /// Buff生命周期函数
    /// 1，被添加、改变层数时
    /// 2，被移除时
    /// 3，间隔时间调用
    /// </summary>
    public abstract class BuffLifeCycleFunc
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="buff">Buff对象</param>
        /// <param name="modifyStack">改变的层数</param>
        public abstract void Execute(BuffObj buff, int modifyStack = 0);
    }

    #region 伤害流程

    /// <summary>
    /// 在执行伤害流程时，拥有这个Buff作为攻击者执行的函数
    /// </summary>
    public abstract class BuffHurtFunc
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="buff">Buff对象</param>
        /// <param name="damageInfo">Buff可能会重新计算伤害</param>
        /// <param name="target">攻击目标</param>
        public abstract void Execute(BuffObj buff, ref AddDamageInfo damageInfo, SkillCom target);
    }

    /// <summary>
    /// 在执行伤害流程时，拥有这个Buff被攻击执行的函数
    /// </summary>
    public abstract class BuffBeHurtFunc
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="buff">Buff对象</param>
        /// <param name="damageInfo">Buff可能会重新计算伤害</param>
        /// <param name="attacker">攻击者</param>
        public abstract void Execute(BuffObj buff, ref AddDamageInfo damageInfo, SkillCom attacker);
    }

    /// <summary>
    /// 在执行伤害流程时，如果击杀目标执行的函数
    /// </summary>
    public abstract class BuffKilledFunc
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="buff">Buff对象</param>
        /// <param name="damageInfo">伤害信息</param>
        /// <param name="target">被击杀目标</param>
        public abstract void Execute(BuffObj buff, AddDamageInfo damageInfo, SkillCom target);
    }

    /// <summary>
    /// 在执行伤害流程时，拥有这个Buff被杀死执行的函数
    /// </summary>
    public abstract class BuffBeKilledFunc
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="buff">buff对象</param>
        /// <param name="damageInfo">伤害信息</param>
        /// <param name="attacker">攻击者</param>
        public abstract void Execute(BuffObj buff, AddDamageInfo damageInfo, SkillCom attacker);
    }


    #endregion
}
