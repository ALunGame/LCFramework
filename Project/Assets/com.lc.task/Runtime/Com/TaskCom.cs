using LCECS;
using LCECS.Core;
using System;
using System.Collections.Generic;

namespace LCTask
{
    public class TaskCom : BaseCom
    {
        [NonSerialized]
        private List<TaskObj> tasks = new List<TaskObj>();

        public IReadOnlyList<TaskObj> Tasks { get => tasks; }

        public void AddTask(TaskObj aoeObj)
        {
            tasks.Add(aoeObj);
        }

        public void RemoveTask(TaskObj aoeObj)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Equals(aoeObj))
                {
                    tasks.RemoveAt(i);
                }
            }
        }
    }
}
