using System;
using System.Collections.Generic;

namespace LCTask
{
    /// <summary>
    /// 任务行为组执行
    /// </summary>
    public class TaskActionGroupExecute
    {
        public enum ActionGroup
        {
            Act,
            ActSucess,
            ActFail,
        }

        private TaskObj taskObj;

        private TaskContent currContent;
        private ActionGroup actionGroup;
        private List<TaskActionFunc> currActions = new List<TaskActionFunc>();
        private List<TaskListenFunc> currActionListenFuncs = new List<TaskListenFunc>();

        private Action<TaskActionState> finishCallBack;

        public TaskActionGroupExecute(TaskObj pTaskObj)
        {
            this.taskObj = pTaskObj; 
        }

        public void SetContent(TaskContent content, Action<TaskActionState> pFinishCallBack)
        {
            Clear();
            currContent = content;
            finishCallBack = pFinishCallBack; 
            ExecuteListenFuncs();
        }

        public void Execute()
        {
            if (currActions == null || currActions.Count <= 0)
            {
                ActionGroupFinish(TaskActionState.Finished);
                return;
            }
            //执行第一个
            TaskActionFunc firstActionFunc = currActions[0];
            if (firstActionFunc.ActionState == TaskActionState.Wait)
            {
                firstActionFunc.Start(taskObj);
                if (!firstActionFunc.IsLegal(firstActionFunc.ActionState))
                {
                    TaskLocate.Log.LogError("行为非法执行>>>>", firstActionFunc.ActionState, firstActionFunc.GetType().Name);
                    ActionGroupFinish(firstActionFunc.ActionState);
                    return;
                }
            }
            for (int i = 0; i < currActions.Count; i++)
            {
                TaskActionFunc actionFunc = currActions[i];
                
                if (actionFunc.ActionState == TaskActionState.Running)
                {
                    actionFunc.Running();
                    if (!actionFunc.IsLegal(actionFunc.ActionState))
                    {
                        TaskLocate.Log.LogError("行为非法执行>>>>", actionFunc.ActionState, actionFunc.GetType().Name);
                        ActionGroupFinish(actionFunc.ActionState);
                        return;
                    }
                }
                
                if (actionFunc.ActionState == TaskActionState.Finished)
                {
                    int nextIndex = i + 1;
                    if (nextIndex >= currActions.Count)
                    {
                        ActionGroupFinish(TaskActionState.Finished);
                        return;
                    }
                    TaskActionFunc nextAct = currActions[nextIndex];
                    if (nextAct.ActionState == TaskActionState.Wait)
                    {
                        nextAct.Start(taskObj);
                        if (!nextAct.IsLegal(nextAct.ActionState))
                        {
                            TaskLocate.Log.LogError("行为非法执行>>>>", nextAct.ActionState, nextAct.GetType().Name);
                            ActionGroupFinish(actionFunc.ActionState);
                            return;
                        }
                    }
                }
            }
        }

        private void Clear()
        {
            if (currActionListenFuncs != null && currActionListenFuncs.Count > 0)
            {
                for (int i = 0; i < currActionListenFuncs.Count; i++)
                {
                    currActionListenFuncs[i].Clear(taskObj);
                }
            }
            if (currActions != null || currActions.Count > 0)
            {
                for (int i = 0; i < currActions.Count; i++)
                {
                    currActions[i].Clear();
                }
            }
            currActions.Clear();
        }

        private void ExecuteListenFuncs()
        {
            currActionListenFuncs = currContent.actionListenFuncs;
            if (currActionListenFuncs!= null && currActionListenFuncs.Count > 0)
            {
                for (int i = 0; i < currActionListenFuncs.Count; i++)
                {
                    currActionListenFuncs[i].Listen(taskObj);
                }
            }
        }

        public void ChangeActionGroup(ActionGroup pGroup)
        {
            actionGroup = pGroup;
            currActions = GetActionsByGroup(actionGroup);
            if (currActions != null || currActions.Count > 0)
            {
                for (int i = 0; i < currActions.Count; i++)
                {
                    currActions[i].Reset();
                }
            }
        }

        private List<TaskActionFunc> GetActionsByGroup(ActionGroup group)
        {
            switch (group)
            {
                case ActionGroup.Act:
                    return currContent.actionFuncs;
                case ActionGroup.ActSucess:
                    return currContent.actionSuccess;
                case ActionGroup.ActFail:
                    return currContent.actionFail;
                default:
                    return null;
            }
        }

        private void ActionGroupFinish(TaskActionState state)
        {
            Clear();
            Action<TaskActionState> func = finishCallBack;
            if (func == null)
                return;
            finishCallBack = null;
            func(state);
        }

    }
}
