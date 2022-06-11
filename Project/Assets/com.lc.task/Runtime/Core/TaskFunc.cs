using LCMap;
using System.Collections.Generic;

namespace LCTask
{
    /// <summary>
    /// ����Ŀ����ֺ���
    /// </summary>
    public abstract class TaskTargetDisplayFunc
    {
        public abstract void Execute(TaskObj taskObj, List<ActorObj> targets);
        public abstract void Clear(TaskObj taskObj, List<ActorObj> targets);
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
            actionState = TaskActionState.Running;
            OnStart(taskObj);
        }

        /// <summary>
        /// ������
        /// </summary>
        public void Running()
        {
            OnRunning(taskObj);
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
        protected abstract void OnStart(TaskObj taskObj);

        /// <summary>
        /// ������ÿ֡���ã�ע������
        /// </summary>
        /// <returns></returns>
        protected virtual void OnRunning(TaskObj taskObj) { }

        /// <summary>
        /// ����ʱ
        /// </summary>
        /// <returns></returns>
        protected abstract void OnClear(TaskObj taskObj);

        /// <summary>
        /// ��Ϊ���
        /// </summary>
        /// <returns></returns>
        protected void Finish()
        {
            actionState = TaskActionState.Finished;
            Clear();
        }

        /// <summary>
        /// ��Ϊʧ��
        /// </summary>
        /// <returns></returns>
        protected void Fail()
        {
            actionState = TaskActionState.Fail;
            Clear();
        }

        /// <summary>
        /// ��Ϊ����
        /// </summary>
        /// <returns></returns>
        protected void Error()
        {
            actionState = TaskActionState.Error;
            Clear();
        }
    }
}
