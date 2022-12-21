using LCECS;
using LCECS.Core;
using System;
using System.Collections.Generic;

namespace LCTask
{
    public class TaskCom : BaseCom
    {
        [NonSerialized]
        private Dictionary<int, TaskObj> tasks = new Dictionary<int, TaskObj>();

        public IReadOnlyDictionary<int, TaskObj> Tasks { get => tasks; }

        protected override void OnAwake(Entity pEntity)
        {
            TaskLocate.Task.SetTaskCom(this);
        }

        public void AddTask(TaskObj taskObj)
        {
            if (tasks.ContainsKey(taskObj.TaskId))
                return;
            tasks.Add(taskObj.TaskId, taskObj);
        }

        public void RemoveTask(int pTaskId)
        {
            if (!tasks.ContainsKey(pTaskId))
                return;
            tasks.Remove(pTaskId);
        }

        public TaskObj GetTask(int pTaskId)
        {
            if (!tasks.ContainsKey(pTaskId))
                return null;
            return tasks[pTaskId];
        }
    }
}
