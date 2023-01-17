using LCECS.Core;
using LCECS.EntityGraph;
using LCNode;
using System;
using System.Collections.Generic;

namespace LCSkill
{
    [NodeMenuItem("演员/技能/技能组件")]
    public class Entity_Node_SkillCom : Entity_ComNode
    {
        public override string Title { get => "技能组件"; set => base.Title = value; }
        public override string Tooltip { get => "技能组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(SkillCom);

        public override BaseCom CreateRuntimeNode()
        {
            SkillCom skillCom = new SkillCom();
            return skillCom;
        }
    }

    [NodeMenuItem("全局/伤害组件")]
    public class Entity_Node_DamageCom : Entity_ComNode
    {
        public override string Title { get => "全局伤害组件"; set => base.Title = value; }
        public override string Tooltip { get => "全局伤害组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(DamageCom);

        public override BaseCom CreateRuntimeNode()
        {
            DamageCom damageCom = new DamageCom();
            return damageCom;
        }
    }

    [NodeMenuItem("全局/子弹组件")]
    public class Entity_Node_BulletCom : Entity_ComNode
    {
        public override string Title { get => "全局子弹组件"; set => base.Title = value; }
        public override string Tooltip { get => "全局子弹组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(BulletCom);

        public override BaseCom CreateRuntimeNode()
        {
            BulletCom bulletCom = new BulletCom();
            return bulletCom;
        }
    }

    [NodeMenuItem("全局/Aoe组件")]
    public class Entity_Node_AoeCom : Entity_ComNode
    {
        public override string Title { get => "全局Aoe组件"; set => base.Title = value; }
        public override string Tooltip { get => "全局Aoe组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(AoeCom);

        public override BaseCom CreateRuntimeNode()
        {
            AoeCom aoeCom = new AoeCom();
            return aoeCom;
        }
    }
}