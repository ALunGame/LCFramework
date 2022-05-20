using Demo.BevNode;
using LCECS.Core.Tree.Base;
using LCECS.Tree;
using LCNode;

namespace Demo.Tree
{
    public abstract class Tree_Bev_Act_Node : Tree_ActNode
    {
    }

    [NodeMenuItem("演员/移动")]
    public class Tree_Bev_Act_Move : Tree_Bev_Act_Node
    {
        public override string Title { get => "移动"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_Move node = new BEV_ACT_Move();
            return node;
        }

    }

    [NodeMenuItem("演员/跳跃")]
    public class Tree_Bev_Act_Jump : Tree_Bev_Act_Node
    {
        public override string Title { get => "跳跃"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_Jump node = new BEV_ACT_Jump();
            return node;
        }

    }

    [NodeMenuItem("演员/释放技能")]
    public class Tree_Bev_Act_PushSkill : Tree_Bev_Act_Node
    {
        public override string Title { get => "释放技能"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_PushSkill node = new BEV_ACT_PushSkill();
            return node;
        }

    }
}