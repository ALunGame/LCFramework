using Demo.Com;
using LCMap;
using LCECS;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Server
{
    public enum WorkType
    {
        Repair,
        Collect,
        Protect,
    }

    public class WorkServer
    {
        public List<int> createActors = new List<int>() {220,221,222,223,224 };

        public void Init()
        {
            ECSLocate.ECS.GetWorld().GetCom<DayNightCom>().RegStageChange(OnDayChange);
        }

        private void OnDayChange(DayNightStage stage)
        {
            //每天早晨创建村民
            if (stage == DayNightStage.Morning)
            {
                int actorIdIndex        = Random.Range(0, createActors.Count);
                Vector3 createPos       = MapLocate.Map.GetActor(341).transform.position;
                ActorModel actorModel   = new ActorModel(createActors[actorIdIndex], ActorType.Villager, createPos);
                MapLocate.Map.CreateActor(actorModel, MapLocate.Map.CurrArea);
            }
        }


        public void AfterMapInit()
        {
            foreach (ActorObj managerActor in MapLocate.Map.GetActors(typeof(ManagerCom).FullName))
            {
                SetManagerBuilding(managerActor.Entity.GetCom<ManagerCom>());
            }
            foreach (ActorObj workerActor in MapLocate.Map.GetActors(typeof(WorkerCom).FullName))
            {
                AddWorker(workerActor.Entity.GetCom<WorkerCom>());
            }
        }

        public void AddWorker(WorkerCom workerCom)
        {
            ActorObj managerActor  = MapLocate.Map.GetActor(workerCom.managerActorId);
            if (managerActor == null)
                return;

            workerCom.managerActor = managerActor;

            ActorObj workerActor   = MapLocate.Map.GetActor(workerCom.EntityUid);
            ManagerCom managerCom  = managerActor.Entity.GetCom<ManagerCom>();
            managerCom.RemoveWorker(workerActor);
            managerCom.AddWorker(workerActor);
        }

        public void SetManagerBuilding(ManagerCom managerCom)
        {
            managerCom.SetBuilding();
        }
    }
}
