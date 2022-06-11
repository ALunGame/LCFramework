using System;
using System.Collections.Generic;

namespace LCTask
{
    /// <summary>
    /// 任务行为组执行
    /// </summary>
    public class TaskActionGroupExecute
    {
        enum ActionGroup
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

        public void SetContent(TaskContent content, Action<TaskActionState> finishCallBack)
        {
            Clear();
            currContent = content;
            ExecuteListenFuncs();
            ChangeActionGroup(ActionGroup.Act);
        }

        public void Execute()
        {
            if (currActions == null || currActions.Count < 0)
            {
                OnActionListFinish(TaskActionState.Finished);
                return;
            }
            for (int i = 0; i < currActions.Count; i++)
            {
                TaskActionFunc actionFunc = currActions[i];
                if (actionFunc.ActionState == TaskActionState.Running)
                {
                    actionFunc.Running();
                    return;
                }
                if (actionFunc.ActionState == TaskActionState.Finished)
                {
                    int nextIndex = i + 1;
                    if (nextIndex > currActions.Count)
                    {
                        OnActionListFinish(TaskActionState.Finished);
                        return;
                    }
                    TaskActionFunc nextAct = currActions[nextIndex];
                    if (nextAct.ActionState == TaskActionState.Wait)
                    {
                        nextAct.Start(taskObj);
                        return;
                    }
                }
                if (actionFunc.ActionState == TaskActionState.Fail)
                {
                    OnActionListFinish(TaskActionState.Fail);
                    return;
                }
                if (actionFunc.ActionState == TaskActionState.Error)
                {
                    OnActionListFinish(TaskActionState.Error);
                    return;
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

        private void OnActionListFinish(TaskActionState actionState)
        {
            if (actionState == TaskActionState.Error)
            {
                ActionGroupFinish(TaskActionState.Error);
                return;
            }

            //行为失败，执行行为失败表现
            if (actionState == TaskActionState.Fail)
            {
                if (actionGroup == ActionGroup.Act)
                {
                    ChangeActionGroup(ActionGroup.ActFail);
                }
                else
                {
                    ActionGroupFinish(TaskActionState.Fail);
                }
                return;
            }

            //行为成功，执行行为成功表现
            if (actionState == TaskActionState.Finished)
            {
                if (actionGroup == ActionGroup.Act)
                {
                    ChangeActionGroup(ActionGroup.ActSucess);
                }
                else
                {
                    if (actionGroup == ActionGroup.ActFail)
                    {
                        ActionGroupFinish(TaskActionState.Fail);
                    }
                    else
                    {
                        ActionGroupFinish(TaskActionState.Finished);
                    }
                }
                return;
            }
        }

        private void ChangeActionGroup(ActionGroup pGroup)
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
