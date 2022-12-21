using System.Collections;
using System.Collections.Generic;
using Demo.UserData;
using UnityEngine;

namespace LCTask
{
    public class TaskServer : ITaskServer
    {
        private TaskCom taskCom;
        
        public void Init()
        {
            InitDataChange();
        }

        public void Clear()
        {
        }

        public void SetTaskCom(TaskCom pTaskCom)
        {
            taskCom = pTaskCom;
            foreach (int taskId in TaskData.Instance.AcceptTasks)
            {
                OnAcceptTask(taskId);
            }
            foreach (int taskId in TaskData.Instance.ExecuteTasks)
            {
                OnExecuteTask(taskId);
            }
        }
        
        /// <summary>
        /// 接受任务
        /// </summary>
        /// <returns></returns>
        public bool AcceptTask(int pTaskId)
        {
            TaskObj taskObj = taskCom.GetTask(pTaskId);
            if (taskObj == null)
            {
                TaskLocate.Log.LogError("接受任务失败,没有此任务", pTaskId);
                return false;
            }
            return taskObj.Accept();
        }

        /// <summary>
        /// 提交任务
        /// </summary>
        /// <returns></returns>
        public bool ExecuteTask(int pTaskId)
        {
            TaskObj taskObj = taskCom.GetTask(pTaskId);
            if (taskObj == null)
            {
                TaskLocate.Log.LogError("接受任务失败,没有此任务", pTaskId);
                return false;
            }
            return taskObj.Execute();
        }

        /// <summary>
        /// 任务完成
        /// </summary>
        /// <param name="pTaskI"></param>
        public void FinishTask(int pTaskId)
        {
            TaskData.Instance.AddFinishTask(pTaskId);
        }
        
        #region 数据改变事件

        private void InitDataChange()
        {
            TaskData.Instance.OnAcceptTaskChanged += OnAcceptTask;
            TaskData.Instance.OnExecuteTaskChanged += OnExecuteTask;
            TaskData.Instance.OnFinishTaskChanged += OnFinsihTask;
            TaskData.Instance.OnRemoveTaskChanged += OnRemoveTask;
        }

        private void OnAcceptTask(int pTaskId)
        {
            if (taskCom == null)
            {
                return;
            }
            
            SetStageOrAddTask(pTaskId, TaskWaitState.Accept);
        }
        
        private void OnExecuteTask(int pTaskId)
        {
            if (taskCom == null)
            {
                return;
            }
            SetStageOrAddTask(pTaskId, TaskWaitState.Execute);
        }
        
        private void OnFinsihTask(int pTaskId)
        {
            if (taskCom == null)
            {
                return;
            }

            ExecuteTaskFinishActions(pTaskId);
        }
        
        private void OnRemoveTask(int pTaskId)
        {
            if (taskCom == null)
            {
                return;
            }
            TaskLocate.Task.RemoveTask(pTaskId);
        }
        
        /// <summary>
        /// 设置任务阶段或添加任务
        /// </summary>
        /// <param name="pTaskId"></param>
        /// <param name="taskStage"></param>
        private void SetStageOrAddTask(int pTaskId, TaskWaitState pWaitState)
        {
            if (pWaitState == TaskWaitState.None)
            {
                return;
            }
            TaskObj taskObj = taskCom.GetTask(pTaskId);
            if (taskObj == null)
            {
                if (TaskLocate.Config.GetTaskModel(pTaskId,out var model))
                {
                    taskObj = new TaskObj(pTaskId, model);
                    taskCom.AddTask(taskObj);
                }
                else
                {
                    TaskLocate.Log.LogWarning("添加任务失败,没有任务配置", pTaskId);
                    return;
                }

            }
            //设置阶段
            taskObj.SetWaitState(pWaitState);

            //自动执行
            TaskStage tStage = pWaitState == TaskWaitState.Accept ? TaskStage.Accept : TaskStage.Execute;
            if (taskObj.CheckCanAutoDoStage(tStage))
            {
                if (tStage == TaskStage.Accept)
                {
                    AcceptTask(pTaskId);
                }
                else if (tStage == TaskStage.Execute)
                {
                    ExecuteTask(pTaskId);
                }
            }
        }

        /// <summary>
        /// 执行任务完成行为
        /// </summary>
        /// <param name="pTaskId"></param>
        private void ExecuteTaskFinishActions(int pTaskId)
        {
            if (taskCom == null)
            {
                return;
            }
            TaskObj taskObj = taskCom.GetTask(pTaskId);
            if (taskObj == null)
            {
                return;
            }

            taskObj.ExecuteFinishActions();
        }
        
        /// <summary>
        /// 任务完成时
        /// </summary>
        /// <param name="pTaskId"></param>
        public void RemoveTask(int pTaskId)
        {
            taskCom.RemoveTask(pTaskId);
        }

        #endregion

    }
}