using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 在执行伤害流程时，拥有这个Buff作为攻击者执行的函数
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="damageInfo"></param>
    /// <param name="target"></param>
    public delegate void BuffOnHit(BuffObj buff, ref AddDamageInfo damageInfo, SkillCom target);

    /// <summary>
    /// 在执行伤害流程时，拥有这个Buff作为攻击者执行的函数
    /// </summary>
    public static class BuffHitFunc
    {
        private static Dictionary<string, BuffOnHit> funcDict = new Dictionary<string, BuffOnHit>();

        /// <summary>
        /// 添加执行的函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="func">函数</param>
        public static void AddExecuteFunc(string funcName, BuffOnHit func)
        {
            if (funcDict.ContainsKey(funcName))
            {
                SkillLocate.Log.LogError("添加BuffTickFunc出错，重复函数名", funcName);
                return;
            }
            funcDict.Add(funcName, func);
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="buff">Buff对象</param>
        /// <param name="modifyStack">改变的层数</param>
        public static void ExecuteFunc(string funcName, BuffObj buff, ref AddDamageInfo damageInfo, SkillCom target)
        {
            if (!funcDict.ContainsKey(funcName))
                return;
            BuffOnHit func = funcDict[funcName];
            func.Invoke(buff,ref damageInfo,target);
        }
    }
}
