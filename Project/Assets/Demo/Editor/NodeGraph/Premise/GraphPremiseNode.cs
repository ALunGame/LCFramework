using DecNode.Premise;
using Demo.Com;
using LCECS.NodeGraph.Logic.Node;
using Premise;
using XPToolchains.NodeGraph;

namespace Demo.NodeGraph.Premise
{
    [NodeMenuItem("前提/检测实体状态", typeof(PMCheckEntityState))]
    public class GraphPremiseCheckStateNode : GraphLogicPremiseNode
    {
        public override string name => "检测实体状态";

        [Input(name = "实体状态", allowMultiple = false), ShowAsDrawer]
        public EntityState entityState = EntityState.Normal;
    }

    [NodeMenuItem("前提/进入攻击区域", typeof(PMEnterAttack))]
    public class GraphPremiseEnterAttackNode : GraphLogicPremiseNode
    {
        public override string name => "进入攻击区域";
    }

    [NodeMenuItem("前提/进入警戒区域", typeof(PMEnterGuard))]
    public class GraphPremiseEnterGuardNode : GraphLogicPremiseNode
    {
        public override string name => "进入警戒区域";
    }
}
