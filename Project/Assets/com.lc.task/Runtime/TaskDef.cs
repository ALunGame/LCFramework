using LCConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCTask
{
    /// <summary>
    /// 任务条件关系
    /// </summary>
    public enum ConditionRelated
    {
        AND,
        OR,
    }

    public static class TaskDef
    {
        public const string CnfRootPath = "Assets/Demo/Asset/Config/";

        /// <summary>
        /// 对话根目录
        /// </summary>
        public const string DialogRootPath = CnfRootPath + "Task/";

        public static string GetTaskCnfName()
        {
            return ConfigDef.GetCnfNoExName($"Task");
        }

        public static string GetTaskCnfPath()
        {
            return DialogRootPath + ConfigDef.GetCnfName($"Task");
        }
    }
}
