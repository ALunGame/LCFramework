using Demo.Com;
using LCMap;

namespace Demo.Server
{
    public class WorkServer
    {
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
