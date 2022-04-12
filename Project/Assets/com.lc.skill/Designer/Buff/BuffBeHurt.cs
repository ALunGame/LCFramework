using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 在执行伤害流程时，拥有这个Buff作为挨打者执行的函数
    /// </summary>
    /// <param name="buff">Buff对象</param>
    /// <param name="damageInfo">伤害</param>
    /// <param name="attacker">攻击者</param>
    public delegate void BuffOnBeHurt(BuffObj buff, ref AddDamageInfo damageInfo, SkillCom attacker);

    /// <summary>
    /// 在执行伤害流程时，拥有这个Buff作为挨打者执行的函数
    /// </summary>
    public static class BuffBeHurt
    {
        private static Dictionary<string, BuffOnBeHurt> funcDict = new Dictionary<string, BuffOnBeHurt>();

        /// <summary>
        /// 添加执行的函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="func">函数</param>
        public static void AddExecuteFunc(string funcName, BuffOnBeHurt func)
        {
            if (funcDict.ContainsKey(funcName))
            {
                SkillLocate.Log.LogError("添加BuffBeHurt出错，重复函数名", funcName);
                return;
            }
            funcDict.Add(funcName, func);
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="buff">Buff对象</param>
        /// <param name="attacker">攻击者</param>
        public static void ExecuteFunc(string funcName, BuffObj buff, ref AddDamageInfo damageInfo, SkillCom attacker)
        {
            if (!funcDict.ContainsKey(funcName))
                return;
            BuffOnBeHurt func = funcDict[funcName];
            func.Invoke(buff, ref damageInfo, attacker);
        }
    }
}
