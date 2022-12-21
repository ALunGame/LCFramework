using System;
using System.Collections.Generic;
using Demo.Com;
using LCECS.Core;

namespace Demo.Scripts.System.Actor
{
    public class WorkSystem : BaseSystem
    {
        protected override List<Type> RegContainListenComs()
        {
            return new List<Type>() {typeof(WorkerCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            WorkerCom workerCom = GetCom<WorkerCom>(comList[0]);
            workerCom.Work();
        }
    }
}