using LCECS.Core.Tree;
using UnityEngine;
using XPToolchains.NodeGraph;

namespace LCECS.NodeGraph.Logic.Node
{
    public class GraphLogicPremiseNode : BaseNode
    {
        public override Color color => Color.blue;

        [Input(name = "下一个前提")]
        public GraphLogicPremiseData lastPremise = null;

        [Input(name = "前提值", allowMultiple = false),ShowAsDrawer]
        public bool checkValue = false;

        [Input(name = "前提关系", allowMultiple = false), ShowAsDrawer]
        public PremiseType premiseType = PremiseType.AND;

        [Output(name = "节点")]
        public GraphLogicPremiseData node = null;
    }
}
