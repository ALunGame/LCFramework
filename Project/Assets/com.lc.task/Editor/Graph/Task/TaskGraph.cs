using LCNode;
using LCNode.Model;
using LCToolkit;
using System;
using System.Collections.Generic;

namespace LCTask.TaskGraph
{
    public class TaskGraph : BaseGraph
    {
        [NonSerialized]
        private List<Type> NodeTypes = new List<Type>();

        protected override void OnEnabled()
        {
            base.OnEnabled();
            CollectNodeTypes();
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
            AddNodes<Task_Node>();

            NodeTypes.Add(typeof(Task_TargetNode));

            AddNodes<Task_TargetDisplayFuncNode>();
            AddNodes<Task_ConditionFuncNode>();
            AddNodes<Task_ActionFuncNode>();
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
