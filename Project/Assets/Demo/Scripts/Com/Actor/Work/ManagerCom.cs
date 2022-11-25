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
        public Actor buildingActor;
        [NonSerialized]
        public BuildingCom buildingCom;
        [NonSerialized]
        public BagCom buildingBagCom;

        [NonSerialized]
        public Dictionary<int, List<Actor>> workers = new Dictionary<int, List<Actor>>();

        public void SetBuilding()
        {
            buildingActor       = MapLocate.Map.GetActor(buildingActorId);
            buildingCom         = buildingActor.GetCom<BuildingCom>();
            buildingBagCom      = buildingActor.GetCom<BagCom>();

            List<Actor> actors = new List<Actor>();
            foreach (var item in workers)
                actors.AddRange(item.Value);

            workers.Clear();

            foreach (var item in buildingBagCom.itemlist)
                workers.Add(item.id, new List<Actor>());

            foreach (var item in actors)
                AddWorker(item);
        }

        public void AddWorker(Actor workerActor)
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
            workerActor.GetOrCreateCom<CollectCom>().collectActorId = findKey;
        }

        public void RemoveWorker(Actor workerActor)
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
