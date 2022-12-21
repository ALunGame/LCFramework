using System;
using System.Collections.Generic;
using System.Linq;
using LCToolkit.ViewModel;
using UnityEngine;

namespace LCNode.Model
{
    public partial class BaseGroup : ViewModel
    {
        public event Action<IEnumerable<BaseNode>> onNodesAdded;
        public event Action<IEnumerable<BaseNode>> onNodesRemoved;

        #region Property
        
        public Type ModelType
        {
            get;
        }
        public BaseGraph Owner
        {
            get;
            private set;
        }
        public string GroupName
        {
            get { return GetPropertyValue<string>(nameof(groupName)); }
            set { SetPropertyValue(nameof(groupName), value); }
        }
        public Vector2 Position
        {
            get { return GetPropertyValue<Vector2>(nameof(position)); }
            set { SetPropertyValue(nameof(position), value); }
        }
        public Color BackgroundColor
        {
            get { return GetPropertyValue<Color>(nameof(backgroundColor)); }
            set { SetPropertyValue(nameof(backgroundColor), value); }
        }
        public IReadOnlyList<string> Nodes
        {
            get { return nodes; }
        }
        #endregion

        internal void Enable(BaseGraph graph)
        {
            Owner = graph;
            this[nameof(BaseGroup.groupName)] = new BindableProperty<string>(() => groupName, v => groupName = v);
            this[nameof(BaseGroup.position)] = new BindableProperty<Vector2>(() => position, v => position = v);
            this[nameof(BaseGroup.backgroundColor)] = new BindableProperty<Color>(() => backgroundColor, v => backgroundColor = v);
            OnEnabled();
        }

        public void AddNodes(IEnumerable<BaseNode> pNodes)
        {
            var tempNodes = pNodes.Where(element => !nodes.Contains(element.GUID) && element.Owner == this.Owner).ToArray();
            foreach (var element in tempNodes)
            {
                foreach (var group in Owner.Groups)
                {
                    group.nodes.Remove(element.GUID);
                }
                nodes.Add(element.GUID);
            }
            onNodesAdded?.Invoke(tempNodes);
        }

        public void RemoveNodes(IEnumerable<BaseNode> pNodes)
        {
            var tempNodes = pNodes.Where(element => nodes.Contains(element.GUID) && element.Owner == Owner).ToArray();
            foreach (var node in tempNodes)
            {
                nodes.Remove(node.GUID);
            }
            onNodesRemoved?.Invoke(pNodes);
        }

        public void AddNode(BaseNode element)
        {
            AddNodes(new BaseNode[] { element });
        }

        public void RemoveNode(BaseNode element)
        {
            RemoveNodes(new BaseNode[] { element });
        }

        public void AddNodesWithoutNotify(IEnumerable<BaseNode> elements)
        {
            elements = elements.Where(element => !nodes.Contains(element.GUID) && element.Owner == this.Owner);
            foreach (var element in elements)
            {
                nodes.Add(element.GUID);
            }
        }

        public void RemoveNodesWithoutNotify(IEnumerable<BaseNode> elements)
        {
            elements = elements.Where(element => nodes.Contains(element.GUID) && element.Owner == this.Owner);
            foreach (var element in elements)
            {
                nodes.Remove(element.GUID);
            }
        }

        #region Overrides
        protected virtual void OnEnabled() { }

        #endregion
    }
}