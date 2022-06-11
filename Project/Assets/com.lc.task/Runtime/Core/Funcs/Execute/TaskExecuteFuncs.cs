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

        protected override void OnStart(TaskObj taskObj)
        {
            throw new NotImplementedException();
        }

        protected override void OnRunning(TaskObj taskObj)
        {
            
        }

        protected override void OnClear(TaskObj taskObj)
        {
            throw new NotImplementedException();
        }
    }
}
