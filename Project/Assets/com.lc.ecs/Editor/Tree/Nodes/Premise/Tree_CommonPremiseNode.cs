using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Premise;
using LCNode;
using LCToolkit;

namespace LCECS.Tree.Premise
{
    [NodeMenuItem("通用/检测执行实体Id")]
    public class Tree_Common_Pre_CheckEntityId : Tree_PremiseNode
    {
        public override string Title { get => "检测执行实体Id"; set => base.Title = value; }
        
        [NodeValue("实体Id")]
        public int entityId = 0;
        
        public override NodePremise CreateRuntimeNode()
        {
            BEV_PRE_CheckEntityId func = new BEV_PRE_CheckEntityId();
            func.entityId = entityId;
            return func;
        }
    }
}