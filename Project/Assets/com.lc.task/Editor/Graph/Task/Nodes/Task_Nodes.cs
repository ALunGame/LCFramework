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

        [InputPort("下一个条件", BasePort.Capacity.Single)]
        public Task_ConditionFuncData nextNode;

        public List<TaskConditionFunc> GetFuncs()
        {
            List<TaskConditionFunc> funcs = new List<TaskConditionFunc>();
            funcs.Add(CreateFunc());
            List<Task_ConditionFuncNode> nextNodes = NodeHelper.GetNodeOutNodes<Task_ConditionFuncNode>(Owner, this, "下一个条件");
            if (nextNodes.Count > 0)
            {
                funcs.AddRange(nextNodes[0].GetFuncs());
            }
            return funcs;
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

        [InputPort("下一个表现", BasePort.Capacity.Single)]
        public Task_DisplayFuncData nextNode;

        public List<TaskTargetDisplayFunc> GetFuncs()
        {
            List<TaskTargetDisplayFunc> funcs = new List<TaskTargetDisplayFunc>();
            funcs.Add(CreateFunc());
            List<Task_TargetDisplayFuncNode> nextNodes = NodeHelper.GetNodeOutNodes<Task_TargetDisplayFuncNode>(Owner, this, "下一个表现");
            if (nextNodes.Count > 0)
            {
                funcs.AddRange(nextNodes[0].GetFuncs());
            }
            return funcs;
        }

        public abstract TaskTargetDisplayFunc CreateFunc();
    }

    #endregion

    #region 任务行为函数

    public class Task_ActionFuncData { }
    /// <summary>
    /// 任务行为函数
    /// </summary>
    public abstract class Task_ActionFuncNode : BaseNode
    {
        public abstract List<TaskActionFunc> GetFuncs();

        public abstract TaskActionFunc CreateFunc();
    }

    /// <summary>
    /// 任务通用行为
    /// </summary>
    public abstract class Task_CommonActionFuncNode : Task_ActionFuncNode
    {
        public override string Title { get => "任务通用行为"; set => base.Title = value; }
        public override string Tooltip { get => "任务通用行为"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public Task_ActionFuncData parentNode;

        [InputPort("下一个行为", BasePort.Capacity.Single)]
        public Task_ActionFuncData nextNode;

        public override List<TaskActionFunc> GetFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            funcs.Add(CreateFunc());
            List<Task_ActionFuncNode> nextNodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "下一个行为");
            if (nextNodes.Count > 0)
            {
                funcs.AddRange(nextNodes[0].GetFuncs());
            }
            return funcs;
        }
    }

    /// <summary>
    /// 任务接受行为函数
    /// </summary>
    public abstract class Task_AcceptActionFuncNode : Task_ActionFuncNode
    {
        public override string Title { get => "任务接受行为函数"; set => base.Title = value; }
        public override string Tooltip { get => "任务接受行为函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public Task_ActionFuncData parentNode;
    } 

    /// <summary>
    /// 任务提交行为函数
    /// </summary>
    public abstract class Task_ExecuteActionFuncNode : Task_ActionFuncNode
    {
        public override string Title { get => "任务提交行为函数"; set => base.Title = value; }
        public override string Tooltip { get => "任务提交行为函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public Task_ActionFuncData parentNode;
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

        [InputPort("阶段条件", BasePort.Capacity.Single)]
        public Task_ConditionFuncData conditionFuncs;

        [InputPort("阶段目标", BasePort.Capacity.Single)]
        public MapActorData target;

        [InputPort("阶段目标表现", BasePort.Capacity.Single)]
        public Task_DisplayFuncData targetDisplayFuncs;

        [OutputPort("阶段行为", BasePort.Capacity.Single)]
        public Task_ActionFuncData actionFuncs;

        [OutputPort("阶段成功", BasePort.Capacity.Single)]
        public Task_ActionFuncData actionSuccess;

        [OutputPort("阶段失败", BasePort.Capacity.Single)]
        public Task_ActionFuncData actionFail;

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
            List<Task_ConditionFuncNode> conditionNodes = NodeHelper.GetNodeOutNodes<Task_ConditionFuncNode>(Owner, this, "阶段条件");
            if (conditionNodes.Count > 0)
            {
                return conditionNodes[0].GetFuncs();
            }
            return null;
        }

        public List<TaskTargetDisplayFunc> GetDisplayFuncs()
        {
            List<Task_TargetDisplayFuncNode> conditionNodes = NodeHelper.GetNodeOutNodes<Task_TargetDisplayFuncNode>(Owner, this, "阶段目标表现");
            if (conditionNodes.Count > 0)
            {
                return conditionNodes[0].GetFuncs();
            }
            return null;
        }

        #region 行为

        public List<TaskActionFunc> GetActionFuncs()
        {
            List<Task_ActionFuncNode> actNodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "阶段行为");
            if (actNodes.Count > 0)
            {
                return actNodes[0].GetFuncs();
            }
            return null;
        }

        public List<TaskActionFunc> GetActionSuccessFuncs()
        {
            List<Task_ActionFuncNode> actNodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "阶段成功");
            if (actNodes.Count > 0)
            {
                return actNodes[0].GetFuncs();
            }
            return null;
        }

        public List<TaskActionFunc> GetActionFailFuncs()
        {
            List<Task_ActionFuncNode> actNodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "阶段失败");
            if (actNodes.Count > 0)
            {
                return actNodes[0].GetFuncs();
            }
            return null;
        }
        #endregion

        public TaskContent GetContent()
        {
            TaskContent content = new TaskContent();
            content.mapId = GetTargetMapId();
            content.actorIds = GetTargetActorIds();
            content.displayFuncs = GetDisplayFuncs();
            content.conditionFuncs = GetConditionFuncs();
            content.actionFuncs = GetActionFuncs();
            content.actionSuccess = GetActionSuccessFuncs();
            content.actionFail = GetActionFailFuncs();
            return content;
        }
    }

    [NodeMenuItem("任务接受")]
    public class Task_AcceptNode : Task_Node
    {
        public override string Title { get => $"接受{taskId}任务"; set => base.Title = value; }
    }

    [NodeMenuItem("任务提交")]
    public class Task_ExecuteNode : Task_Node
    {
        public override string Title { get => $"提交{taskId}任务"; set => base.Title = value; }

    }
}
