using LCNode;
using LCNode.Model;
using LCToolkit;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill.AoeGraph
{
    public class AoeMoveFuncData { }

    /// <summary>
    /// Aoe移动函数
    /// </summary>
    public abstract class Aoe_MoveFuncNode : BaseNode
    {
        public override string Tooltip { get => "Aoe移动函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public AoeMoveFuncData parentNode;

        public abstract AoeMoveFunc CreateFunc();
    }

    public class AoeLifeCycleFuncData { }

    /// <summary>
    /// Aoe生命周期函数
    /// </summary>
    public abstract class Aoe_LifeCycleFuncNode : BaseNode
    {
        public override string Tooltip { get => "Aoe生命周期函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public AoeLifeCycleFuncData parentNode;

        public abstract AoeLifeCycleFunc CreateFunc();
    }

    #region 进入离开

    public class AoeActorEnterFuncData { }

    /// <summary>
    /// 新的演员进入函数
    /// </summary>
    public abstract class Aoe_ActorEnterFuncNode : BaseNode
    {
        public override string Tooltip { get => "新的演员进入函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public AoeActorEnterFuncData parentNode;

        public abstract AoeActorEnter CreateFunc();
    }

    public class AoeActorLeaveFuncData { }

    /// <summary>
    /// 演员离开函数
    /// </summary>
    public abstract class Aoe_ActorLeaveFuncNode : BaseNode
    {
        public override string Tooltip { get => "演员离开函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public AoeActorLeaveFuncData parentNode;

        public abstract AoeActorLeave CreateFunc();
    }

    public class AoeBulletEnterFuncData { }

    /// <summary>
    /// 子弹进入调用
    /// </summary>
    public abstract class Aoe_BulletEnterFuncNode : BaseNode
    {
        public override string Tooltip { get => "子弹进入函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public AoeBulletEnterFuncData parentNode;

        public abstract AoeBulletEnter CreateFunc();
    }

    public class AoeBulletLeaveFuncData { }

    /// <summary>
    /// 子弹离开调用
    /// </summary>
    public abstract class Aoe_BulletLeaveFuncNode : BaseNode
    {
        public override string Tooltip { get => "子弹离开函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public AoeBulletLeaveFuncData parentNode;

        public abstract AoeBulletLeave CreateFunc();
    }

    #endregion

    public class Aoe_Node : BaseNode
    {
        [NodeValue("预制体")]
        public UnityObjectAsset asset = new UnityObjectAsset();

        [NodeValue("区域形状")]
        public Shape areaShape = new Shape();

        [NodeValue("TickTime间隔")]
        public float tickTime = -1;

        [OutputPort("移动函数", BasePort.Capacity.Single)]
        public AoeMoveFuncData moveFunc;

        [OutputPort("当创建时", BasePort.Capacity.Multi)]
        public AoeLifeCycleFuncData onCreateFunc;

        [OutputPort("在TickTime间隔执行", BasePort.Capacity.Multi)]
        public AoeLifeCycleFuncData onTickFunc;

        [OutputPort("当被移除时", BasePort.Capacity.Multi)]
        public AoeLifeCycleFuncData onRemovedFunc;

        #region 进入离开

        [OutputPort("当演员进入时", BasePort.Capacity.Multi)]
        public AoeActorEnterFuncData onActorEnterFunc;

        [OutputPort("当演员离开时", BasePort.Capacity.Multi)]
        public AoeActorLeaveFuncData onActorLeaveFunc;

        [OutputPort("当子弹进入时", BasePort.Capacity.Multi)]
        public AoeBulletEnterFuncData onBulletEnterFunc;

        [OutputPort("当子弹离开时", BasePort.Capacity.Multi)]
        public AoeBulletLeaveFuncData onBulletLeaveFunc;

        #endregion

        public AoeMoveFunc GetMoveFunc()
        {
            AoeMoveFunc func = null;
            //组件节点
            List<Aoe_MoveFuncNode> nodes = NodeHelper.GetNodeOutNodes<Aoe_MoveFuncNode>(Owner, this);
            if (nodes.Count > 0)
            {
                func = nodes[0].CreateFunc();
            }
            return func;
        }

        #region 生命周期

        public List<AoeLifeCycleFunc> GetOnCreateFuncs()
        {
            List<AoeLifeCycleFunc> funcs = new List<AoeLifeCycleFunc>();
            //组件节点
            List<Aoe_LifeCycleFuncNode> nodes = NodeHelper.GetNodeOutNodes<Aoe_LifeCycleFuncNode>(Owner, this, "当添加时");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        public List<AoeLifeCycleFunc> GetOnTickFuncs()
        {
            List<AoeLifeCycleFunc> funcs = new List<AoeLifeCycleFunc>();
            //组件节点
            List<Aoe_LifeCycleFuncNode> nodes = NodeHelper.GetNodeOutNodes<Aoe_LifeCycleFuncNode>(Owner, this, "在TickTime间隔执行");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        public List<AoeLifeCycleFunc> GetOnRemovedFuncs()
        {
            List<AoeLifeCycleFunc> funcs = new List<AoeLifeCycleFunc>();
            //组件节点
            List<Aoe_LifeCycleFuncNode> nodes = NodeHelper.GetNodeOutNodes<Aoe_LifeCycleFuncNode>(Owner, this, "当被移除时");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        #endregion

        #region 进入离开

        public List<AoeActorEnter> GetOnActorEnterFuncs()
        {
            List<AoeActorEnter> funcs = new List<AoeActorEnter>();
            //组件节点
            List<Aoe_ActorEnterFuncNode> nodes = NodeHelper.GetNodeOutNodes<Aoe_ActorEnterFuncNode>(Owner, this);
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        public List<AoeActorLeave> GetOnActorLeaveFuncs()
        {
            List<AoeActorLeave> funcs = new List<AoeActorLeave>();
            //组件节点
            List<Aoe_ActorLeaveFuncNode> nodes = NodeHelper.GetNodeOutNodes<Aoe_ActorLeaveFuncNode>(Owner, this);
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        public List<AoeBulletEnter> GetOnBulletEnterFuncs()
        {
            List<AoeBulletEnter> funcs = new List<AoeBulletEnter>();
            //组件节点
            List<Aoe_BulletEnterFuncNode> nodes = NodeHelper.GetNodeOutNodes<Aoe_BulletEnterFuncNode>(Owner, this);
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        public List<AoeBulletLeave> GetOnBulletLeaveFuncs()
        {
            List<AoeBulletLeave> funcs = new List<AoeBulletLeave>();
            //组件节点
            List<Aoe_BulletLeaveFuncNode> nodes = NodeHelper.GetNodeOutNodes<Aoe_BulletLeaveFuncNode>(Owner, this);
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        #endregion
    }
}
