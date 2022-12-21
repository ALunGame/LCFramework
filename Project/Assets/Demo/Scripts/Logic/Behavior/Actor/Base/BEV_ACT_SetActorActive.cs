using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCMap;

namespace Demo.Behavior
{
    /// <summary>
    /// 设置演员显示隐藏
    /// </summary>
    public class BEV_ACT_SetActorActive : NodeAction
    {
        public bool isActive = false;
        public string stateName = "";

        protected override void OnEnter(NodeData wData)
        {
            Actor actor = LCMap.MapLocate.Map.GetActor(wData.Uid);
            actor.IsActive = isActive;
            if (!string.IsNullOrEmpty(stateName))
                actor.DisplayCom.SetState(stateName);
        }

        protected override int OnRunning(NodeData wData)
        {
            return NodeState.FINISHED;
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
        }
    }
}
