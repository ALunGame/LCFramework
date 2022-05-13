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
            AddNodes<Skill_ConditionNode>();
            AddNodes<Skill_CostNode>();
            AddNodes<Skill_LearnBuffNode>();
            NodeTypes.Add(typeof(Skill_LearnBuffNode));
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

        public override IEnumerable<Type> GetNodeTypes()
        {
            foreach (var type in NodeTypes)
            {
                yield return type;
            }
        }
    }
}
