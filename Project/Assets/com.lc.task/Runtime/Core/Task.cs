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
        /// ��δ��ʼ
        /// </summary>
        None,

        /// <summary>
        /// ���ܣ��жϽ���������ִ�н�����Ϊ
        /// </summary>
        Accept,

        /// <summary>
        /// �ύ���ж��ύ������ִ���ύ��Ϊ
        /// </summary>
        Execute,

        /// <summary>
        /// �������
        /// </summary>
        Finish,
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

        public List<TaskListenFunc> actionListenFuncs = new List<TaskListenFunc>();

        public List<TaskActionFunc> actionSuccess = new List<TaskActionFunc>();

        public List<TaskActionFunc> actionFail = new List<TaskActionFunc>();

        #region ����

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

            //��һ��
            int nextIndex = pConIndex + 1;
            if (nextIndex < 0 || conditionFuncs.Count >= nextIndex)
                return trueValue;
            else
            {
                if (confunc.conditionType == ConditionType.AND)
                {
                    return trueValue && checkCondition(nextIndex, pTaskObj);
                }
                else if (confunc.conditionType == ConditionType.OR)
                {
                    return trueValue || checkCondition(nextIndex, pTaskObj);
                }
            }
            return trueValue;
        }

        #endregion

        /// <summary>
        /// ����Ƿ�����Զ�ִ��
        /// </summary>
        /// <param name="pTaskObj"></param>
        /// <returns></returns>
        public bool CheckAutoExecute()
        {
            if (mapId == MapLocate.AllMapId && actorIds.Count <= 0)
                return true;
            return false;
        }
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
        /// ѡ��ķ�֧Id
        /// </summary>
        public int SelBranchId { get; private set; }

        /// <summary>
        /// ����
        /// </summary>
        public TaskModel Model;

        /// <summary>
        /// ��ǰ����׶�Ŀ��
        /// </summary>
        public List<ActorObj> Targets { get; private set; }

        /// <summary>
        /// �׶���Ϊ��ִ��
        /// </summary>
        public TaskActionGroupExecute GroupExecute { get; private set; }

        public TaskObj(int taskId, TaskModel model)
        {
            this.TaskId  = taskId;
            this.Stage   = TaskStage.None;
            this.Model   = model;
            this.GroupExecute = new TaskActionGroupExecute(this);
        }

        /// <summary>
        /// ��⵱ǰ�׶���������
        /// </summary>
        /// <returns></returns>
        public bool CheckCondition(TaskStage stage)
        {
            TaskContent content = GetContentByStage(stage);
            return content.CheckCondition(this);
        }

        /// <summary>
        /// ִ������Ŀ�����
        /// </summary>
        public void ExecuteTargetsDisplay(TaskStage stage)
        {
            ClearTargetsDisplay();
            TaskContent content = GetContentByStage(stage);
            //�ҵ��µ�
            if (content.mapId == MapLocate.AllMapId || content.mapId == MapLocate.Map.CurrMapId)
            {
                for (int i = 0; i < content.actorIds.Count; i++)
                {
                    Targets.AddRange(MapLocate.Map.GetActors(content.actorIds[i]));
                }
            }
            for (int i = 0; i < content.displayFuncs.Count; i++)
                content.displayFuncs[i].Execute(this, Targets);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public bool Accept()
        {
            if (Stage == TaskStage.Accept)
            {
                TaskLocate.Log.Log($"�޷���������{TaskId},��ǰ�������ڽ�����>>>");
                return false;
            }
            if (!CheckCondition(TaskStage.Accept))
            {
                TaskLocate.Log.Log($"�޷���������{TaskId},��ǰ��������δ����>>>");
                return false;
            }
            Stage = TaskStage.Accept;
            TaskContent content = GetContentByStage(Stage);
            GroupExecute.SetContent(content, OnActionGroupFinish);
            return true;
        }

        /// <summary>
        /// �ύ
        /// </summary>
        /// <returns></returns>
        public bool Execute()
        {
            if (Stage == TaskStage.Execute)
            {
                TaskLocate.Log.Log($"�޷��ύ����{TaskId},��ǰ���������ύ��>>>");
                return false;
            }
            if (!CheckCondition(TaskStage.Execute))
            {
                TaskLocate.Log.Log($"�޷��ύ����{TaskId},��ǰ��������δ����>>>");
                return false;
            }
            Stage = TaskStage.Execute;
            TaskContent content = GetContentByStage(Stage);
            GroupExecute.SetContent(content, OnActionGroupFinish);
            return true;
        }

        /// <summary>
        /// ����ѡ���֧
        /// </summary>
        /// <param name="pBranchId"></param>
        public void SetSelBranchId(int pBranchId)
        {
            SelBranchId = pBranchId;
        }

        #region Ŀ��

        /// <summary>
        /// ��������Ŀ�����
        /// </summary>
        private void ClearTargetsDisplay()
        {
            TaskContent content = GetContentByStage(Stage);
            //����ǰ
            if (content == null)
                return;
            for (int i = 0; i < content.displayFuncs.Count; i++)
                content.displayFuncs[i].Clear(this, Targets);
            Targets.Clear();
        }

        #endregion

        private TaskContent GetContentByStage(TaskStage pStage)
        {
            if (pStage == TaskStage.Accept)
            {
                return Model.accept;
            }
            if (pStage == TaskStage.Execute)
            {
                return Model.execute;
            }
            return null;    
        }

        private void OnActionGroupFinish(TaskActionState actionState)
        {
            if (actionState == TaskActionState.Error)
            {
                TaskLocate.Log.LogError($"����{Stage}�׶�ִ�г���>>>");
                return;
            }
            if (actionState == TaskActionState.Fail)
            {
                TaskLocate.Log.Log($"����{Stage}�׶�ִ��ʧ��!,����Ƿ�����Զ��ٴ�ִ�д˽׶�");
                TaskContent content = GetContentByStage(Stage);
                bool autoExecute = content.CheckAutoExecute();
                if (Stage == TaskStage.Accept)
                {
                    Stage = TaskStage.None;
                    if (autoExecute)
                        Accept();
                    else
                    {
                        ExecuteTargetsDisplay(TaskStage.Accept);
                    }
                }
                if (Stage == TaskStage.Execute)
                {
                    Stage = TaskStage.None;
                    if (autoExecute)
                        Execute();
                    else
                    {
                        ExecuteTargetsDisplay(TaskStage.Execute);
                    }
                }
                return;
            }
            if (actionState == TaskActionState.Finished)
            {
                TaskLocate.Log.Log($"����{Stage}�׶�ִ�����!,�����һ�׶��Ƿ�����Զ�ִ��");
                TaskStage nextStage = Stage == TaskStage.Accept ? TaskStage.Execute : TaskStage.Finish;
                if (nextStage == TaskStage.Execute)
                {
                    TaskContent content = GetContentByStage(Stage);
                    bool autoExecute    = content.CheckAutoExecute();
                    if (autoExecute)
                        Execute();
                    else
                    {
                        ExecuteTargetsDisplay(TaskStage.Execute);
                    }
                }
                if (nextStage == TaskStage.Finish)
                {
                    TaskLocate.Task.OnTaskFinish(TaskId);
                }
            }
        }
    }

    public class TaskExecute
    {
        private bool isExecuting;
        private TaskContent content;
        private TaskObj taskObj;

        public bool CheckCanExecute()
        {
            if (isExecuting)
                return true;
            if (content.conditionFuncs == null || content.conditionFuncs.Count <= 0)
                return true;
            return CheckCondition(0);
        }

        private bool CheckCondition(int pConIndex)
        {
            if (pConIndex < 0 || content.conditionFuncs.Count >= pConIndex)
                return true;
            TaskConditionFunc confunc = content.conditionFuncs[pConIndex];
            bool trueValue = confunc.CheckTure(taskObj);
            trueValue      = confunc.checkValue == trueValue ? true : false;

            //��һ��
            int nextIndex = pConIndex + 1;
            if (nextIndex < 0 || content.conditionFuncs.Count >= nextIndex)
                return trueValue;
            else
            {
                if (confunc.conditionType == ConditionType.AND)
                {
                    return trueValue && CheckCondition(nextIndex);
                }
                else if (confunc.conditionType == ConditionType.OR)
                {
                    return trueValue || CheckCondition(nextIndex);
                }
            }
            return trueValue;
        }

        private void Execute()
        {

        }
    }
}
