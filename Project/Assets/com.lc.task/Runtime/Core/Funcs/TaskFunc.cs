using LCMap;
using System.Collections.Generic;

namespace LCTask
{
    /// <summary>
    /// ����Ŀ����ֺ���
    /// </summary>
    public abstract class TaskTargetDisplayFunc
    {
        public abstract void Execute(TaskObj taskObj, List<Actor> targets);
        public abstract void Clear(TaskObj taskObj, List<Actor> targets);
    }

    /// <summary>
    /// ������������
    /// </summary>
    public abstract class TaskConditionFunc
    {
        public bool checkValue = true;
        public ConditionType conditionType;

        public abstract bool CheckTure(TaskObj taskObj);
    }

    /// <summary>
    /// �����������
    /// </summary>
    public abstract class TaskListenFunc
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="taskObj"></param>
        public abstract void Listen(TaskObj taskObj);
        
        /// <summary>
        /// �������
        /// </summary>
        public abstract void Clear(TaskObj taskObj);
    }

    /// <summary>
    /// ������Ϊ״̬
    /// </summary>
    public enum TaskActionState
    {
        /// <summary>
        /// ����
        /// </summary>
        Error,

        /// <summary>
        /// ���
        /// </summary>
        Finished,

        /// <summary>
        /// ʧ��
        /// </summary>
        Fail,  
        
        /// <summary>
        /// ����ִ��
        /// </summary>
        Running,

        /// <summary>
        /// �ȴ�ִ��
        /// </summary>
        Wait,
    }

    /// <summary>
    /// ������Ϊ����
    /// </summary>
    public abstract class TaskActionFunc
    {
        private TaskActionState actionState = TaskActionState.Wait;
        private TaskObj taskObj;
        public TaskActionState ActionState { get => actionState;}

        /// <summary>
        /// ��ʼִ����Ϊ
        /// </summary>
        /// <param name="taskObj"></param>
        public void Start(TaskObj taskObj)
        {
            if (actionState != TaskActionState.Wait)
                return;
            this.taskObj = taskObj;
            actionState = OnStart(taskObj);
        }

        /// <summary>
        /// ������
        /// </summary>
        public void Running()
        {
            if (actionState == TaskActionState.Finished)
                return;
            actionState = OnRunning(taskObj);
        }

        /// <summary>
        /// ��Ϊ����
        /// </summary>
        public void Clear()
        {
            OnClear(taskObj);
        }

        /// <summary>
        /// ������Ϊ
        /// </summary>
        public void Reset()
        {
            if (actionState == TaskActionState.Error)
            {
                TaskLocate.Log.LogError("����������Ϊ״̬ʧ�ܣ���Ϊʧ��>>>>", this.GetType().Name);
                return;
            }
            actionState = TaskActionState.Wait;
            Clear();
        }

        /// <summary>
        /// ��ʼִ����Ϊʱ
        /// </summary>
        /// <param name="taskObj"></param>
        protected abstract TaskActionState OnStart(TaskObj taskObj);

        /// <summary>
        /// ������ÿ֡���ã�ע������
        /// </summary>
        /// <returns></returns>
        protected virtual TaskActionState OnRunning(TaskObj taskObj) { return TaskActionState.Finished; }

        /// <summary>
        /// ����ʱ
        /// </summary>
        /// <returns></returns>
        protected abstract void OnClear(TaskObj taskObj);

        protected bool IsFinish(TaskActionState actionState)
        {
            return actionState == TaskActionState.Finished; 
        }

        public bool IsLegal(TaskActionState actionState)
        {
            return actionState != TaskActionState.Fail && actionState != TaskActionState.Error;
        }

    }
}
