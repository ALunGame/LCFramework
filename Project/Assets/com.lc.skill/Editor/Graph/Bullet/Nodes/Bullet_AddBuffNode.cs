using LCNode;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCSkill.BulletGraph
{
    /// <summary>
    /// 击中添加Buff
    /// </summary>
    [NodeMenuItem("添加Buff")]
    public class Bullet_HitAddBuffNode : Bullet_HitFuncNode
    {
        public override string Title { get => $"添加Buff:{addBuff.id}"; set => base.Title = value; }

        [NodeValue("添加Buff")]
        public AddBuffModel addBuff = new AddBuffModel();

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override BulletHitFunc CreateFunc()
        {
            BulletHitAddBuffFunc func = new BulletHitAddBuffFunc();
            func.addBuff = addBuff;
            return func;
        }
    }
}