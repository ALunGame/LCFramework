using Demo.Skill.Buff;
using LCNode;
using LCSkill;
using LCSkill.BuffGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    /// <summary>
    /// 生命周期伤害函数
    /// </summary>
    [NodeMenuItem("受伤抖动")]
    public class Buff_BeHurtShake : Buff_BeHurtFuncNode
    {
        public override string Title { get => "受伤抖动"; set => base.Title = value; }

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override BuffBeHurtFunc CreateFunc()
        {
            SkillBuffBeHurtShake func = new SkillBuffBeHurtShake();
            return func;
        }
    }

    /// <summary>
    /// 生命周期伤害函数
    /// </summary>
    [NodeMenuItem("受伤暂停决策")]
    public class Buff_BeHurtPauseDec : Buff_BeHurtFuncNode
    {
        public override string Title { get => "受伤暂停决策"; set => base.Title = value; }

        [NodeValue("暂停时间")]
        public float pauseTime = 0.3f;

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override BuffBeHurtFunc CreateFunc()
        {
            SkillBuffBeHurtPauseDec func = new SkillBuffBeHurtPauseDec();
            func.pauseTime = pauseTime;
            return func;
        }
    }
}
