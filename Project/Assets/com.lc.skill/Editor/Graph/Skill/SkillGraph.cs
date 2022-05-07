using LCNode;
using LCNode.Model;
using LCToolkit;
using System;
using System.Collections.Generic;

namespace LCSkill.SkillGraph
{
    public class SkillGraph : BaseGraph
    {
        [NonSerialized]
        private List<Type> NodeTypes = new List<Type>();

        protected override void OnEnabled()
        {
            base.OnEnabled();
            CollectNodeTypes();
            foreach (var item in nodes)
            {
                if (item.Value is Skill_Node)
                {
                    return;
                }
            }
            AddNode<Skill_Node>(UnityEngine.Vector2.zero);
        }

        private void CollectNodeTypes()
        {
            NodeTypes.Clear();
            foreach (var type in ReflectionHelper.GetChildTypes<Skill_ConditionNode>())
            {
                if (type.IsAbstract)
                    continue;
                if (!AttributeHelper.TryGetTypeAttribute(type, out NodeMenuItemAttribute attr))
                    continue;
                NodeTypes.Add(type);
            }
            foreach (var type in ReflectionHelper.GetChildTypes<Skill_CostNode>())
            {
                if (type.IsAbstract)
                    continue;
                if (!AttributeHelper.TryGetTypeAttribute(type, out NodeMenuItemAttribute attr))
                    continue;
                NodeTypes.Add(type);
            }
            foreach (var type in ReflectionHelper.GetChildTypes<Skill_LearnBuffNode>())
            {
                if (type.IsAbstract)
                    continue;
                if (!AttributeHelper.TryGetTypeAttribute(type, out NodeMenuItemAttribute attr))
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
