using LCECS;
using LCECS.Core;
using System;
using System.Collections.Generic;

namespace LCTask
{
    public class TaskSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() {typeof(TaskCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            TaskCom taskCom = GetCom<TaskCom>(comList[0]);
            DealTask(taskCom);
        }

        private void DealTask(TaskCom taskCom)
        {
            
        }
    }
}
