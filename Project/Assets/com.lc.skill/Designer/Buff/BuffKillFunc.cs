using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 在执行伤害流程时，如果击杀目标执行的函数
    /// </summary>
    /// <param name="buff">Buff对象</param>
    /// <param name="damageInfo">伤害信息</param>
    /// <param name="target">被击杀目标</param>
    public delegate void BuffOnKill(BuffObj buff, AddDamageInfo damageInfo, SkillCom target);

    /// <summary>
    /// 在执行伤害流程时，如果击杀目标执行的函数
    /// </summary>
    public static class BuffKillFunc
    {
        private static Dictionary<string, BuffOnKill> funcDict = new Dictionary<string, BuffOnKill>();

        /// <summary>
        /// 添加执行的函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="func">函数</param>
        public static void AddExecuteFunc(string funcName, BuffOnKill func)
        {
            if (funcDict.ContainsKey(funcName))
            {
                SkillLocate.Log.LogError("添加 BuffKillFunc 出错，重复函数名", funcName);
                return;
            }
            funcDict.Add(funcName, func);
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="buff">Buff对象</param>
        /// <param name="damageInfo">伤害信息</param>
        /// <param name="target">被击杀目标</param>
        public static void ExecuteFunc(string funcName, BuffObj buff, AddDamageInfo damageInfo, SkillCom target)
        {
            if (!funcDict.ContainsKey(funcName))
                return;
            BuffOnKill func = funcDict[funcName];
            func.Invoke(buff, damageInfo, target);
        }
    }
}
