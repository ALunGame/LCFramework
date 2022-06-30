using Demo.Com;
using LCECS.Core;
using LCECS.EntityGraph;
using LCNode;
using System;
using System.Collections.Generic;

namespace Demo
{
    #region 演员

    [NodeMenuItem("演员/基础属性组件")]
    public class Entity_Node_BasePropertyCom : Entity_ComNode
    {
        public override string Title { get => "基础属性组件"; set => base.Title = value; }
        public override string Tooltip { get => "基础属性组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(PlayerPropertyCom);

        [NodeValue("食物")]
        public PropertyInfo Food = PropertyInfo.Zero;

        [NodeValue("生命")]
        public PropertyInfo Hp = PropertyInfo.Zero;

        [NodeValue("攻击")]
        public PropertyInfo Attack = PropertyInfo.Zero;

        [NodeValue("移动速度")]
        public PropertyInfo MoveSpeed = PropertyInfo.Zero;

        [NodeValue("行动速度")]
        public PropertyInfo ActionSpeed = PropertyInfo.Zero;

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override BaseCom CreateRuntimeNode()
        {
            BasePropertyCom propertyCom = new BasePropertyCom();
            propertyCom.Food = Food;
            propertyCom.Hp = Hp;
            propertyCom.Attack = Attack;
            propertyCom.MoveSpeed = MoveSpeed;
            propertyCom.ActionSpeed = ActionSpeed;
            return propertyCom;
        }
    }

    [NodeMenuItem("演员/玩家属性组件")]
    public class Entity_Node_PropertyCom : Entity_ComNode
    {
        public override string Title { get => "玩家属性组件"; set => base.Title = value; }
        public override string Tooltip { get => "玩家属性组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(PlayerPropertyCom);

        [NodeValue("魔法")]
        public PropertyInfo Mp = PropertyInfo.Zero;

        [NodeValue("跳跃速度")]
        public PropertyInfo JumpSpeed = PropertyInfo.Zero;

        [NodeValue("爬墙速度")]
        public PropertyInfo ClimbSpeed = PropertyInfo.Zero;

        [NodeValue("行动速度")]
        public PropertyInfo ActionSpeed = PropertyInfo.Zero;

        protected override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override BaseCom CreateRuntimeNode()
        {
            PlayerPropertyCom propertyCom = new PlayerPropertyCom();
            propertyCom.Mp = Mp;
            propertyCom.JumpSpeed = JumpSpeed;
            propertyCom.ClimbSpeed = ClimbSpeed;
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

    [NodeMenuItem("演员/背包组件")]
    public class Entity_Node_BagCom : Entity_ComNode
    {
        public override string Title { get => "背包组件"; set => base.Title = value; }
        public override string Tooltip { get => "背包组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(BagCom);

        [NodeValue("默认物品")]
        public List<BagItem> items = new List<BagItem>();

        public override BaseCom CreateRuntimeNode()
        {
            BagCom bagCom = new BagCom();
            bagCom.itemlist = items;
            return bagCom;
        }
    }

    [NodeMenuItem("演员/建筑/建筑组件")]
    public class Entity_Node_BuildingCom : Entity_ComNode
    {
        public override string Title { get => "建筑组件"; set => base.Title = value; }
        public override string Tooltip { get => "建筑组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(BuildingCom);

        public override BaseCom CreateRuntimeNode()
        {
            BuildingCom com = new BuildingCom();
            return com;
        }
    }

    [NodeMenuItem("演员/气泡组件")]
    public class Entity_Node_BubbleCom : Entity_ComNode
    {
        public override string Title { get => "气泡组件"; set => base.Title = value; }
        public override string Tooltip { get => "气泡组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(BubbleCom);

        public override BaseCom CreateRuntimeNode()
        {
            BubbleCom bubbleCom = new BubbleCom();
            return bubbleCom;
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

    [NodeMenuItem("演员/移动组件")]
    public class Entity_Node_MoveCom : Entity_ComNode
    {
        public override string Title { get => "移动组件"; set => base.Title = value; }
        public override string Tooltip { get => "移动组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(PlayerMoveCom);

        public override BaseCom CreateRuntimeNode()
        {
            PlayerMoveCom com = new PlayerMoveCom();
            return com;
        }
    }

    [NodeMenuItem("演员/重力组件")]
    public class Entity_Node_GravityCom : Entity_ComNode
    {
        public override string Title { get => "重力组件"; set => base.Title = value; }
        public override string Tooltip { get => "重力组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(GravityCom);

        [NodeValue("质量")]
        public float Mass = 1;

        public override BaseCom CreateRuntimeNode()
        {
            GravityCom com = new GravityCom();
            com.Mass = Mass;
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


    #region AI

    [NodeMenuItem("演员/AI/徘徊组件")]
    public class Entity_Node_WanderCom : Entity_ComNode
    {
        public override string Title { get => "徘徊组件"; set => base.Title = value; }
        public override string Tooltip { get => "徘徊组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(WanderCom);

        public override BaseCom CreateRuntimeNode()
        {
            WanderCom com = new WanderCom();
            return com;
        }
    }

    [NodeMenuItem("演员/AI/注视环绕组件")]
    public class Entity_Node_GazeSurroundCom : Entity_ComNode
    {
        public override string Title { get => "注视环绕组件"; set => base.Title = value; }
        public override string Tooltip { get => "注视环绕组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(GazeSurroundCom);

        public override BaseCom CreateRuntimeNode()
        {
            GazeSurroundCom com = new GazeSurroundCom();
            return com;
        }
    }

    [NodeMenuItem("演员/AI/采集组件")]
    public class Entity_Node_CollectCom : Entity_ComNode
    {
        public override string Title { get => "采集组件"; set => base.Title = value; }
        public override string Tooltip { get => "采集组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(CollectCom);

        [NodeValue("采集的木匾演员Id")]
        public int collectTargetActorId;

        [NodeValue("采集最大数量")]
        public int collectMaxCnt;

        public override BaseCom CreateRuntimeNode()
        {
            CollectCom com = new CollectCom();
            com.collectActorId = collectTargetActorId;
            com.collectMaxCnt = collectMaxCnt;
            return com;
        }
    }

    [NodeMenuItem("演员/AI/道路移动组件")]
    public class Entity_Node_WayPointMoveCom : Entity_ComNode
    {
        public override string Title { get => "道路移动组件"; set => base.Title = value; }
        public override string Tooltip { get => "道路移动组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(WayPointMoveCom);

        public override BaseCom CreateRuntimeNode()
        {
            WayPointMoveCom com = new WayPointMoveCom();
            return com;
        }
    }

    #endregion

    #region 工作

    [NodeMenuItem("演员/工作/工人组件")]
    public class Entity_Node_WorkerCom : Entity_ComNode
    {
        public override string Title { get => "工人组件"; set => base.Title = value; }
        public override string Tooltip { get => "工人组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(WorkerCom);

        /// <summary>
        /// 管理者演员
        /// </summary>
        [NodeValue("管理者演员Id")]
        public int managerActorId;

        public override BaseCom CreateRuntimeNode()
        {
            WorkerCom com = new WorkerCom();
            com.managerActorId = managerActorId;
            return com;
        }
    }

    [NodeMenuItem("演员/工作/管理者组件")]
    public class Entity_Node_ManagerCom : Entity_ComNode
    {
        public override string Title { get => "管理者组件"; set => base.Title = value; }
        public override string Tooltip { get => "管理者组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(ManagerCom);

        /// <summary>
        /// 管理者演员
        /// </summary>
        [NodeValue("建筑演员Id")]
        public int buildingActorId;

        public override BaseCom CreateRuntimeNode()
        {
            ManagerCom com = new ManagerCom();
            com.buildingActorId = buildingActorId;
            return com;
        }
    }

    [NodeMenuItem("演员/工作/生产组件")]
    public class Entity_Node_ProduceCom : Entity_ComNode
    {
        public override string Title { get => "生产组件"; set => base.Title = value; }
        public override string Tooltip { get => "生产组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(ProduceCom);

        /// <summary>
        /// 生产id
        /// </summary>
        [NodeValue("生产id")]
        public List<int> produceIds = new List<int>();

        public override BaseCom CreateRuntimeNode()
        {
            ProduceCom com = new ProduceCom();
            com.produceIds = produceIds;
            return com;
        }
    }

    #endregion

    #endregion

    #region 全局

    [NodeMenuItem("全局/相机/拖动相机")]
    public class Entity_Node_DragCameraCom : Entity_ComNode
    {
        public override string Title { get => "拖动相机组件"; set => base.Title = value; }
        public override string Tooltip { get => "拖动相机组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(DragCameraCom);

        /// <summary>
        /// 拖拽速度
        /// </summary>
        [NodeValue("拖拽速度")]
        public float DragSpeed = 1.2f;

        /// <summary>
        /// 弹簧回弹区域
        /// </summary>
        [NodeValue("弹簧回弹区域")]
        public float SpringOffset = 0.5f;

        /// <summary>
        /// 弹簧回弹时间
        /// </summary>
        [NodeValue("弹簧回弹时间")]
        public float SpringSmoothTime = 0.05f;

        /// <summary>
        /// 弹簧强度
        /// </summary>
        [NodeValue("弹簧强度")]
        public float SpringIntensity = 0.75f;

        /// <summary>
        /// 惯性移动速率
        /// </summary>
        [NodeValue("惯性移动速率")]
        public float InertiaRate = 0.4f;

        /// <summary>
        /// 惯性移动阻尼总时间
        /// </summary>
        [NodeValue("惯性移动阻尼总时间")]
        public float InertiaDampDuration = 0.4f;

        public override BaseCom CreateRuntimeNode()
        {
            DragCameraCom dragCom = new DragCameraCom();
            dragCom.DragSpeed = DragSpeed;
            dragCom.SpringOffset = SpringOffset;
            dragCom.SpringSmoothTime = SpringSmoothTime;
            dragCom.SpringIntensity = SpringIntensity;
            dragCom.InertiaRate = InertiaRate;
            dragCom.InertiaDampDuration = InertiaDampDuration;
            return dragCom;
        }
    }

    [NodeMenuItem("全局/相机/跟随相机")]
    public class Entity_Node_FollowCameraCom : Entity_ComNode
    {
        public override string Title { get => "跟随相机组件"; set => base.Title = value; }
        public override string Tooltip { get => "跟随相机组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(FollowCameraCom);

        public override BaseCom CreateRuntimeNode()
        {
            FollowCameraCom followCom = new FollowCameraCom();
            return followCom;
        }
    }

    [NodeMenuItem("全局/输入/输入组件")]
    public class Entity_Node_InputCom : Entity_ComNode
    {
        public override string Title { get => "输入组件"; set => base.Title = value; }
        public override string Tooltip { get => "输入组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(InputCom);

        public override BaseCom CreateRuntimeNode()
        {
            InputCom com = new InputCom();
            return com;
        }
    }

    [NodeMenuItem("全局/计时器组件")]
    public class Entity_Node_TimerCom : Entity_ComNode
    {
        public override string Title { get => "计时器组件"; set => base.Title = value; }
        public override string Tooltip { get => "计时器组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(TimerCom);

        public override BaseCom CreateRuntimeNode()
        {
            TimerCom com = new TimerCom();
            return com;
        }
    }

    [NodeMenuItem("全局/昼夜组件")]
    public class Entity_Node_DayNightCom : Entity_ComNode
    {
        public override string Title { get => "昼夜组件"; set => base.Title = value; }
        public override string Tooltip { get => "昼夜组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(DayNightCom);

        [NodeValue("昼夜阶段信息")]
        public List<DayNightStageInfo> stageInfos = new List<DayNightStageInfo>();

        public override BaseCom CreateRuntimeNode()
        {
            DayNightCom com = new DayNightCom();
            com.stageInfos = stageInfos;
            return com;
        }
    }

    #endregion
}