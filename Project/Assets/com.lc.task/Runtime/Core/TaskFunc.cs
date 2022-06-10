using LCMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCTask
{
    /// <summary>
    /// 任务目标表现函数
    /// </summary>
    public abstract class TaskTargetDisplayFunc
    {
        public abstract void Execute(TaskObj taskObj, List<ActorObj> targets);

        /// <summary>
        /// 检测完成,每帧检测注意性能
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckFinish();
    }

    /// <summary>
    /// 任务条件函数
    /// </summary>
    public abstract class TaskConditionFunc
    {
        public bool checkValue = true;
        public ConditionType conditionType;

        public abstract bool CheckTure(TaskObj taskObj);
    }

    /// <summary>
    /// 任务行为函数
    /// </summary>
    public abstract class TaskActionFunc
    {
        public abstract void Execute(TaskObj taskObj);

        /// <summary>
        /// 检测完成,每帧检测注意性能
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckFinish();
    }
}
