using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// 当Buff被添加、改变层数时执行的函数
    /// </summary>
    /// <param name="buff">Buff对象</param>
    /// <param name="modifyStack">改变的层数</param>
    public delegate void BuffOnOccur(BuffObj buff, int modifyStack);

    /// <summary>
    /// 当Buff被添加、改变层数时执行的函数
    /// </summary>
    public static class BuffOccurFunc
    {
        private static Dictionary<string, BuffOnOccur> funcDict = new Dictionary<string, BuffOnOccur>();

        /// <summary>
        /// 添加执行的函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="func">函数</param>
        public static void AddExecuteFunc(string funcName, BuffOnOccur func)
        {
            if (funcDict.ContainsKey(funcName))
            {
                SkillLocate.Log.LogError("添加BuffOccurFunc出错，重复函数名",funcName);
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
        public static void ExecuteFunc(string funcName, BuffObj buff, int modifyStack)
        {
            if (!funcDict.ContainsKey(funcName))
                return;
            BuffOnOccur func = funcDict[funcName];
            func.Invoke(buff, modifyStack);
        }
    }
}
