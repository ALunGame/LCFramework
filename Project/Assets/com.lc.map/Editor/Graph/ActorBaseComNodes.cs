using LCECS.Core;
using LCECS.EntityGraph;
using LCNode;
using System;

namespace LCMap
{
    [NodeMenuItem("演员/基础/表现节点组件")]
    public class Entity_Actor_DisplayCom : Entity_ComNode
    {
        public override string Title { get => "表现节点组件"; set => base.Title = value; }
        public override string Tooltip { get => "表现节点组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(ActorDisplayCom);

        public override BaseCom CreateRuntimeNode()
        {
            ActorDisplayCom com = new ActorDisplayCom();
            return com;
        }
    }

    [NodeMenuItem("演员/基础/交互组件")]
    public class Entity_Actor_InteractiveCom : Entity_ComNode
    {
        public override string Title { get => "交互组件"; set => base.Title = value; }
        public override string Tooltip { get => "交互组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(ActorInteractiveCom);

        public override BaseCom CreateRuntimeNode()
        {
            ActorInteractiveCom com = new ActorInteractiveCom();
            return com;
        }
    }
}