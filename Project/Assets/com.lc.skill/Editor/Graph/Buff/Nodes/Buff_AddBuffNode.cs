using LCNode;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCSkill.BuffGraph
{
    /// <summary>
    /// 生命周期添加Buff
    /// </summary>
    [NodeMenuItem("添加Buff")]
    public class Buff_LifeCycleAddBuffNode : Buff_LifeCycleFuncNode
    {
        public override string Title { get => $"添加Buff:{addBuff.id}"; set => base.Title = value; }

        [NodeValue("添加Buff")]
        public AddBuffModel addBuff = new AddBuffModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override BuffLifeCycleFunc CreateFunc()
        {
            BuffLifeCycleAddBuffFunc func = new BuffLifeCycleAddBuffFunc();
            func.addBuff = addBuff;
            return func;
        }
    }
}