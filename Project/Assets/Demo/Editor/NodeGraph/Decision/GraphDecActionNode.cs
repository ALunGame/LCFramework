using DecNode;
using Demo.DecNode;
using LCECS.NodeGraph.Logic.Node;
using XPToolchains.NodeGraph;

namespace Demo.NodeGraph.Decision
{
    [NodeMenuItem("决策/攻击", typeof(DNAttack))]
    public class GraphDecAttackActionNode : GraphLogicActionNode
    {
        public override string name => "攻击";
    }

    [NodeMenuItem("决策/寻路玩家", typeof(DNSeekToPlayer))]
    public class GraphDecSeekToPlayerActionNode : GraphLogicActionNode
    {
        public override string name => "寻路玩家";
    }

    [NodeMenuItem("决策/徘徊", typeof(DNWander))]
    public class GraphDecWanderActionNode : GraphLogicActionNode
    {
        public override string name => "徘徊";
    }

    [NodeMenuItem("决策/发送请求", typeof(DNSendReq))]
    public class GraphDecSendReqNode : GraphLogicActionNode
    {
        public override string name => "发送请求";

        [Input(name = "请求Id", allowMultiple = false), ShowAsDrawer]
        public BevType ReqType = BevType.None;
    }
}
