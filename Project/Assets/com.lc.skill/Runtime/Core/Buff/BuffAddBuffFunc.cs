using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// Buff生命周期造成伤害
    /// </summary>
    public class BuffLifeCycleAddBuffFunc : BuffLifeCycleFunc
    {
        public AddBuffModel addBuff;

        public override void Execute(BuffObj buff, int modifyStack = 0)
        {
        }
    }
}