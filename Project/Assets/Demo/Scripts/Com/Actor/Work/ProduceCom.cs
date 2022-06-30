using LCECS.Core;
using LCMap;
using System;
using System.Collections.Generic;
using LCConfig;
using Demo.Config;

namespace Demo.Com
{
    public class ProduceCom : BaseCom
    {
        public List<int> produceIds = new List<int>();

        [NonSerialized]
        public List<int> currProduces = new List<int>();

        /// <summary>
        /// 获得可以制作的产品Id
        /// </summary>
        /// <returns></returns>
        public List<int> GetCanMakeProduceIds(BagCom bagCom)
        {
            Dictionary<int, int> itemDict = new Dictionary<int, int>();
            foreach (var item in bagCom.itemlist)
                itemDict.Add(item.id, item.cnt);

            List<int> resIds = new List<int>();
            foreach (int produceId in produceIds)
            {
                ProduceCnf produceCnf = LCConfig.Config.ProduceCnf[produceId];
                if (produceCnf.CheckCanMake(itemDict))
                {
                    foreach (var item in produceCnf.costItems)
                    {
                        itemDict[item.id] = itemDict[item.id] - item.cnt;
                    }
                    resIds.Add(produceId);
                }
            }
            return resIds;
        }
    }
}