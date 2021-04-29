
using LCECS.Core.Tree.Nodes.Action;
using LCECS.NodeGraph.Logic.Node;
using UnityEngine;
using XPToolchains.NodeGraph;

namespace LCECS.NodeGraph.Logic.Node
{
    public class GraphLogicActionNode : GraphLogicBaseNode
    {
        public override Color color => Color.green;

        [Input(name = "上一节点", allowMultiple = false), Vertical]
        public GraphLogicNodeData lastNode = null;
    }

    [NodeMenuItem("通用/延时",typeof(NodeActionDelay))]
    public class GraphLogicDelayActionNode : GraphLogicActionNode
    {
        public override string name => "延时";

        [Input(name = "延时", allowMultiple = false), ShowAsDrawer]
        public float WaitTime = -1;
    }
}


//namespace LCECS.NodeGraph.Decision.Node
//{
//    [NodeMenuItem("决策/发送请求", typeof(BNReqRequset))]
//    public class GraphLogicReqRequsetActionNode : GraphLogicActionNode
//    {
//        public override string name => "发送请求";

//        [Input(name = "请求Id", allowMultiple = false), ShowAsDrawer]
//        public int ReqId = -1;
//    } 
//}
