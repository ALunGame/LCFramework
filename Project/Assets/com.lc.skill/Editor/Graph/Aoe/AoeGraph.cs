using LCNode;
using LCNode.Model;
using LCToolkit;
using System;
using System.Collections.Generic;

namespace LCSkill.AoeGraph
{
    public class AoeGraph : BaseGraph
    {
        [NonSerialized]
        private List<Type> NodeTypes = new List<Type>();

        protected override void OnEnabled()
        {
            base.OnEnabled();
            CollectNodeTypes();
            foreach (var item in nodes)
            {
                if (item.Value is Aoe_Node)
                {
                    return;
                }
            }
            AddNode<Aoe_Node>(UnityEngine.Vector2.zero);
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
            AddNodes<Aoe_MoveFuncNode>();
            AddNodes<Aoe_LifeCycleFuncNode>();
            AddNodes<Aoe_ActorEnterFuncNode>();
            AddNodes<Aoe_ActorLeaveFuncNode>();
            AddNodes<Aoe_BulletEnterFuncNode>();
            AddNodes<Aoe_BulletLeaveFuncNode>();
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
