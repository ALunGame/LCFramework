using Demo.Server;
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

        public WorkType workType;
        public int managerActorId;

        [NonSerialized]
        public Actor managerActor;
    }
}
