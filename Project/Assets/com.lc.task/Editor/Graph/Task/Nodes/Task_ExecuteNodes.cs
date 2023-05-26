using LCNode;
using LCNode.Model;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCTask.TaskGraph
{
    public class Task_BranchFuncData { }

    /// <summary>
    /// 任务分支行为
    /// </summary>
    [NodeMenuItem("任务/分支")]
    public class Task_BranchNode : BaseNode
    {
        public override string Title { get => "分支"; set => base.Title = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public Task_BranchFuncData target;

        [InputPort("分支条件", BasePort.Capacity.Multi)]
        public Task_ConditionFuncData conditionFuncs;

        [OutputPort("分支行为", BasePort.Capacity.Multi)]
        public Task_ActionFuncData actionFuncs;

        private List<TaskConditionFunc> GetConditionFuncs()
        {
            List<TaskConditionFunc> funcs = new List<TaskConditionFunc>();
            List<Task_ConditionFuncNode> conditionNodes = NodeHelper.GetNodeOutNodes<Task_ConditionFuncNode>(Owner.Model, this, "分支条件");
            if (conditionNodes.Count > 0)
            {
                for (int i = 0; i < conditionNodes.Count; i++)
                {
                    funcs.Add(conditionNodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public List<TaskActionFunc> GetActionFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner.Model, this, "分支行为");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public TaskBranch GetBranch()
        {
            TaskBranch taskBranch = new TaskBranch();
            taskBranch.conditionFuncs = GetConditionFuncs();
            taskBranch.actionFuncs = GetActionFuncs();
            return taskBranch;
        }
    }

    /// <summary>
    /// 任务分支行为
    /// </summary>
    [NodeMenuItem("任务/分支行为")]
    public class Task_ACT_ExecuteBranchNode : Task_ExecuteActionFuncNode
    {
        public override string Title { get => "分支根节点"; set => base.Title = value; }


        [OutputPort("分支", BasePort.Capacity.Multi)]
        public Task_BranchFuncData branchs;

        public override TaskActionFunc CreateFunc()
        {
            TaskBranchFunc branchFunc = new TaskBranchFunc();
            List<Task_BranchNode> nodes = NodeHelper.GetNodeOutNodes<Task_BranchNode>(Owner.Model, this, "分支");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    TaskBranch branch = nodes[i].GetBranch();
                    branch.branchId = i;
                    branchFunc.branches.Add(branch);
                }
            }
            return branchFunc;
        }
    }
}