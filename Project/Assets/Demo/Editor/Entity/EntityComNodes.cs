using Demo.Com;
using LCECS.Core;
using LCECS.EntityGraph;
using LCNode;
using System;
using System.Collections.Generic;
using Cnf;
using Demo.Com.MainActor;
using Demo.Com.MainActor.NewMove;

namespace Demo
{
    #region 演员

    #region 基础

    [NodeMenuItem("演员/基础/属性组件")]
    public class Entity_Actor_BasePropertyCom : Entity_ComNode
    {
        public override string Title { get => "基础属性组件"; set => base.Title = value; }
        public override string Tooltip { get => "基础属性组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(BasePropertyCom);

        public override BaseCom CreateRuntimeNode()
        {
            BasePropertyCom propertyCom = new BasePropertyCom();
            return propertyCom;
        }
    }

    [NodeMenuItem("演员/基础/动画组件")]
    public class Entity_Actor_AnimCom : Entity_ComNode
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

    [NodeMenuItem("演员/基础/背包组件")]
    public class Entity_Actor_BagCom : Entity_ComNode
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

    [NodeMenuItem("演员/基础/气泡组件")]
    public class Entity_Actor_BubbleCom : Entity_ComNode
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

    [NodeMenuItem("演员/基础/阻挡组件")]
    public class Entity_Actor_Collider2DCom : Entity_ComNode
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

    [NodeMenuItem("演员/基础/移动组件")]
    public class Entity_Actor_MoveCom : Entity_ComNode
    {
        public override string Title { get => "移动组件"; set => base.Title = value; }
        public override string Tooltip { get => "移动组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(MoveCom);

        [NodeValue("质量")]
        public float Mass = 1;

        public override BaseCom CreateRuntimeNode()
        {
            MoveCom com = new MoveCom();
            com.Mass = Mass;
            return com;
        }
    }
    
    [NodeMenuItem("演员/基础/移动请求组件")]
    public class Entity_Actor_MoveRequestCom : Entity_ComNode
    {
        public override string Title { get => "移动请求组件"; set => base.Title = value; }
        public override string Tooltip { get => "移动请求组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(MoveRequestCom);
        
        public override BaseCom CreateRuntimeNode()
        {
            MoveRequestCom com = new MoveRequestCom();
            return com;
        }
    }

    [NodeMenuItem("演员/基础/重力组件")]
    public class Entity_Actor_GravityCom : Entity_ComNode
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

    #endregion

    #region 玩家

    [NodeMenuItem("演员/玩家/玩家组件")]
    public class Entity_Actor_PlayerCom : Entity_ComNode
    {
        public override string Title { get => "玩家组件"; set => base.Title = value; }
        public override string Tooltip { get => "玩家组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(PlayerCom);

        public override BaseCom CreateRuntimeNode()
        {
            PlayerCom com = new PlayerCom();
            return com;
        }
    }
    
    [NodeMenuItem("演员/玩家/玩家移动组件")]
    public class Entity_Actor_MainActorMoveCom : Entity_ComNode
    {
        public override string Title { get => "玩家移动组件"; set => base.Title = value; }
        public override string Tooltip { get => "玩家移动组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(MainActorMoveCom);

        [NodeValue("移动速度")]
        public float MoveSpeed = 12;
        [NodeValue("爬墙速度")]
        public float ClimbSpeed = 6;
        
        [NodeValue("一段跳最大距离")]
        public float JumpDis = 1;
        [NodeValue("一段跳跳跃速度")]
        public float JumpSpeed = 18;
        
        [NodeValue("可以二段跳最小距离")]
        public float JumpSecondCanDis = 0.2f;			   
        [NodeValue("二段跳最大距离")]
        public float JumpSecondDis = 0.5f;            
        [NodeValue("二段跳跳跃速度")]
        public float JumpSecondSpeed = 20;
        
        public override BaseCom CreateRuntimeNode()
        {
            MainActorMoveCom com = new MainActorMoveCom();
            com.MoveSpeed = MoveSpeed;
            com.ClimbSpeed = ClimbSpeed;
            com.JumpDis = JumpDis;
            com.JumpSpeed = JumpSpeed;
            com.JumpSecondCanDis = JumpSecondCanDis;
            com.JumpSecondDis = JumpSecondDis;
            com.JumpSecondSpeed = JumpSecondSpeed;
            return com;
        }
    }
    
    [NodeMenuItem("演员/玩家/新版玩家移动组件")]
    public class Entity_Actor_NewMainActorMoveCom : Entity_ComNode
    {
        public override string Title { get => "新版玩家移动组件"; set => base.Title = value; }
        public override string Tooltip { get => "新版玩家移动组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(NewMainActorMoveCom);
        
        public override BaseCom CreateRuntimeNode()
        {
            NewMainActorMoveCom com = new NewMainActorMoveCom();
            return com;
        }
    }
    
    #endregion

    #region 其他

    [NodeMenuItem("演员/其他/建筑组件")]
    public class Entity_Actor_BuildingCom : Entity_ComNode
    {
        public override string Title { get => "建筑组件"; set => base.Title = value; }
        public override string Tooltip { get => "建筑组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(BuildingCom);

        [NodeValue("建筑类型")]
        public BuildingType buildingType;

        public override BaseCom CreateRuntimeNode()
        {
            BuildingCom com = new BuildingCom();
            com.buildingType = buildingType;
            return com;
        }
    }

    [NodeMenuItem("演员/其他/阵营组件")]
    public class Entity_Actor_CampCom : Entity_ComNode
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

    #endregion

    #region AI

    [NodeMenuItem("演员/AI/徘徊组件")]
    public class Entity_Actor_WanderCom : Entity_ComNode
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
    public class Entity_Actor_GazeSurroundCom : Entity_ComNode
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

    #endregion

    #region 工作

    // [NodeMenuItem("演员/工作/工人组件")]
    // public class Entity_Actor_WorkerCom : Entity_ComNode
    // {
    //     public override string Title { get => "工人组件"; set => base.Title = value; }
    //     public override string Tooltip { get => "工人组件"; set => base.Tooltip = value; }
    //     public override Type RuntimeNode => typeof(WorkerCom);
    //
    //     public override BaseCom CreateRuntimeNode()
    //     {
    //         WorkerCom com = new WorkerCom();
    //         return com;
    //     }
    // }
    //
    // [NodeMenuItem("演员/工作/管理者组件")]
    // public class Entity_Actor_ManagerCom : Entity_ComNode
    // {
    //     public override string Title { get => "管理者组件"; set => base.Title = value; }
    //     public override string Tooltip { get => "管理者组件"; set => base.Tooltip = value; }
    //     public override Type RuntimeNode => typeof(ManagerCom);
    //
    //     public override BaseCom CreateRuntimeNode()
    //     {
    //         ManagerCom com = new ManagerCom();
    //         return com;
    //     }
    // }
    //
    // [NodeMenuItem("演员/工作/产出物品组件")]
    // public class Entity_Actor_OutputItemCom : Entity_ComNode
    // {
    //     public override string Title { get => "产出物品组件"; set => base.Title = value; }
    //     public override string Tooltip { get => "产出物品组件"; set => base.Tooltip = value; }
    //     public override Type RuntimeNode => typeof(OutputItemCom);
    //
    //     [NodeValue("产出信息")]
    //     public List<ItemInfo> outputInfos = new List<ItemInfo>();
    //
    //     public override BaseCom CreateRuntimeNode()
    //     {
    //         OutputItemCom com = new OutputItemCom();
    //         com.outputInfos = outputInfos;
    //         return com;
    //     }
    // }
    //
    // [NodeMenuItem("演员/工作/生产物品组件")]
    // public class Entity_Actor_ProduceItemCom : Entity_ComNode
    // {
    //     public override string Title { get => "生产物品组件"; set => base.Title = value; }
    //     public override string Tooltip { get => "生产物品组件"; set => base.Tooltip = value; }
    //     public override Type RuntimeNode => typeof(ProduceItemCom);
    //
    //     [NodeValue("可以生产的物品Id")]
    //     public List<int> produceItems = new List<int>();
    //
    //     public override BaseCom CreateRuntimeNode()
    //     {
    //         ProduceItemCom com = new ProduceItemCom();
    //         com.produceItems = produceItems;
    //         return com;
    //     }
    // }
    //
    // [NodeMenuItem("演员/工作/修理物品组件")]
    // public class Entity_Actor_RepairItemCom : Entity_ComNode
    // {
    //     public override string Title { get => "生产物品组件"; set => base.Title = value; }
    //     public override string Tooltip { get => "生产物品组件"; set => base.Tooltip = value; }
    //     public override Type RuntimeNode => typeof(RepairItemCom);
    //
    //     [NodeValue("可以修复的物品Id")]
    //     public List<int> items = new List<int>();
    //
    //     public override BaseCom CreateRuntimeNode()
    //     {
    //         RepairItemCom com = new RepairItemCom();
    //         com.itemIds = items;
    //         return com;
    //     }
    // }

    #endregion

    #region 交互

    [NodeMenuItem("演员/交互/武器组件")]
    public class Entity_Actor_WeaponCom : Entity_ComNode
    {
        public override string Title { get => "武器组件"; set => base.Title = value; }
        public override string Tooltip { get => "武器组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(WeaponCom);

        public override BaseCom CreateRuntimeNode()
        {
            WeaponCom com = new WeaponCom();
            return com;
        }
    }

    #endregion

    #endregion

    #region 全局

    [NodeMenuItem("全局/相机/拖动相机")]
    public class Entity_Global_DragCameraCom : Entity_ComNode
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
    public class Entity_Global_FollowCameraCom : Entity_ComNode
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
    public class Entity_Global_InputCom : Entity_ComNode
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
    
    [NodeMenuItem("全局/输入/玩家输入组件")]
    public class Entity_Global_MainActorInputCom : Entity_ComNode
    {
        public override string Title { get => "玩家输入组件"; set => base.Title = value; }
        public override string Tooltip { get => "玩家输入组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(MainActorInputCom);

        public override BaseCom CreateRuntimeNode()
        {
            MainActorInputCom com = new MainActorInputCom();
            return com;
        }
    }

    [NodeMenuItem("全局/基础/计时器组件")]
    public class Entity_Global_TimerCom : Entity_ComNode
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

    [NodeMenuItem("全局/天气/昼夜组件")]
    public class Entity_Global_DayNightCom : Entity_ComNode
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