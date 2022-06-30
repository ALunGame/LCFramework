using Demo.Com;
using LCMap;

namespace Demo
{
    public class StorageInteractive : ActorInteractive
    {
        protected override void OnExecute(ActorObj executeActor)
        {
            BuildingCom owerBuildingCom     = actor.Entity.GetCom<BuildingCom>();
            BagCom owerBuildingBagCom       = actor.Entity.GetCom<BagCom>();

            CollectCom executeCollectCom    = executeActor.Entity.GetCom<CollectCom>();
            BagCom executeBagCom            = executeActor.Entity.GetCom<BagCom>();

            BagItem storageItem = executeBagCom.GetBagItem(executeCollectCom.collectActorId);
            if (storageItem.cnt <= 0)
            {
                GameLocate.Log.LogError("存储失败，没有物品", executeCollectCom.collectActorId);
                ExecuteFinish();
                return;
            }

            executeBagCom.CoverItem(owerBuildingCom.Storage(executeActor, storageItem, owerBuildingBagCom));
            ExecuteFinish();
        }
    }
}