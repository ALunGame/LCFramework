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

    #region 交互

    [NodeMenuItem("演员/AI/移动到采集点")]
    public class Tree_Bev_Act_MoveToCollect : Base_BEV_ACT_Node
    {
        public override string Title { get => "移动到采集点"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_MoveToCollectActor node = new BEV_ACT_MoveToCollectActor();
            return node;
        }
    }


    [NodeMenuItem("演员/AI/播放交互动画")]
    public class Tree_Bev_Act_PlayInteractiveAnim : Base_BEV_ACT_Node
    {
        public override string Title { get => "播放交互动画"; set => base.Title = value; }

        [NodeValue("动画时长")]
        public float animTime;

        [NodeValue("动画次数")]
        public int animCnt;

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_PlayInteractiveAnim node = new BEV_ACT_PlayInteractiveAnim();
            node.animTime = animTime;
            node.animCnt = animCnt;
            return node;
        }
    }

    [NodeMenuItem("演员/AI/执行交互")]
    public class Tree_Bev_Act_ExecuteInteractive : Base_BEV_ACT_Node
    {
        public override string Title { get => "移动到采集点"; set => base.Title = value; }

        public string interactiveName;

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_ExecuteInteractive node = new BEV_ACT_ExecuteInteractive();
            node.interactiveTypeName = Tree_Bev_Act_ExecuteInteractiveView.interactiveDict[interactiveName].FullName;
            return node;
        }
    }


    #endregion



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