using LCECS.Core;
using LCMap;
using System;

namespace Demo.Com
{
    /// <summary>
    /// 工人
    /// </summary>
    public class WorkerCom : BaseCom
    {
        public int managerActorId;

        [NonSerialized]
        public ActorObj managerActor;
    }
}
