using LCECS.Core;
using LCECS.EntityGraph;
using LCNode;

namespace LCSkill
{
    [NodeMenuItem("演员/技能/技能组件")]
    public class Entity_Node_SkillCom : Entity_ComNode
    {
        public override string Title { get => "技能组件"; set => base.Title = value; }
        public override string Tooltip { get => "技能组件"; set => base.Tooltip = value; }

        public override BaseCom CreateRuntimeNode()
        {
            SkillCom skillCom = new SkillCom();
            return skillCom;
        }
    }

    [NodeMenuItem("演员/技能/伤害组件")]
    public class Entity_Node_DamageCom : Entity_ComNode
    {
        public override string Title { get => "伤害组件"; set => base.Title = value; }
        public override string Tooltip { get => "伤害组件"; set => base.Tooltip = value; }

        public override BaseCom CreateRuntimeNode()
        {
            DamageCom damageCom = new DamageCom();
            return damageCom;
        }
    }
}