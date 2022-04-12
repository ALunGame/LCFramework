using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCNode;
using LCNode.Model;
using UnityEngine;

namespace LCECS.Tree
{
    public abstract class Tree_ActNode : Tree_BaseNode
    {
        public override Color TitleColor { get => Color.green; set => base.TitleColor = value; }

        [InputPort("节点", BasePort.Capacity.Single, BasePort.Orientation.Vertical)]
        public NodeData node;

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }
    }

    [NodeMenuItem("常用/延时")]
    public class Tree_ActDelay : Tree_ActNode
    {
        public override string Title { get => "延时"; set => base.Title = value; }

        [NodeValue("延时(秒)")]
        public float WaitTime = -1;

        public override Node CreateRuntimeNode()
        {
            NodeActionDelay actionDelay = new NodeActionDelay();
            actionDelay.WaitTime = WaitTime;
            return actionDelay;
        }
    }
}
