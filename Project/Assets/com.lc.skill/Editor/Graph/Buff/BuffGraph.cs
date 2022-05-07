using LCNode;
using LCNode.Model;
using LCToolkit;
using System;
using System.Collections.Generic;

namespace LCSkill.BuffGraph
{
    public class BuffGraph : BaseGraph
    {
        [NonSerialized]
        private List<Type> NodeTypes = new List<Type>();

        protected override void OnEnabled()
        {
            base.OnEnabled();
            CollectNodeTypes();
            foreach (var item in nodes)
            {
                if (item.Value is Buff_Node)
                {
                    return;
                }
            }
            AddNode<Buff_Node>(UnityEngine.Vector2.zero);
        }

        private void AddNodes<T>()
        {
            foreach (var type in ReflectionHelper.GetChildTypes<T>())
            {
                if (type.IsAbstract)
                    continue;
                if (!AttributeHelper.TryGetTypeAttribute(type, out NodeMenuItemAttribute attr))
                    continue;
                NodeTypes.Add(type);
            }
        }

        private void CollectNodeTypes()
        {
            NodeTypes.Clear();
            AddNodes<Buff_FreedFuncNode>();
            AddNodes<Buff_LifeCycleFuncNode>();
            AddNodes<Buff_HurtFuncNode>();
            AddNodes<Buff_BeHurtFuncNode>();
            AddNodes<Buff_KilledFuncNode>();
            AddNodes<Buff_BeKilledFuncNode>();
        }

        public override IEnumerable<Type> GetNodeTypes()
        {
            foreach (var type in NodeTypes)
            {
                yield return type;
            }
        }
    }
}
