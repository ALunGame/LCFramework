using LCJson;
using LCLoad;
using System.Collections.Generic;

namespace LCTask
{
    public class TaskConfigServer
    {
        private Dictionary<int, TaskModel> configDict = new Dictionary<int, TaskModel>();

        public void Init()
        {
            string assetName = TaskDef.GetTaskCnfName();
            string jsonStr   = LoadHelper.LoadString(assetName);
            if (string.IsNullOrEmpty(jsonStr))
            {
                TaskLocate.Log.LogError("任务配置加载失败没有资源", assetName);
            }
            else
            {
                List<TaskModel> tasks = JsonMapper.ToObject<List<TaskModel>>(jsonStr);
                for (int i = 0; i < tasks.Count; i++)
                {
                    configDict.Add(tasks[i].id, tasks[i]);
                }
            }
        }

        public void Clear()
        {
            configDict.Clear();
        }

        public bool GetTaskModel(int pTaskId,out TaskModel model)
        {
            model = default;
            if (!configDict.ContainsKey(pTaskId))
            {
                TaskLocate.Log.LogError("没有任务配置", pTaskId);
                return false;
            }
            model = configDict[pTaskId];
            return true;
        }
    } 
}
