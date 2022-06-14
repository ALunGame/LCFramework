using System;
using System.Collections.Generic;

namespace LCTask
{
    /// <summary>
    /// 任务分支行为
    /// </summary>
    public class TaskBranchFunc : TaskActionFunc
    {
        public List<TaskBranch> branches = new List<TaskBranch>();
        private TaskBranch selBranch = null;
        private List<TaskActionFunc> currActions = new List<TaskActionFunc>();

        protected override TaskActionState OnStart(TaskObj taskObj)
        {
            selBranch = GetBranch(taskObj.SelBranchId);
            if (selBranch == null)
            {
                return TaskActionState.Error;
            }
            currActions = selBranch.actionFuncs;
            return TaskActionState.Running;
        }

        protected override TaskActionState OnRunning(TaskObj taskObj)
        {
            if (selBranch == null)
                return TaskActionState.Error;
            if (currActions == null || currActions.Count < 0)
            {
                return TaskActionState.Finished;
            }
            for (int i = 0; i < currActions.Count; i++)
            {
                TaskActionFunc actionFunc = currActions[i];
                if (actionFunc.ActionState == TaskActionState.Running)
                {
                    actionFunc.Running();
                    if (!IsLegal(actionFunc.ActionState))
                    {
                        TaskLocate.Log.LogError("分支行为非法执行>>>>", actionFunc.ActionState, actionFunc.GetType().Name);
                        return actionFunc.ActionState;
                    }
                    return TaskActionState.Running;
                }
                if (actionFunc.ActionState == TaskActionState.Finished)
                {
                    int nextIndex = i + 1;
                    if (nextIndex > currActions.Count)
                    {
                        return TaskActionState.Finished;
                    }
                    TaskActionFunc nextAct = currActions[nextIndex];
                    if (nextAct.ActionState == TaskActionState.Wait)
                    {
                        nextAct.Start(taskObj);
                        if (!IsLegal(nextAct.ActionState))
                        {
                            TaskLocate.Log.LogError("分支行为非法执行>>>>", nextAct.ActionState, nextAct.GetType().Name);
                            return nextAct.ActionState;
                        }
                        return TaskActionState.Running;
                    }
                }
            }
            TaskLocate.Log.LogError("分支行为非法执行,没有正确退出>>>>");
            return TaskActionState.Error;
        }

        protected override void OnClear(TaskObj taskObj)
        {
            if (currActions != null || currActions.Count > 0)
            {
                for (int i = 0; i < currActions.Count; i++)
                {
                    currActions[i].Clear();
                }
            }
            currActions.Clear();
        }

        public TaskBranch GetBranch(int branchId)
        {
            for (int i = 0; i < branches.Count; i++)
            {
                if (branches[i].branchId == branchId)
                {
                    return branches[i];
                }
            }
            return null;
        }

        #region 条件

        public bool CheckCondition(int branchId, TaskObj pTaskObj)
        {
            TaskBranch branch = GetBranch(branchId);
            if (branch == null)
                return false;   
            if (branch.conditionFuncs == null || branch.conditionFuncs.Count <= 0)
                return true;
            return checkCondition(branch.conditionFuncs,0, pTaskObj);
        }

        private bool checkCondition(List<TaskConditionFunc> conditionFuncs, int pConIndex, TaskObj pTaskObj)
        {
            if (pConIndex < 0 || conditionFuncs.Count >= pConIndex)
                return true;
            TaskConditionFunc confunc = conditionFuncs[pConIndex];
            bool trueValue = confunc.CheckTure(pTaskObj);
            trueValue = confunc.checkValue == trueValue ? true : false;

            //下一个
            int nextIndex = pConIndex + 1;
            if (nextIndex < 0 || conditionFuncs.Count >= nextIndex)
                return trueValue;
            else
            {
                if (confunc.conditionType == ConditionType.AND)
                {
                    return trueValue && checkCondition(conditionFuncs,nextIndex, pTaskObj);
                }
                else if (confunc.conditionType == ConditionType.OR)
                {
                    return trueValue || checkCondition(conditionFuncs,nextIndex, pTaskObj);
                }
            }
            return trueValue;
        }

        #endregion
    }
}
