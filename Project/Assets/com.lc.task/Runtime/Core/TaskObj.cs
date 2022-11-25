using LCMap;
using System.Collections.Generic;

namespace LCTask
{
    /// <summary>
    /// 任务阶段
    /// </summary>
    public enum TaskStage
    {
        /// <summary>
        /// 尚未开始
        /// </summary>
        None,

        /// <summary>
        /// 接受，判断接受条件，执行接受行为
        /// </summary>
        Accept,

        /// <summary>
        /// 提交，判断提交条件，执行提交行为
        /// </summary>
        Execute,

        /// <summary>
        /// 任务完成
        /// </summary>
        Finish,
    }

    /// <summary>
    /// 运行时创建的任务对象
    /// </summary>
    public class TaskObj
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public int TaskId { get; private set; }

        /// <summary>
        /// 任务阶段
        /// </summary>
        public TaskStage Stage { get; private set; }

        /// <summary>
        /// 数据
        /// </summary>
        public TaskModel Model;

        /// <summary>
        /// 当前任务阶段目标
        /// </summary>
        public List<Actor> Targets { get; private set; }

        /// <summary>
        /// 阶段行为组执行
        /// </summary>
        public TaskActionGroupExecute GroupExecute { get; private set; }

        #region 分支

        /// <summary>
        /// 选择的分支Id
        /// </summary>
        public int SelBranchId { get; private set; }

        /// <summary>
        /// 分支
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
        /// 检测当前阶段任务条件
        /// </summary>
        /// <returns></returns>
        public bool CheckCondition(TaskStage stage)
        {
            TaskContent content = GetContentByStage(stage);
            return content.CheckCondition(this);
        }

        /// <summary>
        /// 执行任务目标表现
        /// </summary>
        public void ExecuteTargetsDisplay(TaskStage stage)
        {
            ClearTargetsDisplay();
            TaskContent content = GetContentByStage(stage);
            //找到新的
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
        /// 接受
        /// </summary>
        /// <returns></returns>
        public bool Accept()
        {
            if (Stage == TaskStage.Accept)
            {
                TaskLocate.Log.Log($"无法接受任务{TaskId},当前任务正在接受中>>>");
                return false;
            }
            if (!CheckCondition(TaskStage.Accept))
            {
                TaskLocate.Log.Log($"无法接受任务{TaskId},当前任务条件未满足>>>");
                return false;
            }
            Stage = TaskStage.Accept;
            TaskContent content = GetContentByStage(Stage);
            GroupExecute.SetContent(content, OnActionGroupFinish);
            return true;
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public bool Execute()
        {
            if (Stage == TaskStage.Execute)
            {
                TaskLocate.Log.Log($"无法提交任务{TaskId},当前任务正在提交中>>>");
                return false;
            }
            if (!CheckCondition(TaskStage.Execute))
            {
                TaskLocate.Log.Log($"无法提交任务{TaskId},当前任务条件未满足>>>");
                return false;
            }
            Stage = TaskStage.Execute;
            TaskContent content = GetContentByStage(Stage);
            GroupExecute.SetContent(content, OnActionGroupFinish);
            return true;
        }

        #region 分支

        /// <summary>
        /// 设置选择分支
        /// </summary>
        /// <param name="pBranchId"></param>
        public void SetSelBranchId(int pBranchId)
        {
            if (BranchFunc == null)
            {
                TaskLocate.Log.LogError("选择分支出错,没有分支", TaskId, pBranchId);
                return;
            }
            if (BranchFunc.GetBranch(pBranchId) == null)
            {
                TaskLocate.Log.LogError("选择分支出错,没有分支", TaskId, pBranchId);
                return;
            }
            SelBranchId = pBranchId;
        } 

        /// <summary>
        /// 检测分支条件
        /// </summary>
        /// <param name="pBranchId"></param>
        /// <returns></returns>
        public bool CheckBranchCondition(int pBranchId)
        {
            if (BranchFunc == null)
            {
                TaskLocate.Log.LogError("选择分支出错,没有分支", TaskId, pBranchId);
                return false;
            }
            return BranchFunc.CheckCondition(pBranchId, this);
        }

        #endregion

        #region 目标

        /// <summary>
        /// 清理任务目标表现
        /// </summary>
        private void ClearTargetsDisplay()
        {
            TaskContent content = GetContentByStage(Stage);
            //清理当前
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
                TaskLocate.Log.LogError($"任务{Stage}阶段执行出错>>>");
                return;
            }
            if (actionState == TaskActionState.Fail)
            {
                TaskLocate.Log.Log($"任务{Stage}阶段执行失败!,检测是否可以自动再次执行此阶段");
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
                TaskLocate.Log.Log($"任务{Stage}阶段执行完成!,检测下一阶段是否可以自动执行");
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

            //下一个
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
