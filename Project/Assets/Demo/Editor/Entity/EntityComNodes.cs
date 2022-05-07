using Demo.Com;
using LCECS.Core;
using LCECS.EntityGraph;
using LCNode;
using LCToolkit.ViewModel;
using System;

namespace Demo
{
    [NodeMenuItem("玩家/玩家组件")]
    public class Entity_Node_PlayerCom : Entity_ComNode
    {
        public override string Title { get => "玩家组件"; set => base.Title = value; }
        public override string Tooltip { get => "玩家组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(PlayerCom);

        public override BaseCom CreateRuntimeNode()
        {
            PlayerCom playerCom = new PlayerCom();
            return playerCom;
        }
    }

    #region 演员

    [NodeMenuItem("演员/属性组件")]
    public class Entity_Node_PropertyCom : Entity_ComNode
    {
        public override string Title { get => "属性组件"; set => base.Title = value; }
        public override string Tooltip { get => "属性组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(PropertyCom);


        [NodeValue("生命")]
        public PropertyInfo Hp = PropertyInfo.Zero;
        [NodeValue("魔法")]
        public PropertyInfo Mp = PropertyInfo.Zero;
        [NodeValue("攻击")]
        public PropertyInfo Attack = PropertyInfo.Zero;

        [NodeValue("移动速度")]
        public PropertyInfo MoveSpeed = PropertyInfo.Zero;

        [NodeValue("跳跃速度")]
        public PropertyInfo JumpSpeed = PropertyInfo.Zero;

        [NodeValue("爬墙速度")]
        public PropertyInfo ClimbSpeed = PropertyInfo.Zero;

        [NodeValue("行动速度")]
        public PropertyInfo ActionSpeed = PropertyInfo.Zero;

        protected override void OnEnabled()
        {
            base.OnEnabled();
            this[nameof(Hp)] = new BindableProperty<PropertyInfo>(() => Hp, v => Hp = v, "生命");
            this[nameof(Mp)] = new BindableProperty<PropertyInfo>(() => Mp, v => Mp = v, "魔法");
            this[nameof(Attack)] = new BindableProperty<PropertyInfo>(() => Attack, v => Attack = v, "攻击");
            this[nameof(MoveSpeed)] = new BindableProperty<PropertyInfo>(() => MoveSpeed, v => MoveSpeed = v, "移动速度");
            this[nameof(JumpSpeed)] = new BindableProperty<PropertyInfo>(() => JumpSpeed, v => JumpSpeed = v, "跳跃速度");
            this[nameof(ClimbSpeed)] = new BindableProperty<PropertyInfo>(() => ClimbSpeed, v => ClimbSpeed = v, "爬墙速度");
            this[nameof(ActionSpeed)] = new BindableProperty<PropertyInfo>(() => ActionSpeed, v => ActionSpeed = v, "行动速度");
        }


        public override BaseCom CreateRuntimeNode()
        {
            PropertyCom propertyCom = new PropertyCom();
            propertyCom.Hp = Hp;
            propertyCom.Mp = Mp;
            propertyCom.Attack = Attack;
            propertyCom.MoveSpeed = MoveSpeed;
            propertyCom.JumpSpeed = JumpSpeed;
            propertyCom.ClimbSpeed = ClimbSpeed;
            propertyCom.ActionSpeed = ActionSpeed;
            return propertyCom;
        }
    }

    [NodeMenuItem("演员/动画组件")]
    public class Entity_Node_AnimCom : Entity_ComNode
    {
        public override string Title { get => "动画组件"; set => base.Title = value; }
        public override string Tooltip { get => "动画组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(AnimCom);

        public override BaseCom CreateRuntimeNode()
        {
            AnimCom animCom = new AnimCom();
            return animCom;
        }
    }

    [NodeMenuItem("演员/阵营组件")]
    public class Entity_Node_CampCom : Entity_ComNode
    {
        public override string Title { get => "阵营组件"; set => base.Title = value; }
        public override string Tooltip { get => "阵营组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(CampCom);

        [NodeValue("阵营")]
        public CampType camp;

        public override BaseCom CreateRuntimeNode()
        {
            CampCom campCom = new CampCom();
            campCom.Camp = camp;
            return campCom;
        }
    }

    [NodeMenuItem("演员/阻挡组件")]
    public class Entity_Node_Collider2DCom : Entity_ComNode
    {
        public override string Title { get => "阻挡组件"; set => base.Title = value; }
        public override string Tooltip { get => "阻挡组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(Collider2DCom);

        public override BaseCom CreateRuntimeNode()
        {
            Collider2DCom collider2DCom = new Collider2DCom();
            return collider2DCom;
        }
    }

    [NodeMenuItem("演员/寻路组件")]
    public class Entity_Node_SeekPathCom : Entity_ComNode
    {
        public override string Title { get => "寻路组件"; set => base.Title = value; }
        public override string Tooltip { get => "寻路组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(SeekPathCom);

        public override BaseCom CreateRuntimeNode()
        {
            SeekPathCom seekPathCom = new SeekPathCom();
            return seekPathCom;
        }
    }

    [NodeMenuItem("演员/移动组件")]
    public class Entity_Node_MoveCom : Entity_ComNode
    {
        public override string Title { get => "移动组件"; set => base.Title = value; }
        public override string Tooltip { get => "移动组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(MoveCom);

        public override BaseCom CreateRuntimeNode()
        {
            MoveCom com = new MoveCom();
            return com;
        }
    }

    [NodeMenuItem("演员/重力组件")]
    public class Entity_Node_GravityCom : Entity_ComNode
    {
        public override string Title { get => "重力组件"; set => base.Title = value; }
        public override string Tooltip { get => "重力组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(GravityCom);

        [NodeValue("重力方向")]
        public GravityDir Dir = GravityDir.Down;

        public override BaseCom CreateRuntimeNode()
        {
            GravityCom com = new GravityCom();
            com.Dir = Dir;
            return com;
        }
    }

    [NodeMenuItem("演员/变换组件")]
    public class Entity_Node_TransformCom : Entity_ComNode
    {
        public override string Title { get => "变换组件"; set => base.Title = value; }
        public override string Tooltip { get => "变换组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(TransformCom);

        [NodeValue("正方向")]
        public DirType Dir = DirType.Right;

        public override BaseCom CreateRuntimeNode()
        {
            TransformCom com = new TransformCom();
            com.ForwardDir = Dir;
            return com;
        }
    }

    #endregion

    #region 全局

    //[NodeMenuItem("演员/属性组件")]
    //public class Entity_Node_PropertyCom : Entity_ComNode
    //{
    //    public override string Title { get => "属性组件"; set => base.Title = value; }
    //    public override string Tooltip { get => "属性组件"; set => base.Tooltip = value; }

    //    public PropertyInfo Hp = PropertyInfo.Zero;
    //    public PropertyInfo Mp = PropertyInfo.Zero;
    //    public PropertyInfo Attack = PropertyInfo.Zero;
    //    public PropertyInfo MoveSpeed = PropertyInfo.Zero;
    //    public PropertyInfo ActionSpeed = PropertyInfo.Zero;

    //    public override BaseCom CreateRuntimeNode()
    //    {
    //        PropertyCom propertyCom = new PropertyCom();
    //        propertyCom.Hp = Hp;
    //        propertyCom.Mp = Mp;
    //        propertyCom.Attack = Attack;
    //        propertyCom.MoveSpeed = MoveSpeed;
    //        propertyCom.ActionSpeed = ActionSpeed;
    //        return propertyCom;
    //    }
    //}

    #endregion
}