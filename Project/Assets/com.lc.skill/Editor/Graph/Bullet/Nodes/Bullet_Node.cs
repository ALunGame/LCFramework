using LCNode;
using LCNode.Model;
using LCToolkit;
using System.Collections.Generic;

namespace LCSkill.BulletGraph
{
    public class BulletMoveFuncData { }

    /// <summary>
    /// 子弹移动函数
    /// </summary>
    public abstract class Bullet_MoveFuncNode : BaseNode
    {
        public override string Tooltip { get => "子弹移动函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public BulletMoveFuncData parentNode;

        public abstract BulletMoveFunc CreateFunc();
    }

    public class BulletCatchFuncData { }

    /// <summary>
    /// 子弹释放时寻找目标函数
    /// </summary>
    public abstract class Bullet_CatchFuncNode : BaseNode
    {
        public override string Tooltip { get => "释放时寻找目标函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public BulletMoveFuncData parentNode;

        public abstract BulletCatchActorFunc CreateFunc();
    }

    public class BulletLifeCycleFuncData { }

    /// <summary>
    /// Bullet生命周期函数
    /// </summary>
    public abstract class Bullet_LifeCycleFuncNode : BaseNode
    {
        public override string Tooltip { get => "Bullet生命周期函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public BulletLifeCycleFuncData parentNode;

        public abstract BulletLifeCycleFunc CreateFunc();
    }

    public class BulletHitFuncData { }

    /// <summary>
    /// 命中目标调用
    /// </summary>
    public abstract class Bullet_HitFuncNode : BaseNode
    {
        public override string Tooltip { get => "命中目标函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public BulletHitFuncData parentNode;

        public abstract BulletHitFunc CreateFunc();
    }

    public class Bullet_Node : BaseNode
    {
        [NodeValue("子弹预制体")]
        public UnityObjectAsset asset = new UnityObjectAsset();

        [NodeValue("子弹尺寸")]
        public float radius;

        [NodeValue("子弹可以命中次数")]
        public int hitTimes = 1;

        [NodeValue("子弹命中同一目标延迟")]
        public float sameTargetDelay = 0.02f;

        [NodeValue("子弹碰到障碍移除")]
        public bool removeOnObstacle = false;

        [NodeValue("子弹击中敌人")]
        public bool hitEnemy = true;

        [NodeValue("子弹击中友军")]
        public bool hitFriend = false;

        [OutputPort("移动函数", BasePort.Capacity.Single)]
        public BulletMoveFuncData moveFunc;

        [OutputPort("捕捉目标函数", BasePort.Capacity.Single)]
        public BulletMoveFuncData catchFunc;

        [OutputPort("当创建时", BasePort.Capacity.Multi)]
        public BulletLifeCycleFuncData onCreateFunc;

        [OutputPort("当被移除时", BasePort.Capacity.Multi)]
        public BulletLifeCycleFuncData onRemovedFunc;

        [OutputPort("当击中时", BasePort.Capacity.Multi)]
        public BulletHitFuncData onHitFunc;

        public BulletMoveFunc GetMoveFunc()
        {
            BulletMoveFunc func = null;
            //组件节点
            List<Bullet_MoveFuncNode> nodes = NodeHelper.GetNodeOutNodes<Bullet_MoveFuncNode>(Owner, this);
            if (nodes.Count > 0)
            {
                func = nodes[0].CreateFunc();
            }
            return func;
        }

        public BulletCatchActorFunc GetCatchFunc()
        {
            BulletCatchActorFunc func = null;
            //组件节点
            List<Bullet_CatchFuncNode> nodes = NodeHelper.GetNodeOutNodes<Bullet_CatchFuncNode>(Owner, this);
            if (nodes.Count > 0)
            {
                func = nodes[0].CreateFunc();
            }
            return func;
        }

        #region 生命周期

        public List<BulletLifeCycleFunc> GetOnCreateFuncs()
        {
            List<BulletLifeCycleFunc> funcs = new List<BulletLifeCycleFunc>();
            //组件节点
            List<Bullet_LifeCycleFuncNode> nodes = NodeHelper.GetNodeOutNodes<Bullet_LifeCycleFuncNode>(Owner, this, "当添加时");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }

        public List<BulletLifeCycleFunc> GetOnRemovedFuncs()
        {
            List<BulletLifeCycleFunc> funcs = new List<BulletLifeCycleFunc>();
            //组件节点
            List<Bullet_LifeCycleFuncNode> nodes = NodeHelper.GetNodeOutNodes<Bullet_LifeCycleFuncNode>(Owner, this, "当被移除时");
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

        public List<BulletHitFunc> GetOnHitFuncs()
        {
            List<BulletHitFunc> funcs = new List<BulletHitFunc>();
            //组件节点
            List<Bullet_HitFuncNode> nodes = NodeHelper.GetNodeOutNodes<Bullet_HitFuncNode>(Owner, this);
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].CreateFunc());
                }
            }
            return funcs;
        }
    }
}
