using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCSkill
{
    public delegate void TimelineEvent(TimelineObj timeline, params object[] args);

    public static class TimelineNodeFunc
    {
        private static Dictionary<string, TimelineEvent> funcDict = new Dictionary<string, TimelineEvent>();

        /// <summary>
        /// 添加执行的函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="func">函数</param>
        public static void AddExecuteFunc(string funcName, TimelineEvent func)
        {
            if (funcDict.ContainsKey(funcName))
            {
                SkillLocate.Log.LogError("添加 TimelineNodeFunc 出错，重复函数名", funcName);
                return;
            }
            funcDict.Add(funcName, func);
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="funcName">函数名</param>
        /// <param name="timelineObj">timeline对象</param>
        /// <param name="skill">技能对象</param>
        /// <param name="timeline">技能表现</param>
        public static void ExecuteFunc(string funcName, TimelineObj timelineObj, params object[] args)
        {
            if (!funcDict.ContainsKey(funcName))
                return;
            TimelineEvent func = funcDict[funcName];
            func.Invoke(timelineObj, args);
        }
    }
}
