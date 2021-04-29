using Demo.BevNode;
using LCECS.NodeGraph.Logic.Node;
using XPToolchains.NodeGraph;

namespace Demo.NodeGraph.Behavior
{
    [NodeMenuItem("行为/释放技能", typeof(BNPlaySkill))]
    public class GraphBevPlaySkillActionNode : GraphLogicActionNode
    {
        public override string name => "释放技能";

        [Input(name = "技能Id", allowMultiple = false), ShowAsDrawer]
        public int SkillId = 0;
    }

    [NodeMenuItem("行为/寻路", typeof(BNSeekPath))]
    public class GraphBevSeekPathActionNode : GraphLogicActionNode
    {
        public override string name => "寻路";
    }

    [NodeMenuItem("行为/停止移动", typeof(BNStopMove))]
    public class GraphBevStopMoveActionNode : GraphLogicActionNode
    {
        public override string name => "停止移动";
    }

    [NodeMenuItem("行为/转向玩家", typeof(BNTurnToPlayer))]
    public class GraphBevTurnToPlayerActionNode : GraphLogicActionNode
    {
        public override string name => "转向玩家";
    }

    [NodeMenuItem("行为/玩家移动", typeof(BNPlayerMove))]
    public class GraphBevPlayerMoveActionNode : GraphLogicActionNode
    {
        public override string name => "玩家移动";
    }

    [NodeMenuItem("行为/玩家普通攻击", typeof(BNPlayerNormalAttack))]
    public class GraphBevPlayerNormalAttackActionNode : GraphLogicActionNode
    {
        public override string name => "玩家普通攻击";
    }
}
