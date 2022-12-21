using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// Buff生命周期添加Buff
    /// </summary>
    public class BuffLifeCycleAddBuffFunc : BuffLifeCycleFunc
    {
        public AddBuffModel addBuff;

        public override void Execute(BuffObj buff, int modifyStack = 0)
        {
            SkillLocate.Skill.CreateBuff(buff.ower, buff.ower, addBuff);
        }
    }
}