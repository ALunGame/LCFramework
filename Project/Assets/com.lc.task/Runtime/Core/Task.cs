using LCMap;
using System.Collections.Generic;

namespace LCTask
{
    /// <summary>
    /// ����׶�
    /// </summary>
    public enum TaskStage
    {
        /// <summary>
        /// ����ǰ��ִ�н���Ŀ�����
        /// </summary>
        PreAccept,

        /// <summary>
        /// ���ܣ��жϽ���������ִ�н�����Ϊ
        /// </summary>
        Accept,

        /// <summary>
        /// �ύǰ��ִ���ύĿ�����
        /// </summary>
        PreExecute,

        /// <summary>
        /// �ύ���ж��ύ������ִ���ύ��Ϊ
        /// </summary>
        Execute,
    }

    /// <summary>
    /// ��������
    /// </summary>
    public class TaskContent
    {
        public int mapId;

        public List<int> actorIds = new List<int>();

        public List<TaskTargetDisplayFunc> displayFuncs = new List<TaskTargetDisplayFunc>();

        public List<TaskConditionFunc> conditionFuncs = new List<TaskConditionFunc>();

        public List<TaskActionFunc> actionFuncs = new List<TaskActionFunc>();

        public List<TaskActionFunc> actionSuccess = new List<TaskActionFunc>();

        public List<TaskActionFunc> actionFail = new List<TaskActionFunc>();
    }

    /// <summary>
    /// ������������
    /// </summary>
    public struct TaskModel
    {
        /// <summary>
        /// ����Id
        /// </summary>
        public int id;

        /// <summary>
        /// ǰ�ý�������
        /// </summary>
        public List<int> preUnlockTasks;

        /// <summary>
        /// ��������
        /// </summary>
        public List<int> unlockTasks;

        /// <summary>
        /// ����
        /// </summary>
        public TaskContent accept;

        /// <summary>
        /// �ύ
        /// </summary>
        public TaskContent execute;
    } 

    /// <summary>
    /// ����ʱ�������������
    /// </summary>
    public class TaskObj
    {
        /// <summary>
        /// ����Id
        /// </summary>
        public int TaskId { get; private set; }

        /// <summary>
        /// ����׶�
        /// </summary>
        public TaskStage Stage { get; private set; }

        /// <summary>
        /// ����
        /// </summary>
        public TaskModel Model;

        /// <summary>
        /// ��ǰ����׶�Ŀ��
        /// </summary>
        public List<ActorObj> Targets { get; private set; }
    }

    public class TaskExecute
    {
        private bool isExecuting;
        private TaskContent content;

        public bool CheckCanExecute()
        {
            if (isExecuting)
                return true;
            if (content.conditionFuncs == null || content.conditionFuncs.Count <= 0)
                return true;
            
        }

        private bool CheckCondition()
        {

        }
    }
}
