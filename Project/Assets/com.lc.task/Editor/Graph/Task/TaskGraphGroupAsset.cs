using LCJson;
using LCNode;
using LCNode.Model;
using LCNode.Model.Internal;
using LCToolkit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCTask.TaskGraph
{
    [CreateAssetMenu(fileName = "Task组", menuName = "配置组/Task组", order = 1)]
    public class TaskGraphGroupAsset : BaseGraphGroupAsset<TaskGraphAsset>
    {
        public override string DisplayName => "Task";

        class TaskGraphData
        {
            public int taskId;
            public Task_AcceptNode acceptNode;
            public Task_ExecuteNode executeNode;

            public TaskGraphData(int taskId, Task_AcceptNode acceptNode, Task_ExecuteNode executeNode)
            {
                this.taskId = taskId;
                this.acceptNode = acceptNode;
                this.executeNode = executeNode;
            }

            public TaskModel GetTaskModel()
            {
                TaskModel taskModel = new TaskModel();
                taskModel.id = taskId;
                taskModel.accept = acceptNode.GetContent();
                taskModel.execute = executeNode.GetContent();
                return taskModel;
            }
        }

        public override void ExportGraph(List<InternalBaseGraphAsset> assets)
        {
            List<TaskModel> resModels = new List<TaskModel>();
            Dictionary<int, TaskGraphData> dataDict = new Dictionary<int, TaskGraphData>();
            foreach (InternalBaseGraphAsset asset in assets)
            {
                if (asset is TaskGraphAsset)
                {
                    TaskGraphAsset taskGraphAsset = asset as TaskGraphAsset;
                    BaseGraph graphData = taskGraphAsset.DeserializeGraph();
                    List<TaskModel> tModels = SerializeToTaskModel(graphData, taskGraphAsset);
                    foreach (var item in tModels)
                    {
                        for (int i = 0; i < resModels.Count; i++)
                        {
                            if (resModels[i].id == item.id)
                            {
                                Debug.LogError("重复的任务Id>>>>>" + item.id);
                                return;
                            }
                        }
                        resModels.Add(item);
                    }
                }
            }

            SetUnlockTasks(resModels);

            string filePath = TaskDef.GetTaskCnfPath();
            IOHelper.WriteText(JsonMapper.ToJson(resModels), filePath);
            Debug.Log($"任务生成成功》》》{filePath}");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private List<TaskModel> SerializeToTaskModel(BaseGraph graph, TaskGraphAsset asset)
        {
            List<TaskModel> models = new List<TaskModel>();
            List<Task_AcceptNode> acceptNodes = NodeHelper.GetNodes<Task_AcceptNode>(graph);
            List<Task_ExecuteNode> executeNodes = NodeHelper.GetNodes<Task_ExecuteNode>(graph);
            if (acceptNodes.Count <= 0 || executeNodes.Count<=0)
            {
                Debug.LogError($"试图序列化出错，没有根节点");
                return models;
            }

            foreach (var accept in acceptNodes)
            {
                foreach (var execute in executeNodes)
                {
                    if (execute.taskId == accept.taskId)
                    {
                        TaskGraphData graphData = new TaskGraphData(execute.taskId, accept, execute);
                        models.Add(graphData.GetTaskModel());
                    }
                }
            }
            return models;
        }

        private void SetUnlockTasks(List<TaskModel> models)
        {
            for (int i = 0; i < models.Count; i++)
            {
                var item = models[i];
                List<int> resUnlockTasks = new List<int>();

                List<int> acceptUnlockTasks = GetUnlockTasks(item.accept);
                foreach (var acceptUnlockTask in acceptUnlockTasks)
                {
                    if (!resUnlockTasks.Contains(acceptUnlockTask))
                        resUnlockTasks.Add(acceptUnlockTask);
                }

                List<int> executeUnlockTasks = GetUnlockTasks(item.execute);
                foreach (var acceptUnlockTask in executeUnlockTasks)
                {
                    if (!resUnlockTasks.Contains(acceptUnlockTask))
                        resUnlockTasks.Add(acceptUnlockTask);
                }

                item.unlockTasks = resUnlockTasks;
                for (int j = 0; j < resUnlockTasks.Count; j++)
                {
                    int preTaskId = resUnlockTasks[j];
                    for (int t = 0; t < models.Count; t++)
                    {
                        var preTask = models[t];
                        if (preTask.id == preTaskId)
                        {
                            if (preTask.preUnlockTasks == null)
                                preTask.preUnlockTasks = new List<int>();
                            if (!preTask.preUnlockTasks.Contains(preTaskId))
                                preTask.preUnlockTasks.Add(preTaskId);
                        }

                    }
                }
            }
        }

        private List<int> GetUnlockTasks(TaskContent taskContent)
        {
            List<int> unlockTasks = new List<int>();

            void getUnlockTasks(List<TaskActionFunc> actionFuncs,ref List<int> resTaskIds)
            {
                foreach (var item in GetActFuncs(actionFuncs))
                {
                    if (item is TaskUnlockTaskFunc)
                    {
                        TaskUnlockTaskFunc unlockTaskFunc = (TaskUnlockTaskFunc)item;
                        if (!resTaskIds.Contains(unlockTaskFunc.taskId))
                            resTaskIds.Add(unlockTaskFunc.taskId);
                    }
                    else if (item is TaskTryUnlockTaskFunc)
                    {
                        TaskTryUnlockTaskFunc unlockTaskFunc = (TaskTryUnlockTaskFunc)item;
                        if (!resTaskIds.Contains(unlockTaskFunc.taskId))
                            resTaskIds.Add(unlockTaskFunc.taskId);
                    }
                }
            }

            getUnlockTasks(taskContent.actionFuncs,ref unlockTasks);
            getUnlockTasks(taskContent.actionSuccess, ref unlockTasks);
            getUnlockTasks(taskContent.actionFail, ref unlockTasks);

            return unlockTasks;
        }

        private List<TaskActionFunc> GetActFuncs(List<TaskActionFunc> actionFuncs)
        {
            List<TaskActionFunc> resActions = new List<TaskActionFunc>();
            for (int i = 0; i < actionFuncs.Count; i++)
            {
                TaskActionFunc tAct = actionFuncs[i];
                if (tAct is TaskBranchFunc)
                {
                    TaskBranchFunc branchFunc = tAct as TaskBranchFunc;
                    foreach (var item in branchFunc.branches)
                    {
                        resActions.AddRange(item.actionFuncs);
                    }
                }
                else
                {
                    resActions.Add(tAct);
                }
            }
            return resActions;
        }
    } 
}
