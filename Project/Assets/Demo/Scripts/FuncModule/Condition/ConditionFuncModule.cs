using System;
using System.Collections.Generic;
using Cnf;

namespace Demo.Condition
{
    public class ConditionFuncModule : FuncModule
    {
        private Dictionary<ConditionType, Func<ConditionType, string, bool>> condFuncDict =
            new Dictionary<ConditionType, Func<ConditionType, string, bool>>();
        
        public bool CheckIsTrue(ConditionType pType,string pParam)
        {
            if (pType == ConditionType.Null)
            {
                return true;
            }

            if (!condFuncDict.ContainsKey(pType))
            {
                GameLocate.Log.LogError("条件判断出错，没有注册该条件的检测函数",pType,pParam);
                return false;
            }

            return condFuncDict[pType].Invoke(pType, pParam);
        }
    }
}