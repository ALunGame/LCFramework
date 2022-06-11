using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCTask
{
    public class TaskServer : ITaskServer
    {
        private TaskCom taskCom;

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="pTaskId"></param>
        /// <returns></returns>
        public bool AddTask(int pTaskId, TaskStage taskStage = TaskStage.Accept)
        {
            TaskObj taskObj = taskCom.GetTask(pTaskId);
            if (taskObj != null)
            {
                TaskLocate.Log.LogWarning("添加任务失败,重复任务Id",pTaskId);
                return false;
            }
            if (TaskLocate.Config.GetTaskModel(pTaskId,out var model))
            {
                taskObj = new TaskObj(pTaskId, model);
                taskObj.ExecuteTargetsDisplay(taskStage);
                taskCom.AddTask(taskObj);
                return true;
            }
            else
            {
                TaskLocate.Log.LogWarning("添加任务失败,没有任务配置", pTaskId);
                return false;
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
        /// 任务完成时
        /// </summary>
        /// <param name="pTaskId"></param>
        public void OnTaskFinish(int pTaskId)
        {
            taskCom.RemoveTask(pTaskId);
        }
    }
}