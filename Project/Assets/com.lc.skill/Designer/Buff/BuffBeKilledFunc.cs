using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 在执行伤害流程时，拥有这个Buff被杀死执行的函数
    /// </summary>
    /// <param name="buff">buff对象</param>
    /// <param name="damageInfo">伤害信息</param>
    /// <param name="attacker">攻击者</param>
    public delegate void BuffOnBeKilled(BuffObj buff, AddDamageInfo damageInfo, SkillCom attacker);

    /// <summary>
    /// 在执行伤害流程时，拥有这个Buff被杀死执行的函数
    /// </summary>
    public static class BuffBeKilledFunc
    {
        private static Dictionary<string, BuffOnBeKilled> funcDict = new Dictionary<string, BuffOnBeKilled>();

        /// <summary>
        /// 添加执行的函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="func">函数</param>
        public static void AddExecuteFunc(string funcName, BuffOnBeKilled func)
        {
            if (funcDict.ContainsKey(funcName))
            {
                SkillLocate.Log.LogError("添加 BuffBeKilledFunc 出错，重复函数名", funcName);
                return;
            }
            funcDict.Add(funcName, func);
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="buff">buff对象</param>
        /// <param name="damageInfo">伤害信息</param>
        /// <param name="attacker">攻击者</param>
        public static void ExecuteFunc(string funcName, BuffObj buff, AddDamageInfo damageInfo, SkillCom attacker)
        {
            if (!funcDict.ContainsKey(funcName))
                return;
            BuffOnBeKilled func = funcDict[funcName];
            func.Invoke(buff, damageInfo, attacker);
        }
    }
}
