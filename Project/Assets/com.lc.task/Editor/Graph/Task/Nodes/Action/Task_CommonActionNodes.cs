using LCNode;

namespace LCTask.TaskGraph
{
    [NodeMenuItem("任务/解锁任务")]
    public class Task_ACT_UnlockTaskNode : Task_CommonActionFuncNode
    {
        public override string Tooltip { get => "将会保存在玩家数据中"; set => base.Tooltip = value; }

        [NodeValue("解锁的任务Id")]
        public int taskId;

        public override TaskActionFunc CreateFunc()
        {
            TaskUnlockTaskFunc func = new TaskUnlockTaskFunc();
            func.taskId = taskId;
            return func;
        }
    }

    [NodeMenuItem("任务/尝试解锁任务")]
    public class Task_ACT_TryUnlockTaskNode : Task_CommonActionFuncNode
    {
        public override string Tooltip { get => "如果目标任务条件通过,将会保存在玩家数据中"; set => base.Tooltip = value; }

        [NodeValue("尝试解锁的任务Id")]
        public int taskId;

        public override TaskActionFunc CreateFunc()
        {
            TaskTryUnlockTaskFunc func = new TaskTryUnlockTaskFunc();
            func.taskId = taskId;
            return func;
        }
    }

    [NodeMenuItem("任务/移除任务")]
    public class Task_ACT_RemoveTaskNode : Task_CommonActionFuncNode
    {
        public override string Tooltip { get => "移除玩家接受和提及中的任务"; set => base.Tooltip = value; }

        [NodeValue("尝试解锁的任务Id")]
        public int taskId;

        public override TaskActionFunc CreateFunc()
        {
            TaskRemoveTaskFunc func = new TaskRemoveTaskFunc();
            func.taskId = taskId;
            return func;
        }
    }

    [NodeMenuItem("任务/提交任务")]
    public class Task_ACT_ExecuteTaskNode : Task_CommonActionFuncNode
    {
        public override string Tooltip { get => "任务流程中提交其他任务，任务如果有目标将不会执行"; set => base.Tooltip = value; }

        [NodeValue("尝试解锁的任务Id")]
        public int taskId;

        public override TaskActionFunc CreateFunc()
        {
            TaskExecuteTaskFunc func = new TaskExecuteTaskFunc();
            func.taskId = taskId;
            return func;
        }
    }
}
