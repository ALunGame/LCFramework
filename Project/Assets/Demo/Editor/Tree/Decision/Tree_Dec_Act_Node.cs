using Demo.Decision;
using LCECS;
using LCECS.Core.Tree.Base;
using LCECS.Tree;
using LCNode;
using UnityEngine;

namespace Demo.Tree
{
    #region 玩家

    [NodeMenuItem("全局/输入/玩家输入移动")]
    public class Tree_Dec_Act_Input_Player_Move : Base_DEC_Act_Node
    {
        public override string Title { get => "玩家输入移动"; set => base.Title = value; }
        public override Node CreateRuntimeNode()
        {
            DEC_ACT_Input_Player_Move node = new DEC_ACT_Input_Player_Move();
            return node;
        }
    }

    [NodeMenuItem("全局/输入/玩家输入释放技能")]
    public class Tree_Dec_Act_Input_Player_PushSkill : Base_DEC_Act_Node
    {
        public override string Title { get => "玩家输入释放技能"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            DEC_ACT_Input_Player_PushSkill node = new DEC_ACT_Input_Player_PushSkill();
            return node;
        }
    }

    [NodeMenuItem("全局/输入/玩家输入")]
    public class Tree_Dec_Act_Input_Player : Base_DEC_Act_Node
    {
        public override string Title { get => "玩家输入"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            DEC_ACT_Input_Node node = new DEC_ACT_Input_Node();
            return node;
        }
    }

    #endregion

    #region AI

    [NodeMenuItem("演员/AI/徘徊")]
    public class Tree_Dec_Act_Wander : Base_DEC_Act_Node
    {
        public override string Title { get => "徘徊"; set => base.Title = value; }

        [NodeValue("徘徊范围")]
        public float WanderRange = -1;

        public override Node CreateRuntimeNode()
        {
            DEC_ACT_Wander node = new DEC_ACT_Wander();
            node.WanderRange = WanderRange;
            return node;
        }
    }

    [NodeMenuItem("演员/AI/注视环绕")]
    public class Tree_Dec_Act_GazeSurround : Base_DEC_Act_Node
    {
        public override string Title { get => "注视环绕"; set => base.Title = value; }

        [NodeValue("环绕范围")]
        public Vector2 gazeRange;

        [NodeValue("环绕时间")]
        public float gazeTimer;

        public override Node CreateRuntimeNode()
        {
            DEC_ACT_GazeSurround node = new DEC_ACT_GazeSurround();
            node.gazeRange = gazeRange;
            node.gazeTimer = gazeTimer;
            return node;
        }
    }

    [NodeMenuItem("演员/AI/释放技能")]
    public class Tree_Dec_Act_PushSkill : Base_DEC_Act_Node
    {
        public override string Title { get => "释放技能"; set => base.Title = value; }

        [NodeValue("释放技能")]
        public string skillId = "";

        public override Node CreateRuntimeNode()
        {
            DEC_ACT_PushSkill node = new DEC_ACT_PushSkill();
            node.skillId = skillId;
            return node;
        }
    }

    [NodeMenuItem("演员/AI/采集")]
    public class Tree_Dec_Act_Collect : Base_DEC_Act_Node
    {
        public override string Title { get => "采集"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            DEC_ACT_Collect node = new DEC_ACT_Collect();
            return node;
        }
    }

    [NodeMenuItem("演员/AI/存储")]
    public class Tree_Dec_Act_Storage : Base_DEC_Act_Node
    {
        public override string Title { get => "存储"; set => base.Title = value; }

        public override Node CreateRuntimeNode()
        {
            DEC_ACT_Storage node = new DEC_ACT_Storage();
            return node;
        }
    }

    #endregion

    #region 基础

    [NodeMenuItem("演员/基础/发送请求")]
    public class Tree_Dec_Act_PushRequset : Base_DEC_Act_Node
    {
        public override string Title { get => $"发送请求{requestId}"; set => base.Title = value; }

        [NodeValue("请求Id")]
        public RequestId requestId;

        public override Node CreateRuntimeNode()
        {
            DEC_ACT_PushRequset node = new DEC_ACT_PushRequset();
            node.requestId = requestId;
            return node;
        }
    }

    #endregion
}