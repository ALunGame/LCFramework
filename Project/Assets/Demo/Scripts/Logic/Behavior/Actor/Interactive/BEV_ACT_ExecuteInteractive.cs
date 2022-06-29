using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using LCMap;
using System;

namespace Demo.Behavior
{
    /// <summary>
    /// 请求执行交互
    /// </summary>
    public class BEV_ACT_ExecuteInteractive : NodeAction
    {
        class ExecuteInteractiveData
        {
            public string currExecuteTypeName;
        }

        //交互类型名
        public string interactiveTypeName = "";

        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            //获取环境数据
            NodeActionContext context = GetContext<NodeActionContext>(wData);
            ExecuteInteractiveData userData = context.GetUserData<ExecuteInteractiveData>();
            userData.currExecuteTypeName = interactiveTypeName;

            //当前演员
            ActorObj actorObj = LCMap.MapLocate.Map.GetActor(workData.MEntity.Uid);
            //目标演员
            ActorObj targetActor = LCMap.MapLocate.Map.GetActor(wData.Blackboard[BEV_BlackboardKey.InteractiveActorUid].ToString());
            targetActor.ExecuteInteractive(Type.GetType(interactiveTypeName), actorObj);
        }

        protected override int OnRunning(NodeData wData)
        {
            //获取环境数据
            NodeActionContext context = GetContext<NodeActionContext>(wData);
            ExecuteInteractiveData userData = context.GetUserData<ExecuteInteractiveData>();

            //目标演员
            ActorObj targetActor = LCMap.MapLocate.Map.GetActor(wData.Blackboard[BEV_BlackboardKey.InteractiveActorUid].ToString());

            if (targetActor.CurrInteractive == null)
                return NodeState.FINISHED;
            if (targetActor.CurrInteractive.GetType().Name == userData.currExecuteTypeName)
                return NodeState.EXECUTING;
            return NodeState.FINISHED;
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
            //获取环境数据
            NodeActionContext context = GetContext<NodeActionContext>(wData);
            ExecuteInteractiveData userData = context.GetUserData<ExecuteInteractiveData>();
            userData.currExecuteTypeName = "";
        }
    }
}
