using LCConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCTask
{
    /// <summary>
    /// ����������ϵ
    /// </summary>
    public enum ConditionType
    {
        AND,
        OR,
    }

    public static class TaskDef
    {
        public const string CnfRootPath = "Assets/Demo/Asset/Config/";

        /// <summary>
        /// �Ի���Ŀ¼
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
