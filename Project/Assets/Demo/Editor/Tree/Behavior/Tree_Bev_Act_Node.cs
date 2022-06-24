using Demo.Behavior;
using LCECS;
using LCECS.Core.Tree.Base;
using LCNode;

namespace Demo.Tree
{
    [NodeMenuItem("演员/移动")]
    public class Tree_Bev_Act_Move : Base_BEV_ACT_Node
    {
        public override string Title { get => "移动"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_PlayerMove node = new BEV_ACT_PlayerMove();
            return node;
        }

    }

    [NodeMenuItem("演员/AI/徘徊")]
    public class Tree_Bev_Act_Wander : Base_BEV_ACT_Node
    {
        public override string Title { get => "徘徊"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_Wander node = new BEV_ACT_Wander();
            return node;
        }
    }

    [NodeMenuItem("演员/AI/注视环绕")]
    public class Tree_Bev_Act_GazeSurround : Base_BEV_ACT_Node
    {
        public override string Title { get => "注视环绕"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_GazeSurround node = new BEV_ACT_GazeSurround();
            return node;
        }
    }

    [NodeMenuItem("演员/AI/采集")]
    public class Tree_Bev_Act_Collect : Base_BEV_ACT_Node
    {
        public override string Title { get => "采集"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_Collect node = new BEV_ACT_Collect();
            return node;
        }
    }

    [NodeMenuItem("演员/释放技能")]
    public class Tree_Bev_Act_PushSkill : Base_BEV_ACT_Node
    {
        public override string Title { get => "释放技能"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_PushSkill node = new BEV_ACT_PushSkill();
            return node;
        }

    }
}