using System.Collections;
using UnityEngine;

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
        }
    }
}