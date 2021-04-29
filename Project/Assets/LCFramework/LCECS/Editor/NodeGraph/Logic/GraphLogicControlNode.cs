using LCECS.Core.Tree;
using LCECS.Core.Tree.Nodes.Control;
using UnityEngine;
using XPToolchains.NodeGraph;

namespace LCECS.NodeGraph.Logic.Node
{
    public class GraphLogicControlNode : GraphLogicBaseNode
    {
        public override Color color => Color.magenta;

        [Input(name = "上一节点", allowMultiple = false), Vertical]
        public GraphLogicNodeData lastNode = null;
    }

    [NodeMenuItem("控制/循环", typeof(NodeControlLoop))]
    public class GraphLogicLoopControlNode : GraphLogicControlNode
    {
        public override string name => "循环控制";

        [Input(name = "循环次数", allowMultiple = false), ShowAsDrawer]
        public int loopCount = -1;

        [Output(name = "下一节点", allowMultiple = false), Vertical]
        public GraphLogicNodeData nextNode = null;
    }

    [NodeMenuItem("控制/并行", typeof(NodeControlParallel))]
    public class GraphLogicParallelControlNode : GraphLogicControlNode
    {
        public override string name => "并行控制";

        [Input(name = "评估关系", allowMultiple = false), ShowAsDrawer]
        public NodeParallelType evaluateType;
        [Input(name = "运行关系", allowMultiple = false), ShowAsDrawer]
        public NodeParallelType excuteType;

        [Output(name = "下一节点", allowMultiple = true), Vertical]
        public GraphLogicNodeData nextNodes = null;
    }

    [NodeMenuItem("控制/随机选择", typeof(NodeControlRandomSelector))]
    public class GraphLogicRandomSelectorControlNode : GraphLogicControlNode
    {
        public override string name => "随机选择";

        [Output(name = "下一节点", allowMultiple = true), Vertical]
        public GraphLogicNodeData nextNodes = null;
    }

    [NodeMenuItem("控制/顺序选择", typeof(NodeControlSelector))]
    public class GraphLogicSelectorControlNode : GraphLogicControlNode
    {
        public override string name => "顺序选择";

        [Output(name = "下一节点", allowMultiple = true), Vertical]
        public GraphLogicNodeData nextNodes = null;
    }

    [NodeMenuItem("控制/序列执行", typeof(NodeControlSequence))]
    public class GraphLogicSequenceControlNode : GraphLogicControlNode
    {
        public override string name => "序列执行";

        [Output(name = "下一节点", allowMultiple = true), Vertical]
        public GraphLogicNodeData nextNodes = null;
    }
}
