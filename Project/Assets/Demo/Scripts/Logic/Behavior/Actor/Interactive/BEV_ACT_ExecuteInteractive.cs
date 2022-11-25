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
            public string executingKey;
        }

        //交互类型名
        public InteractiveType interactiveType;

        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            //获取环境数据
            NodeActionContext context = GetContext<NodeActionContext>(wData);
            ExecuteInteractiveData userData = context.GetUserData<ExecuteInteractiveData>();
            
            //当前演员
            Actor actorObj = MapLocate.Map.GetActor(workData.MEntity.Uid);
            //目标演员
            Actor targetActor = MapLocate.Map.GetActor(wData.Blackboard[BEV_BlackboardKey.InteractiveActorUid].ToString());
            targetActor.ExecuteInteractive(actorObj, interactiveType);

            //保存
            userData.executingKey = targetActor.InteractivingKey();

            GameLocate.Log.LogR("开始执行交互", wData.Uid, userData.executingKey);

        }

        protected override int OnRunning(NodeData wData)
        {
            //获取环境数据
            NodeActionContext context = GetContext<NodeActionContext>(wData);
            ExecuteInteractiveData userData = context.GetUserData<ExecuteInteractiveData>();

            //目标演员
            Actor targetActor = LCMap.MapLocate.Map.GetActor(wData.Blackboard[BEV_BlackboardKey.InteractiveActorUid].ToString());

            if (!targetActor.Interactiving())
                return NodeState.FINISHED;
            if (targetActor.InteractivingKey() == userData.executingKey)
                return NodeState.EXECUTING;
            GameLocate.Log.LogR("执行交互完成", wData.Uid, userData.executingKey);
            return NodeState.FINISHED;
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
            //获取环境数据
            NodeActionContext context = GetContext<NodeActionContext>(wData);
            ExecuteInteractiveData userData = context.GetUserData<ExecuteInteractiveData>();
            GameLocate.Log.LogR("执行交互结束", wData.Uid, userData.executingKey);
            userData.executingKey = "";
        }
    }
}
