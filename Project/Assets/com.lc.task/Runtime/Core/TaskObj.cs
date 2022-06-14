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

        /// <summary>
        /// �׶���Ϊ��ִ��
        /// </summary>
        public TaskActionGroupExecute GroupExecute { get; private set; }

        #region ��֧

        /// <summary>
        /// ѡ��ķ�֧Id
        /// </summary>
        public int SelBranchId { get; private set; }

        /// <summary>
        /// ��֧
        /// </summary>
        public TaskBranchFunc BranchFunc { get; private set; }

        #endregion

        public TaskObj(int taskId, TaskModel model)
        {
            this.TaskId  = taskId;
            this.Stage   = TaskStage.None;
            this.Model   = model;
            this.GroupExecute = new TaskActionGroupExecute(this);

            if (model.execute != null)
            {
                for (int i = 0; i < model.execute.actionFuncs.Count; i++)
                {
                    TaskActionFunc actFunc = model.execute.actionFuncs[i];
                    if (actFunc is TaskBranchFunc)
                    {
                        BranchFunc = (TaskBranchFunc)actFunc;
                        break;
                    }
                }
            }
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

        #region ��֧

        /// <summary>
        /// ����ѡ���֧
        /// </summary>
        /// <param name="pBranchId"></param>
        public void SetSelBranchId(int pBranchId)
        {
            if (BranchFunc == null)
            {
                TaskLocate.Log.LogError("ѡ���֧����,û�з�֧", TaskId, pBranchId);
                return;
            }
            if (BranchFunc.GetBranch(pBranchId) == null)
            {
                TaskLocate.Log.LogError("ѡ���֧����,û�з�֧", TaskId, pBranchId);
                return;
            }
            SelBranchId = pBranchId;
        } 

        /// <summary>
        /// ����֧����
        /// </summary>
        /// <param name="pBranchId"></param>
        /// <returns></returns>
        public bool CheckBranchCondition(int pBranchId)
        {
            if (BranchFunc == null)
            {
                TaskLocate.Log.LogError("ѡ���֧����,û�з�֧", TaskId, pBranchId);
                return false;
            }
            return BranchFunc.CheckCondition(pBranchId, this);
        }

        #endregion

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
