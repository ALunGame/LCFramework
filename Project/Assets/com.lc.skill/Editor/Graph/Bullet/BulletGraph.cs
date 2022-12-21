using LCNode;
using LCNode.Model;
using LCToolkit;
using System;
using System.Collections.Generic;

namespace LCSkill.BulletGraph
{
    public class BulletGraph : BaseGraph
    {
        [NonSerialized]
        private List<Type> NodeTypes = new List<Type>();

        protected override void OnEnabled()
        {
            base.OnEnabled();
            CollectNodeTypes();
            foreach (var item in nodes)
            {
                if (item.Value is Bullet_Node)
                {
                    return;
                }
            }
            AddNode<Bullet_Node>(UnityEngine.Vector2.zero);
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
            AddNodes<Bullet_MoveFuncNode>();
            AddNodes<Bullet_CatchFuncNode>();
            AddNodes<Bullet_LifeCycleFuncNode>();
            AddNodes<Bullet_HitFuncNode>();
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
