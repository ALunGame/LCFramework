using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// Buff生命周期添加Aoe
    /// </summary>
    public class BuffLifeCycleAddAoeFunc : BuffLifeCycleFunc
    {
        public AddAoeModel addAoe;

        public override void Execute(BuffObj buff, int modifyStack = 0)
        {
            SkillLocate.Skill.CreateAoe(buff.ower, addAoe);
        }
    }
}