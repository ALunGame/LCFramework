using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.UserData
{
    public class TaskData:BaseUserData<TaskData>
    {
        private List<int> acceptTasks = new List<int>();
        /// <summary>
        /// 等待接受的任务
        /// </summary>
        public IReadOnlyList<int> AcceptTasks { get => acceptTasks; }
        /// <summary>
        /// 接受的任务变化
        /// </summary>
        public event Action<int> OnAcceptTaskChanged;

        private List<int> executeTasks = new List<int>();
        /// <summary>
        /// 等待提交的任务
        /// </summary>
        public IReadOnlyList<int> ExecuteTasks { get => executeTasks; }
        /// <summary>
        /// 提交的任务变化
        /// </summary>
        public event Action<int> OnExecuteTaskChanged;

        private List<int> finishTasks = new List<int>();
        /// <summary>
        /// 完成的任务
        /// </summary>
        public IReadOnlyList<int> FinishTasks { get => finishTasks; }
        /// <summary>
        /// 完成的任务变化
        /// </summary>
        public event Action<int> OnFinishTaskChanged;
        
        /// <summary>
        /// 移除任务
        /// </summary>
        public event Action<int> OnRemoveTaskChanged;

        #region Add

        public void AddAcceptTask(int taskId)
        {
            if (executeTasks.Contains(taskId))
                executeTasks.Remove(taskId);
            if (!acceptTasks.Contains(taskId))
                acceptTasks.Add(taskId);
            OnAcceptTaskChanged?.Invoke(taskId);
        }

        public void AddExecuteTask(int taskId)
        {
            if (acceptTasks.Contains(taskId))
                acceptTasks.Remove(taskId);
            if (!executeTasks.Contains(taskId))
                executeTasks.Add(taskId);
            OnExecuteTaskChanged?.Invoke(taskId);
        }

        public void AddFinishTask(int taskId)
        {
            if (acceptTasks.Contains(taskId))
                acceptTasks.Remove(taskId);
            if (executeTasks.Contains(taskId))
                executeTasks.Remove(taskId);
            if (!finishTasks.Contains(taskId))
                finishTasks.Add(taskId);
            OnFinishTaskChanged?.Invoke(taskId);
        }

        #endregion

        #region Remove

        public void RemoveTask(int taskId)
        {
            if (acceptTasks.Contains(taskId))
                acceptTasks.Remove(taskId);
            if (executeTasks.Contains(taskId))
                executeTasks.Remove(taskId);
            OnRemoveTaskChanged?.Invoke(taskId);
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