using LCMap;
using System.Collections.Generic;

namespace LCTask
{
    /// <summary>
    /// 任务内容
    /// </summary>
    public class TaskContent
    {
        public int mapId;

        public List<int> actorIds = new List<int>();

        public List<TaskTargetDisplayFunc> displayFuncs = new List<TaskTargetDisplayFunc>();

        public List<TaskConditionFunc> conditionFuncs = new List<TaskConditionFunc>();

        public List<TaskListenFunc> actionListenFuncs = new List<TaskListenFunc>();

        public List<TaskActionFunc> actionFuncs = new List<TaskActionFunc>();

        public List<TaskActionFunc> actionSuccess = new List<TaskActionFunc>();

        public List<TaskActionFunc> actionFail = new List<TaskActionFunc>();

        #region 条件

        public bool CheckCondition(TaskObj pTaskObj)
        {
            if (conditionFuncs == null || conditionFuncs.Count <= 0)
                return true;
            return checkCondition(0, pTaskObj);
        }

        private bool checkCondition(int pConIndex, TaskObj pTaskObj)
        {
            if (pConIndex < 0 || conditionFuncs.Count >= pConIndex)
                return true;
            TaskConditionFunc confunc = conditionFuncs[pConIndex];
            bool trueValue = confunc.CheckTure(pTaskObj);
            trueValue = confunc.checkValue == trueValue ? true : false;

            //下一个
            int nextIndex = pConIndex + 1;
            if (nextIndex < 0 || conditionFuncs.Count >= nextIndex)
                return trueValue;
            else
            {
                if (confunc.conditionType == ConditionRelated.AND)
                {
                    return trueValue && checkCondition(nextIndex, pTaskObj);
                }
                else if (confunc.conditionType == ConditionRelated.OR)
                {
                    return trueValue || checkCondition(nextIndex, pTaskObj);
                }
            }
            return trueValue;
        }

        #endregion

        /// <summary>
        /// 检测是否可以自动执行
        /// </summary>
        /// <param name="pTaskObj"></param>
        /// <returns></returns>
        public bool CheckAutoExecute()
        {
            if (mapId == MapLocate.AllMapId && (actorIds == null || actorIds.Count <= 0))
                return true;
            return false;
        }
    }

    /// <summary>
    /// 任务配置数据
    /// </summary>
    public struct TaskModel
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public int id;

        /// <summary>
        /// 前置解锁任务
        /// </summary>
        public List<int> preUnlockTasks;

        /// <summary>
        /// 解锁任务
        /// </summary>
        public List<int> unlockTasks;

        /// <summary>
        /// 接受
        /// </summary>
        public TaskContent accept;

        /// <summary>
        /// 提交
        /// </summary>
        public TaskContent execute;
    }
}
