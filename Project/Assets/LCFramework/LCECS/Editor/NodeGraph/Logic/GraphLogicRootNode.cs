using LCECS.Core.Tree.Nodes;
using UnityEngine;
using XPToolchains.NodeGraph;

namespace LCECS.NodeGraph.Logic.Node
{
    public class GraphLogicNodeData { }
    public class GraphLogicPremiseData { }

    public class GraphLogicBaseNode : BaseNode
    {
        [Input(name = "节点前提", allowMultiple = false)]
        public GraphLogicPremiseData premises = null;
    }

    [NodeMenuItem("根节点", typeof(NodeRoot))]
    public class GraphLogicRootNode : GraphLogicBaseNode
    {
        public override Color color => Color.white;

        public override string name => "根节点";

        [Output(name = "下一节点", allowMultiple = false), Vertical]
        public GraphLogicNodeData nextNode = null;
    }
}
