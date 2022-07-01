using Demo.Behavior;
using LCECS;
using LCECS.Core.Tree.Base;
using LCNode;

namespace Demo.Tree
{
    [NodeMenuItem("演员/基础/设置演员显示隐藏")]
    public class Tree_Bev_Act_SetActorActive : Base_BEV_ACT_Node
    {
        public override string Title { get => "设置演员显示隐藏"; set => base.Title = value; }

        [NodeValue("显示")]
        public bool isActive = false;

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_SetActorActive node = new BEV_ACT_SetActorActive();
            node.isActive = isActive;
            return node;
        }

    }

    [NodeMenuItem("演员/玩家/玩家移动")]
    public class Tree_Bev_Act_Move : Base_BEV_ACT_Node
    {
        public override string Title { get => "玩家移动"; set => base.Title = value; }

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

    [NodeMenuItem("演员/工作/移动到采集点")]
    public class Tree_Bev_Act_MoveToCollect : Base_BEV_ACT_Node
    {
        public override string Title { get => "移动到采集点"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_MoveToCollectActor node = new BEV_ACT_MoveToCollectActor();
            return node;
        }
    }

    [NodeMenuItem("演员/工作/移动到存储建筑")]
    public class Tree_Bev_Act_MoveToStorage : Base_BEV_ACT_Node
    {
        public override string Title { get => "移动到存储建筑"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_MoveToStorageActor node = new BEV_ACT_MoveToStorageActor();
            return node;
        }
    }

    [NodeMenuItem("演员/工作/移动到生产建筑")]
    public class Tree_Bev_Act_MoveToProduce : Base_BEV_ACT_Node
    {
        public override string Title { get => "移动到生产建筑"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_MoveToProduceActor node = new BEV_ACT_MoveToProduceActor();
            return node;
        }
    }

    [NodeMenuItem("演员/工作/移动到休息建筑")]
    public class Tree_Bev_Act_MoveToRestBuildingActor : Base_BEV_ACT_Node
    {
        public override string Title { get => "移动到休息建筑"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            BEV_ACT_MoveToRestBuildingActor node = new BEV_ACT_MoveToRestBuildingActor();
            return node;
        }

    }

    [NodeMenuItem("演员/交互/播放交互动画")]
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

    [NodeMenuItem("演员/交互/执行交互")]
    public class Tree_Bev_Act_ExecuteInteractive : Base_BEV_ACT_Node
    {
        public override string Title { get => $"执行{interactiveName}交互"; set => base.Title = value; }

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