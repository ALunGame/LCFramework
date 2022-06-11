using LCMap;
using System.Collections.Generic;

namespace LCTask
{
    /// <summary>
    /// 任务目标表现函数
    /// </summary>
    public abstract class TaskTargetDisplayFunc
    {
        public abstract void Execute(TaskObj taskObj, List<ActorObj> targets);
        public abstract void Clear(TaskObj taskObj, List<ActorObj> targets);
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
    /// 任务监听函数
    /// </summary>
    public abstract class TaskListenFunc
    {
        /// <summary>
        /// 开启监听
        /// </summary>
        /// <param name="taskObj"></param>
        public abstract void Listen(TaskObj taskObj);
        
        /// <summary>
        /// 清理监听
        /// </summary>
        public abstract void Clear(TaskObj taskObj);
    }

    /// <summary>
    /// 任务行为状态
    /// </summary>
    public enum TaskActionState
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 完成
        /// </summary>
        Finished,

        /// <summary>
        /// 失败
        /// </summary>
        Fail,  
        
        /// <summary>
        /// 正在执行
        /// </summary>
        Running,

        /// <summary>
        /// 等待执行
        /// </summary>
        Wait,
    }

    /// <summary>
    /// 任务行为函数
    /// </summary>
    public abstract class TaskActionFunc
    {
        private TaskActionState actionState = TaskActionState.Wait;
        private TaskObj taskObj;
        public TaskActionState ActionState { get => actionState;}

        /// <summary>
        /// 开始执行行为
        /// </summary>
        /// <param name="taskObj"></param>
        public void Start(TaskObj taskObj)
        {
            if (actionState != TaskActionState.Wait)
                return;
            this.taskObj = taskObj;
            actionState = TaskActionState.Running;
            OnStart(taskObj);
        }

        /// <summary>
        /// 运行中
        /// </summary>
        public void Running()
        {
            OnRunning(taskObj);
        }

        /// <summary>
        /// 行为清理
        /// </summary>
        public void Clear()
        {
            OnClear(taskObj);
        }

        /// <summary>
        /// 重置行为
        /// </summary>
        public void Reset()
        {
            if (actionState == TaskActionState.Error)
            {
                TaskLocate.Log.LogError("重置任务行为状态失败，行为失败>>>>", this.GetType().Name);
                return;
            }
            actionState = TaskActionState.Wait;
            Clear();
        }

        /// <summary>
        /// 开始执行行为时
        /// </summary>
        /// <param name="taskObj"></param>
        protected abstract void OnStart(TaskObj taskObj);

        /// <summary>
        /// 运行中每帧调用，注意性能
        /// </summary>
        /// <returns></returns>
        protected virtual void OnRunning(TaskObj taskObj) { }

        /// <summary>
        /// 清理时
        /// </summary>
        /// <returns></returns>
        protected abstract void OnClear(TaskObj taskObj);

        /// <summary>
        /// 行为完成
        /// </summary>
        /// <returns></returns>
        protected void Finish()
        {
            actionState = TaskActionState.Finished;
            Clear();
        }

        /// <summary>
        /// 行为失败
        /// </summary>
        /// <returns></returns>
        protected void Fail()
        {
            actionState = TaskActionState.Fail;
            Clear();
        }

        /// <summary>
        /// 行为错误
        /// </summary>
        /// <returns></returns>
        protected void Error()
        {
            actionState = TaskActionState.Error;
            Clear();
        }
    }
}
