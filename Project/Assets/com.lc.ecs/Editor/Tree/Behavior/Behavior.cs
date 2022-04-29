using Demo.Tree;
using LCNode;
using LCNode.Model;
using LCToolkit;
using System;
using System.Collections.Generic;

namespace LCECS.Tree
{
    public class Behavior : BaseGraph
    {
        [NonSerialized]
        private List<Type> NodeTypes = new List<Type>();

        protected override void OnEnabled()
        {
            base.OnEnabled();
            CollectNodeTypes();
            foreach (var item in nodes)
            {
                if (item.Value is Tree_RootNode)
                {
                    return;
                }
            }
            AddNode<Tree_RootNode>(UnityEngine.Vector2.zero);
        }

        private void CollectNodeTypes()
        {
            foreach (var type in ReflectionHelper.GetChildTypes<Tree_BaseNode>())
            {
                if (type.IsAbstract)
                    continue;
                if (!AttributeHelper.TryGetTypeAttribute(type, out NodeMenuItemAttribute attr))
                    continue;
                if (type == typeof(Tree_RootNode))
                    continue;
                NodeTypes.Add(type);
            }
            foreach (var type in ReflectionHelper.GetChildTypes<Tree_PremiseNode>())
            {
                if (type.IsAbstract)
                    continue;
                if (!AttributeHelper.TryGetTypeAttribute(type, out NodeMenuItemAttribute attr))
                    continue;
                if (type == typeof(Tree_RootNode))
                    continue;
                NodeTypes.Add(type);
            }

            foreach (var type in ReflectionHelper.GetChildTypes<Tree_Bev_Act_Node>())
            {
                if (type.IsAbstract)
                    continue;
                if (!AttributeHelper.TryGetTypeAttribute(type, out NodeMenuItemAttribute attr))
                    continue;
                if (type == typeof(Tree_RootNode))
                    continue;
                NodeTypes.Add(type);
            }
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
