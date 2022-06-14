using System;

namespace LCTask
{
    /// <summary>
    /// 解锁任务行为（将会保存在玩家数据中）
    /// </summary>
    public class TaskUnlockTaskFunc : TaskActionFunc
    {
        public int taskId;

        protected override TaskActionState OnStart(TaskObj taskObj)
        {
            throw new NotImplementedException();
        }

        protected override void OnClear(TaskObj taskObj)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 尝试解锁任务行为（接受条件通过将会保存在玩家数据中）
    /// </summary>
    public class TaskTryUnlockTaskFunc : TaskActionFunc
    {
        public int taskId;

        protected override TaskActionState OnStart(TaskObj taskObj)
        {
            throw new NotImplementedException();
        }

        protected override void OnClear(TaskObj taskObj)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 移除任务
    /// </summary>
    public class TaskRemoveTaskFunc : TaskActionFunc
    {
        public int taskId;

        protected override TaskActionState OnStart(TaskObj taskObj)
        {
            throw new NotImplementedException();
        }

        protected override void OnClear(TaskObj taskObj)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 提交任务
    /// </summary>
    public class TaskExecuteTaskFunc : TaskActionFunc
    {
        public int taskId;

        protected override TaskActionState OnStart(TaskObj taskObj)
        {
            throw new NotImplementedException();
        }

        protected override void OnClear(TaskObj taskObj)
        {
            throw new NotImplementedException();
        }
    }
}
