using Demo.Com;
using LCMap;

namespace Demo
{
    public class StorageInteractive : ActorInteractive
    {
        private int _type = (int)InteractiveType.Storage;
        public override int Type { get => _type; }

        protected override bool OnExecute(Actor executeActor)
        {
            BuildingCom owerBuildingCom     = actor.GetCom<BuildingCom>();
            BagCom owerBuildingBagCom       = actor.GetCom<BagCom>();

            CollectCom executeCollectCom    = executeActor.GetCom<CollectCom>();
            BagCom executeBagCom            = executeActor.GetCom<BagCom>();

            BagItem storageItem = executeBagCom.GetBagItem(executeCollectCom.collectActorId);
            if (storageItem.cnt <= 0)
            {
                GameLocate.Log.LogError("存储失败，没有物品", executeCollectCom.collectActorId);
                return true;
            }

            executeBagCom.CoverItem(owerBuildingCom.Storage(executeActor, storageItem, owerBuildingBagCom));
            return true;
        }
    }
}