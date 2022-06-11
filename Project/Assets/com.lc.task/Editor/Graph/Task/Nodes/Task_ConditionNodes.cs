using LCNode;
using System.Collections.Generic;

namespace LCTask.TaskGraph
{
    [NodeMenuItem("任务/检测任务完成")]
    public class Task_CON_CheckTaskFinishNodes : Task_ConditionFuncNode
    {
        public override string Title { get => "检测任务完成"; set => base.Title = value; }

        [NodeValue("任务Id")]
        public List<int> taskIds = new List<int>();

        public override TaskConditionFunc CreateFunc()
        {
            TaskCondition_CheckTaskFinish func = new TaskCondition_CheckTaskFinish();
            func.taskIds = taskIds;
            return func;
        }
    }
}