using Demo.Com;
using LCMap;

namespace Demo
{
    public class StorageInteractive : ActorInteractive
    {
        protected override void OnExecute(ActorObj executeActor)
        {
            BuildingCom owerBuildingCom    = actor.Entity.GetCom<BuildingCom>();
            CollectCom executeCollectCom   = executeActor.Entity.GetCom<CollectCom>();

            if (executeCollectCom.collectItem.cnt <= 0)
            {
                GameLocate.Log.LogError("存储失败，没有物品", executeCollectCom.collectItem);
                ExecuteFinish();
                return;
            }

            executeCollectCom.collectItem = owerBuildingCom.Storage(executeActor, executeCollectCom.collectItem);
            ExecuteFinish();
        }
    }
}