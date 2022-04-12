using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCSkill
{
    /// <summary>
    /// 当释放一个技能时执行的函数
    /// 为了处理当技能释放时，更改技能表现，比如没有魔法还要释放，会执行没有魔法的动画
    /// </summary>
    /// <param name="buff">buff对象</param>
    /// <param name="skill">技能对象</param>
    /// <param name="timeline">返回技能表现</param>
    /// <returns></returns>
    public delegate TimelineObj BuffOnFreed(BuffObj buff, SkillObj skill, TimelineObj timeline);

    /// <summary>
    /// 当释放一个技能时执行的函数
    /// </summary>
    public static class BuffFreedFunc
    {
        private static Dictionary<string, BuffOnFreed> funcDict = new Dictionary<string, BuffOnFreed>();

        /// <summary>
        /// 添加执行的函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="func">函数</param>
        public static void AddExecuteFunc(string funcName, BuffOnFreed func)
        {
            if (funcDict.ContainsKey(funcName))
            {
                SkillLocate.Log.LogError("添加 BuffFreedFunc 出错，重复函数名", funcName);
                return;
            }
            funcDict.Add(funcName, func);
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="buff">buff对象</param>
        /// <param name="skill">技能对象</param>
        /// <param name="timeline">技能表现</param>
        public static TimelineObj ExecuteFunc(string funcName, BuffObj buff, SkillObj skill, TimelineObj timeline)
        {
            if (!funcDict.ContainsKey(funcName))
                return timeline;
            BuffOnFreed func = funcDict[funcName];
            return func.Invoke(buff, skill, timeline);
        }
    }
}
