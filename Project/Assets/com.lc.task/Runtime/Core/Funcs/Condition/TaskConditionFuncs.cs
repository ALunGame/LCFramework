using System.Collections.Generic;

namespace LCTask
{
    /// <summary>
    /// 检测任务完成
    /// </summary>
    public class TaskCondition_CheckTaskFinish : TaskConditionFunc
    {
        public List<int> taskIds;
        public override bool CheckTure(TaskObj taskObj)
        {
            return false;
        }
    }
}
