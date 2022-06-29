using LCECS.Core;
using System;

namespace Demo.Com
{
    public class CollectCom : BaseCom
    {
        //采集的演员Id
        public int collectActorId;

        //采集最大数量
        public int collectMaxCnt;

        [NonSerialized]
        public BagItem collectItem;

        public void ChangeCollectActorId(int actorId)
        {
            collectActorId = actorId;
            collectItem = new BagItem(actorId,0, collectMaxCnt);
        }
    }
}
