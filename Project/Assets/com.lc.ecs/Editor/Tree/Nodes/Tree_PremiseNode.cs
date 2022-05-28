using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCNode;
using LCNode.Model;
using UnityEngine;

namespace LCECS.Tree
{
    public abstract class Tree_PremiseNode : BaseNode
    {
        public override Color TitleColor { get => Color.blue; set => base.TitleColor = value; }

        [OutputPort("节点", BasePort.Capacity.Single)]
        public PremiseData node;

        [NodeValue("前提值")]
        public bool checkValue = true;

        [NodeValue("前提关系")]
        public PremiseType premiseType = PremiseType.AND;

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        /// <summary>
        /// 获得运行时前提
        /// </summary>
        /// <param name="node">连接的节点</param>
        /// <returns></returns>
        public NodePremise GetRuntimePremise(BaseNode node)
        {
            NodePremise premise = CreateRuntimeNode();
            premise.nodeUid = node.guid;
            premise.premiseType = premiseType;
            premise.checkValue = checkValue;
            return premise;
        }

        public abstract NodePremise CreateRuntimeNode();
    }
}
