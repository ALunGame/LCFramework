using LCMap;
using LCNode;
using LCNode.Model;
using System.Collections.Generic;

namespace LCTask.TaskGraph
{
    #region 任务条件函数

    public class Task_ConditionFuncData { }
    /// <summary>
    /// 任务条件函数
    /// </summary>
    public abstract class Task_ConditionFuncNode : BaseNode
    {
        public override string Title { get => "任务条件函数"; set => base.Title = value; }
        public override string Tooltip { get => "任务条件函数"; set => base.Tooltip = value; }

        [OutputPort("父节点", BasePort.Capacity.Single)]
        public Task_ConditionFuncData parentNode;

        [NodeValue("条件值")]
        public bool checkValue = true;

        [NodeValue("与下一个条件关系")]
        public ConditionType conditionType = ConditionType.AND;

        public TaskConditionFunc GetFunc()
        {
            TaskConditionFunc func  = CreateFunc();
            func.checkValue         = checkValue;
            func.conditionType      = conditionType;
            return func;
        }

        public abstract TaskConditionFunc CreateFunc();
    }

    #endregion

    #region 任务目标表现函数

    public class Task_DisplayFuncData { }
    /// <summary>
    /// 任务目标表现函数
    /// </summary>
    public abstract class Task_TargetDisplayFuncNode : BaseNode
    {
        public override string Title { get => "任务表现函数"; set => base.Title = value; }
        public override string Tooltip { get => "任务表现函数"; set => base.Tooltip = value; }

        [OutputPort("父节点", BasePort.Capacity.Single)]
        public Task_DisplayFuncData parentNode;

        public TaskTargetDisplayFunc GetFunc()
        {
            TaskTargetDisplayFunc func = CreateFunc();
            return func;
        }

        public abstract TaskTargetDisplayFunc CreateFunc();
    }

    #endregion

    #region 任务监听函数

    public class Task_ListenFuncData { }
    /// <summary>
    /// 任务监听函数
    /// </summary>
    public abstract class Task_ListenFuncNode : Map_ActorNode
    {
        [OutputPort("父节点", BasePort.Capacity.Single)]
        public Task_ListenFuncData parentNode;

        public TaskListenFunc GetFunc()
        {
            TaskListenFunc func = CreateFunc();
            return func;
        }

        public abstract TaskListenFunc CreateFunc();
    }

    #endregion

    #region 任务行为函数

    public class Task_ActionFuncData { }
    /// <summary>
    /// 任务通用行为
    /// </summary>
    public abstract class Task_ActionFuncNode : BaseNode
    {
        public TaskActionFunc GetFunc()
        {
            TaskActionFunc func = CreateFunc();
            return func;
        }

        public abstract TaskActionFunc CreateFunc();
    }

    /// <summary>
    /// 任务通用行为
    /// </summary>
    public abstract class Task_CommonActionFuncNode : Task_ActionFuncNode
    {
        [InputPort("父节点", BasePort.Capacity.Single)]
        public Task_ActionFuncData parentNode;
    }

    public class Task_SuccessActionFuncData : Task_ActionFuncData { }

    /// <summary>
    /// 任务阶段行为执行成功行为
    /// 1，主要用于获得道具这种
    /// </summary>
    public abstract class Task_SuccessActionFuncNode : Task_ActionFuncNode
    {
        [InputPort("父节点", BasePort.Capacity.Single)]
        public Task_SuccessActionFuncData parentNode;
    }

    public class Task_AcceptActionFuncData : Task_ActionFuncData { }
    /// <summary>
    /// 任务接受行为函数
    /// 1，用于只能在接受阶段执行的行为
    /// </summary>
    public abstract class Task_AcceptActionFuncNode : Task_ActionFuncNode
    {
        [InputPort("父节点", BasePort.Capacity.Single)]
        public Task_AcceptActionFuncData parentNode;
    }

    public class Task_ExecuteActionFuncData : Task_ActionFuncData { }
    /// <summary>
    /// 任务提交行为函数
    /// 1，用于只能在提交阶段执行的行为
    /// </summary>
    public abstract class Task_ExecuteActionFuncNode : Task_ActionFuncNode
    {
        [InputPort("父节点", BasePort.Capacity.Single)]
        public Task_ExecuteActionFuncData parentNode;
    }

    #endregion

    [NodeMenuItem("任务目标")]
    public class Task_TargetNode : Map_ActorNode
    {
        [OutputPort("父节点", BasePort.Capacity.Single)]
        public MapActorData parentNode;
    }

    public abstract class Task_Node : BaseNode
    {
        public int taskId;

        [InputPort("阶段条件", BasePort.Capacity.Multi)]
        public Task_ConditionFuncData conditionFuncs;

        [InputPort("阶段目标", BasePort.Capacity.Single)]
        public MapActorData target;

        [InputPort("阶段目标表现", BasePort.Capacity.Multi)]
        public Task_DisplayFuncData targetDisplayFuncs;

        public int GetTargetMapId()
        {
            List<Task_TargetNode> targetNodes = NodeHelper.GetNodeOutNodes<Task_TargetNode>(Owner, this, "阶段目标");
            if (targetNodes.Count > 0)
            {
                return (int)targetNodes[0].mapId;
            }
            return 0;
        }

        public List<int> GetTargetActorIds()
        {
            List<Task_TargetNode> targetNodes = NodeHelper.GetNodeOutNodes<Task_TargetNode>(Owner, this, "阶段目标");
            if (targetNodes.Count > 0)
            {
                return targetNodes[0].GetActorIds();
            }
            return null;
        }

        public List<TaskConditionFunc> GetConditionFuncs()
        {
            List<TaskConditionFunc> funcs = new List<TaskConditionFunc>();
            List<Task_ConditionFuncNode> conditionNodes = NodeHelper.GetNodeOutNodes<Task_ConditionFuncNode>(Owner, this, "阶段条件");
            if (conditionNodes.Count > 0)
            {
                for (int i = 0; i < conditionNodes.Count; i++)
                {
                    funcs.Add(conditionNodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public List<TaskTargetDisplayFunc> GetDisplayFuncs()
        {
            List<TaskTargetDisplayFunc> funcs = new List<TaskTargetDisplayFunc>();
            List<Task_TargetDisplayFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_TargetDisplayFuncNode>(Owner, this, "阶段目标表现");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public TaskContent GetContent()
        {
            TaskContent content = CreateContent();
            content.mapId = GetTargetMapId();
            content.actorIds = GetTargetActorIds();
            content.displayFuncs = GetDisplayFuncs();
            content.conditionFuncs = GetConditionFuncs();
            return content;
        }

        public abstract TaskContent CreateContent();
    }

    [NodeMenuItem("任务接受")]
    public class Task_AcceptNode : Task_Node
    {
        public override string Title { get => $"接受{taskId}任务"; set => base.Title = value; }

        [OutputPort("接受行为", BasePort.Capacity.Multi)]
        public Task_AcceptActionFuncData actionFuncs;

        [OutputPort("接受监听", BasePort.Capacity.Multi)]
        public Task_ListenFuncData actionListenFuncs;

        [OutputPort("接受成功", BasePort.Capacity.Multi)]
        public Task_SuccessActionFuncData actionSuccess;

        [OutputPort("接受失败", BasePort.Capacity.Multi)]
        public Task_ActionFuncData actionFail;

        #region 监听

        private List<TaskListenFunc> GetActionListenFuncs()
        {
            List<TaskListenFunc> funcs = new List<TaskListenFunc>();
            List<Task_ListenFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ListenFuncNode>(Owner, this, "接受监听");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        #endregion

        #region 行为

        public List<TaskActionFunc> GetActionFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "接受行为");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public List<TaskActionFunc> GetActionSuccessFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "接受成功");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public List<TaskActionFunc> GetActionFailFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "接受失败");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        #endregion

        public override TaskContent CreateContent()
        {
            TaskContent content = new TaskContent();
            content.actionFuncs = GetActionFuncs();
            content.actionListenFuncs = GetActionListenFuncs();
            content.actionSuccess = GetActionSuccessFuncs();
            content.actionFail = GetActionFailFuncs();
            return content;
        }
    }

    [NodeMenuItem("任务提交")]
    public class Task_ExecuteNode : Task_Node
    {
        public override string Title { get => $"提交{taskId}任务"; set => base.Title = value; }

        [OutputPort("提交行为", BasePort.Capacity.Multi)]
        public Task_ExecuteActionFuncData actionFuncs;

        [OutputPort("提交监听", BasePort.Capacity.Multi)]
        public Task_ListenFuncData actionListenFuncs;

        [OutputPort("提交成功", BasePort.Capacity.Multi)]
        public Task_SuccessActionFuncData actionSuccess;

        [OutputPort("提交失败", BasePort.Capacity.Multi)]
        public Task_ActionFuncData actionFail;


        #region 监听

        private List<TaskListenFunc> GetActionListenFuncs()
        {
            List<TaskListenFunc> funcs = new List<TaskListenFunc>();
            List<Task_ListenFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ListenFuncNode>(Owner, this, "接受监听");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        #endregion

        #region 行为

        public List<TaskActionFunc> GetActionFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "提交行为");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public List<TaskActionFunc> GetActionSuccessFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "提交成功");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public List<TaskActionFunc> GetActionFailFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "提交失败");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        #endregion

        public override TaskContent CreateContent()
        {
            TaskContent content = new TaskContent();
            content.actionFuncs = GetActionFuncs();
            content.actionListenFuncs = GetActionListenFuncs();
            content.actionSuccess = GetActionSuccessFuncs();
            content.actionFail = GetActionFailFuncs();
            return content;
        }
    }
}
