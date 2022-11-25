using LCLoad;
using LCMap;
using UnityEngine;

namespace Demo.Server
{
    public class ItemServer
    {
        private int startUid = -100;
        private int entityId = 501;
        private string itemPrefab = "Actor_Item";
        private GameObject itemRoot;

        public Actor CreateItem(int itemId,int itemCnt,Vector3 itemPos)
        {
            if (itemRoot == null)
                itemRoot = new GameObject("ItemList");

            ActorInfo itemActor = new ActorInfo();
            itemActor.uid = $"item_{startUid--}";
            itemActor.id  = entityId;

            GameObject itemAsset = LoadHelper.LoadPrefab(itemPrefab);
            GameObject itemGo    = GameObject.Instantiate(itemAsset, itemRoot.transform);
            itemGo.transform.position = itemPos;
            Actor actorObj    = ActorCreator.CreateEntity(itemActor);
            actorObj.SetBindGo(itemGo);

            return actorObj;
        }
    }
}
