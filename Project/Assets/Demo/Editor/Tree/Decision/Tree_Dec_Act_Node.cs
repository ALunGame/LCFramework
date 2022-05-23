using Demo.Decision;
using LCECS.Core.Tree.Base;
using LCECS.Tree;
using LCNode;

namespace Demo.Tree
{
    public abstract class Tree_Dec_Act_Node : Tree_ActNode
    {
    }


    [NodeMenuItem("演员/AI/徘徊")]
    public class Tree_Dec_Act_Wander : Tree_Dec_Act_Node
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
}