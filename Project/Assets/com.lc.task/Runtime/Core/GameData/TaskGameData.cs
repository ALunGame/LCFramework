using LCToolkit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LCTask
{
    public class TaskGameData : GameData
    {
        public override string FileName => "Task";

        private List<int> acceptTasks = new List<int>();
        /// <summary>
        /// 等待接受的任务
        /// </summary>
        public IReadOnlyList<int> AcceptTasks { get => acceptTasks; }

        private List<int> executeTasks = new List<int>();
        /// <summary>
        /// 等待提交的任务
        /// </summary>
        public IReadOnlyList<int> ExecuteTasks { get => executeTasks; }

        private List<int> finishTasks = new List<int>();
        /// <summary>
        /// 完成的任务
        /// </summary>
        public IReadOnlyList<int> FinishTasks { get => finishTasks; }

        #region Add

        public void AddAcceptTask(int taskId)
        {
            if (executeTasks.Contains(taskId))
                executeTasks.Remove(taskId);
            if (!acceptTasks.Contains(taskId))
                acceptTasks.Add(taskId);
        }

        public void AddExecuteTask(int taskId)
        {
            if (acceptTasks.Contains(taskId))
                acceptTasks.Remove(taskId);
            if (!executeTasks.Contains(taskId))
                executeTasks.Add(taskId);
        }

        public void AddFinishTask(int taskId)
        {
            if (acceptTasks.Contains(taskId))
                acceptTasks.Remove(taskId);
            if (executeTasks.Contains(taskId))
                executeTasks.Remove(taskId);
            if (!finishTasks.Contains(taskId))
                finishTasks.Add(taskId);
        }

        #endregion

        #region Remove

        public void RemoveTask(int taskId)
        {
            if (acceptTasks.Contains(taskId))
                acceptTasks.Remove(taskId);
            if (executeTasks.Contains(taskId))
                executeTasks.Remove(taskId);
        }

        #endregion

        #region Check

        /// <summary>
        /// 检测任务完成
        /// </summary>
        /// <returns></returns>
        public bool CheckFinish(int taskId)
        {
            return FinishTasks.Contains(taskId);
        }

        #endregion
    }
}
