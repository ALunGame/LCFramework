using LCNode;
using LCNode.Model;
using System.Collections.Generic;

namespace LCSkill.BuffGraph
{
    public class BuffFreedFuncData { }

    /// <summary>
    /// 当释放一个技能时执行的函数节点
    /// </summary>
    public abstract class Buff_FreedFuncNode : BaseNode
    {
        public override string Tooltip { get => "当释放一个技能时执行的函数节点"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public BuffFreedFuncData parentNode;

        public abstract BuffOnFreedFunc CreateFunc();
    }

    public class BuffLifeCycleFuncData { }

    /// <summary>
    /// Buff生命周期函数
    /// </summary>
    public abstract class Buff_LifeCycleFuncNode : BaseNode
    {
        public override string Tooltip { get => "Buff生命周期函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public BuffLifeCycleFuncData parentNode;

        public abstract BuffLifeCycleFunc CreateFunc();
    }

    #region 伤害流程

    public class BuffHurtFuncData { }

    /// <summary>
    /// 在执行伤害流程时，拥有这个Buff作为攻击者执行的函数
    /// </summary>
    public abstract class Buff_HurtFuncNode : BaseNode
    {
        public override string Tooltip { get => "在执行伤害流程时，拥有这个Buff作为攻击者执行的函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public BuffHurtFuncData parentNode;

        public abstract BuffHurtFunc CreateFunc();
    }

    public class BuffBeHurtFuncData { }

    /// <summary>
    /// 在执行伤害流程时，拥有这个Buff被攻击执行的函数
    /// </summary>
    public abstract class Buff_BeHurtFuncNode : BaseNode
    {
        public override string Tooltip { get => "在执行伤害流程时，拥有这个Buff被攻击执行的函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public BuffBeHurtFuncData parentNode;

        public abstract BuffBeHurtFunc CreateFunc();
    }

    public class BuffKilledFuncData { }

    /// <summary>
    /// 在执行伤害流程时，如果击杀目标执行的函数
    /// </summary>
    public abstract class Buff_KilledFuncNode : BaseNode
    {
        public override string Tooltip { get => "在执行伤害流程时，如果击杀目标执行的函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public BuffKilledFuncData parentNode;

        public abstract BuffKilledFunc CreateFunc();
    }

    public class BuffBeKilledFuncData { }

    /// <summary>
    /// 在执行伤害流程时，拥有这个Buff被杀死执行的函数
    /// </summary>
    public abstract class Buff_BeKilledFuncNode : BaseNode
    {
        public override string Tooltip { get => "在执行伤害流程时，拥有这个Buff被杀死执行的函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public BuffBeKilledFuncData parentNode;

        public abstract BuffBeKilledFunc CreateFunc();
    }

    #endregion

    public class Buff_Node : BaseNode
    {
        [NodeValue("TickTime间隔")]
        public float tickTime = -1;

        [OutputPort("当释放技能时", BasePort.Capacity.Single)]
        public BuffFreedFuncData onFreedFunc;

        [OutputPort("当添加时", BasePort.Capacity.Multi)]
        public BuffLifeCycleFuncData onOccurFunc;

        [OutputPort("在TickTime间隔执行", BasePort.Capacity.Multi)]
        public BuffLifeCycleFuncData onTickFunc;

        [OutputPort("当被移除时", BasePort.Capacity.Multi)]
        public BuffLifeCycleFuncData onRemovedFunc;

        #region 伤害流程

        [OutputPort("当攻击时", BasePort.Capacity.Multi)]
        public BuffHurtFuncData onHurtFunc;

        [OutputPort("当被攻击时", BasePort.Capacity.Multi)]
        public BuffBeHurtFuncData onBeHurtFunc;

        [OutputPort("当击杀时", BasePort.Capacity.Multi)]
        public BuffKilledFuncData onKilledFunc;

        [OutputPort("当被击杀时", BasePort.Capacity.Multi)]
        public BuffBeKilledFuncData onBeKilledFunc;

        #endregion

        public BuffOnFreedFunc GetOnFreedFuncs()
        {
            BuffOnFreedFunc func = null;
            //组件节点
            List<Buff_FreedFuncNode> nodes = NodeHelper.GetNodeOutNodes<Buff_FreedFuncNode>(Owner, this);
            if (nodes.Count > 0)
            {
                func = nodes[0].CreateFunc();
            }
            return func;
        }

        #region 生命周期

        public List<BuffLifeCycleFunc> GetOnOccurFuncs()
        {
            List<BuffLifeCycleFunc> funcs = new List<BuffLifeCycleFunc>();
            //组件节点
            List<Buff_LifeCycleFuncNode> nodes = NodeHelper.GetNodeOutNodes<Buff_LifeCycleFuncNode>(Owner, this, "当添加时");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        public List<BuffLifeCycleFunc> GetOnTickFuncs()
        {
            List<BuffLifeCycleFunc> funcs = new List<BuffLifeCycleFunc>();
            //组件节点
            List<Buff_LifeCycleFuncNode> nodes = NodeHelper.GetNodeOutNodes<Buff_LifeCycleFuncNode>(Owner, this, "在TickTime间隔执行");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        public List<BuffLifeCycleFunc> GetOnRemovedFuncs()
        {
            List<BuffLifeCycleFunc> funcs = new List<BuffLifeCycleFunc>();
            //组件节点
            List<Buff_LifeCycleFuncNode> nodes = NodeHelper.GetNodeOutNodes<Buff_LifeCycleFuncNode>(Owner, this, "当被移除时");
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

        #region 伤害流程

        public List<BuffHurtFunc> GetOnHurtFuncs()
        {
            List<BuffHurtFunc> funcs = new List<BuffHurtFunc>();
            //组件节点
            List<Buff_HurtFuncNode> nodes = NodeHelper.GetNodeOutNodes<Buff_HurtFuncNode>(Owner, this);
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        public List<BuffBeHurtFunc> GetOnBeHurtFuncs()
        {
            List<BuffBeHurtFunc> funcs = new List<BuffBeHurtFunc>();
            //组件节点
            List<Buff_BeHurtFuncNode> nodes = NodeHelper.GetNodeOutNodes<Buff_BeHurtFuncNode>(Owner, this);
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        public List<BuffKilledFunc> GetOnKilledFuncs()
        {
            List<BuffKilledFunc> funcs = new List<BuffKilledFunc>();
            //组件节点
            List<Buff_KilledFuncNode> nodes = NodeHelper.GetNodeOutNodes<Buff_KilledFuncNode>(Owner, this);
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        public List<BuffBeKilledFunc> GetOnBeKilledFuncs()
        {
            List<BuffBeKilledFunc> funcs = new List<BuffBeKilledFunc>();
            //组件节点
            List<Buff_BeKilledFuncNode> nodes = NodeHelper.GetNodeOutNodes<Buff_BeKilledFuncNode>(Owner, this);
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
