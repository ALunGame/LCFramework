using LCECS.Core;
using System;
using System.Collections.Generic;
using LCMap;

namespace Demo.Com
{
    public class WorkerCom : BaseCom
    {
        [NonSerialized]
        protected Stack<WorkCommand> workCmds = new();

        [NonSerialized]
        private WorkCommand currCmd;

        protected override void OnAwake(Entity pEntity)
        {
            GameLocate.FuncModule.Work.AddWorkers(pEntity as Actor);
        }

        public int GetCmdCnt()
        {
            return workCmds.Count;
        }

        public void AddWorkCmd(WorkCommand pCmd)
        {
            workCmds.Push(pCmd);
            pCmd.SetExecutor(LCMap.MapLocate.Map.GetActor(EntityUid));
        }

        public void Work()
        {
            if (currCmd != null)
            {
                HandleCmd();
                return;
            }
            if (workCmds.Count <= 0)
                return;
            currCmd = workCmds.Peek();
            HandleCmd();
        }

        private void HandleCmd()
        {
            if (currCmd.CanExecute())
            {
                currCmd.Execute();
            }
        }

        public void WorkFinish(WorkCommand pCmd)
        {
            if (!pCmd.Equals(currCmd))
            {
                GameLocate.Log.LogError($"工作完成失败，完成的请求:{pCmd}不是当前请求:{currCmd}");
                return;
            }
            currCmd = null;
            workCmds.Pop();
        }
    }
}
