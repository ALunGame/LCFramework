using LCECS.Core;
using LCMap;
using System;
using System.Collections.Generic;

namespace Demo.Com
{
    public class StorageRecord
    {
        public int itemId;
        public int itemCnt;

        public StorageRecord(int itemId,int itemCnt)
        {
            this.itemId = itemId;
            this.itemCnt = itemCnt;
        }
    }


    public class BuildingCom : BaseCom
    {
        [NonSerialized]
        public List<int> owerActorIds       = new List<int>();

        [NonSerialized]
        public List<ActorObj> owerActors    = new List<ActorObj>();

        //存储记录
        [NonSerialized]
        public Dictionary<string, List<StorageRecord>> storageRecords = new Dictionary<string, List<StorageRecord>>();

        //偷盗记录
        [NonSerialized]
        public Dictionary<string, List<StorageRecord>> stealRecords = new Dictionary<string, List<StorageRecord>>();

        //存储的物品
        public List<BagItem> storageItems   = new List<BagItem>();

        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="bagItem">返回剩余的物品</param>
        public BagItem Storage(ActorObj actor, BagItem bagItem)
        {
            foreach (var item in storageItems)
            {
                if (item.id == bagItem.id)
                {
                    if (item.CheckIsOutMax())
                    {
                        GameLocate.Log.LogWarning("满了！！！！无法存储", item,bagItem);
                        return bagItem;
                    }
                    int leftCnt = item.GetLeftCnt() - bagItem.cnt;
                    if (leftCnt > 0)
                    {
                        AddStorageRecord(actor, bagItem.id, item.GetLeftCnt());
                        item.Add(item.GetLeftCnt());
                        bagItem.cnt = leftCnt;
                    }
                    else
                    {
                        AddStorageRecord(actor, bagItem.id, bagItem.cnt);
                        item.Add(bagItem.cnt);
                        bagItem.cnt = 0;
                    }
                    return bagItem;
                }
            }
            GameLocate.Log.LogWarning("该建筑无法存储该物品！！！！", bagItem);
            return bagItem;
        }

        private void AddStorageRecord(ActorObj actor, int itemId,int itemCnt)
        {
            if (!stealRecords.ContainsKey(actor.Uid))
            {
                stealRecords.Add(actor.Uid, new List<StorageRecord>());
                stealRecords[actor.Uid].Add(new StorageRecord(itemId,itemCnt));
                return;
            }
            foreach (var item in stealRecords[actor.Uid])
            {
                if (item.itemId == itemId)
                {
                    item.itemCnt += itemCnt;
                }
            }
        }
    }
}