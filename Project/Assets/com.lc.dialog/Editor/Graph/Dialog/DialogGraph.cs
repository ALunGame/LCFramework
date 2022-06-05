using LCNode;
using LCNode.Model;
using LCToolkit;
using System;
using System.Collections.Generic;

namespace LCDialog.DialogGraph
{
    public class DialogGraph : BaseGraph
    {
        [NonSerialized]
        private List<Type> NodeTypes = new List<Type>();

        protected override void OnEnabled()
        {
            base.OnEnabled();
            CollectNodeTypes();
            foreach (var item in nodes)
            {
                if (item.Value is Dialog_Node)
                {
                    return;
                }
            }
            AddNode<Dialog_Node>(UnityEngine.Vector2.zero);
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
            NodeTypes.Add(typeof(Dialog_Node));
            NodeTypes.Add(typeof(Dialog_SpeakerNode));
            NodeTypes.Add(typeof(Dialog_DisposeStepNode));
            AddNodes<Dialog_Node>();

            NodeTypes.Add(typeof(Dialog_StepNode));
            AddNodes<Dialog_StepNode>();

            NodeTypes.Add(typeof(Dialog_DisposeNode));
            AddNodes<Dialog_DisposeNode>();

            AddNodes<Dialog_DisposeFuncNode>();
            AddNodes<Dialog_StepFuncNode>();
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
