using Demo.Com;
using Demo.Decision;
using LCECS;
using LCECS.Core.Tree.Base;
using LCNode;
using LCToolkit;

namespace Demo.Tree
{
    [NodeMenuItem("全局/检测输入")]
    public class Tree_Dec_Pre_CheckInputAction : Base_DEC_PRE_Node
    {
        public override string Title { get => "检测输入"; set => base.Title = value; }

        [NodeValue("输入指令")]
        public InputAction checkAction;

        public override NodePremise CreateRuntimeNode()
        {
            DEC_PRE_CheckInputAction premise = new DEC_PRE_CheckInputAction();
            premise.checkAction = checkAction;
            return premise;
        }
    }

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

    #region 采集

    [NodeMenuItem("演员/采集/检测采集物品超过上限")]
    public class Tree_Dec_Pre_CheckCollectItemIsOutMax : Base_DEC_PRE_Node
    {
        public override string Title { get => "检测采集物品超过上限"; set => base.Title = value; }

        public override NodePremise CreateRuntimeNode()
        {
            DEC_PRE_CheckCollectItemIsOutMax premise = new DEC_PRE_CheckCollectItemIsOutMax();
            return premise;
        }
    }

    [NodeMenuItem("演员/采集/检测采集物品还有剩余")]
    public class Tree_Dec_Pre_CheckCollectItemHasLeft : Base_DEC_PRE_Node
    {
        public override string Title { get => "检测采集物品还有剩余"; set => base.Title = value; }

        public override NodePremise CreateRuntimeNode()
        {
            DEC_PRE_CheckCollectItemHasLeft premise = new DEC_PRE_CheckCollectItemHasLeft();
            return premise;
        }
    }

    #endregion
}