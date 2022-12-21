using Demo.Com;
using LCMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class WorkFuncModule : FuncModule
    {
        private Actor managerActor;
        private List<Actor> workActors = new List<Actor>();
        private List<WorkCommand> waitCmds = new List<WorkCommand>();

        public void SetManager(Actor pActor)
        {
            managerActor = pActor;
        }
        
        public void AddWorkers(Actor pActor)
        {
            if (workActors.Contains(pActor))
                return;
            workActors.Add(pActor);
            if (waitCmds == null)
            {
                return;
            }
            for (int i = waitCmds.Count-1; i >= 0; i--)
            {
                if (SendWorkCommand(waitCmds[i]))
                {
                    waitCmds.RemoveAt(i);
                }
            }
            
        }
        
        public bool SendWorkCommand(WorkCommand pCmd)
        {
            List<Actor> resActors = new List<Actor>();
            foreach (var actor in workActors)
            {
                if (pCmd.CanTakeCommand(actor))
                    resActors.Add(actor);
            }

            if (resActors.Count <= 0)
            {
                waitCmds.Add(pCmd);
                GameLocate.Log.Log($"工作命令挂起，等待可以接受的工人>>>{pCmd}");
                return false;
            }

            Actor resActor = resActors[0];
            int cmdCnt = resActors[0].GetCom<WorkerCom>().GetCmdCnt();
            foreach (var item in resActors)
            {
                if (item.GetCom<WorkerCom>().GetCmdCnt() < cmdCnt)
                {
                    resActor = item;
                }
            }
            resActor.GetCom<WorkerCom>().AddWorkCmd(pCmd);
            return true;
        }

        /// <summary>
        /// 获得产出指定物品的演员
        /// </summary>
        /// <returns></returns>
        public List<Actor> GetOutPutItemActor(int pItemId, Actor pCheckActor)
        {
            List<Actor> actors = new List<Actor>();
            MapArea tArea = MapLocate.Map.GetAreaByActor(pCheckActor);
            foreach (var item in tArea.Actors)
            {
                if (item.Value.GetCom(out OutputItemCom outputItemCom))
                {
                    if (outputItemCom.CheckIsOutputItem(pItemId))
                    {
                        actors.Add(item.Value);
                    }
                }
            }
            return actors;
        }
    }
}
