using LCECS.Core;
using LCMap;
using System;
using System.Collections.Generic;

namespace Demo.Com
{
    /// <summary>
    /// 管理者
    /// </summary>
    public class ManagerCom : BaseCom
    {
        public int buildingActorId;
        [NonSerialized]
        public ActorObj buildingActor;
        [NonSerialized]
        public BuildingCom buildingCom;
        [NonSerialized]
        public BagCom buildingBagCom;

        [NonSerialized]
        public Dictionary<int, List<ActorObj>> workers = new Dictionary<int, List<ActorObj>>();

        public void SetBuilding()
        {
            buildingActor       = MapLocate.Map.GetActor(buildingActorId);
            buildingCom         = buildingActor.Entity.GetCom<BuildingCom>();
            buildingBagCom      = buildingActor.Entity.GetCom<BagCom>();

            List<ActorObj> actors = new List<ActorObj>();
            foreach (var item in workers)
                actors.AddRange(item.Value);

            workers.Clear();

            foreach (var item in buildingBagCom.itemlist)
                workers.Add(item.id, new List<ActorObj>());

            foreach (var item in actors)
                AddWorker(item);
        }

        public void AddWorker(ActorObj workerActor)
        {
            int findKey  = 0;
            int actorCnt = 999;
            foreach (var item in workers)
            {
                if (item.Value.Count < actorCnt)
                {
                    actorCnt = item.Value.Count;
                    findKey = item.Key;
                }
            }

            if (workers[findKey].Contains(workerActor))
                return;
            workers[findKey].Add(workerActor);
            workerActor.Entity.GetOrCreateCom<CollectCom>().collectActorId = findKey;
        }

        public void RemoveWorker(ActorObj workerActor)
        {
            foreach (var item in workers)
            {
                if (item.Value.Contains(workerActor))
                {
                    item.Value.Remove(workerActor);
                }
            }
        }
    }
}
