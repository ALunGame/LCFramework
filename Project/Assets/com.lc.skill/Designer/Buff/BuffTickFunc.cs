using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCSkill
{
    /// <summary>
    /// Buff在tickTime间隔执行的函数
    /// </summary>
    /// <param name="buff">Buff对象</param>
    public delegate void BuffOnTick(BuffObj buff);

    /// <summary>
    /// Buff在tickTime间隔执行的函数
    /// </summary>
    public static class BuffTickFunc
    {
        private static Dictionary<string, BuffOnTick> funcDict = new Dictionary<string, BuffOnTick>();

        /// <summary>
        /// 添加执行的函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="func">函数</param>
        public static void AddExecuteFunc(string funcName, BuffOnTick func)
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
        public static void ExecuteFunc(string funcName, BuffObj buff)
        {
            if (!funcDict.ContainsKey(funcName))
                return;
            BuffOnTick func = funcDict[funcName];
            func.Invoke(buff);
        }
    }
}
