using Demo.Decision;
using LCECS;
using LCECS.Core.Tree.Base;
using LCNode;
using LCToolkit;

namespace Demo.Tree
{

    [NodeMenuItem("演员/AI/检测敌人在攻击范围内")]
    public class Tree_Dec_Pre_CheckEnemyInAttackRange : Base_DEC_PRE_Node
    {
        public override string Title { get => "检测敌人在攻击范围内"; set => base.Title = value; }

        [NodeValue("攻击范围")]
        public Shape checkRange = new Shape();

        public override NodePremise CreateRuntimeNode()
        {
            DEC_PRE_CheckEnemyInAttackRange premise = new DEC_PRE_CheckEnemyInAttackRange();
            premise.checkRange = checkRange;
            return premise;
        }
    }
}