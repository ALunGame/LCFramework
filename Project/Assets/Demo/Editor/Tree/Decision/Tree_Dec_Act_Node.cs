using Demo.Decision;
using LCECS;
using LCECS.Core.Tree.Base;
using LCECS.Tree;
using LCNode;
using UnityEngine;

namespace Demo.Tree
{
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
}