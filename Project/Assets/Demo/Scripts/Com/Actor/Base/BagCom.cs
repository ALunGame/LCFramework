using LCECS.Core;
using System.Collections.Generic;

namespace Demo
{
    public class BagItem
    {
        public int id;
        public int cnt;

        /// <summary>
        /// -1无限
        /// </summary>
        public int maxCnt = -1;

        public BagItem()
        {

        }

        public BagItem(int id,int cnt,int maxCnt)
        {
            this.id = id;
            this.cnt = cnt;
            this.maxCnt = maxCnt;
        }

        public bool Add(int itemCnt)
        {
            int resCnt = cnt + itemCnt;
            if (maxCnt != -1 && resCnt > maxCnt)
                return false;
            cnt = resCnt;
            return true;
        }

        public bool Remove(int itemCnt)
        {
            int resCnt = cnt - itemCnt;
            if (resCnt < 0)
                return false;
            cnt = resCnt;
            return true;
        }

        public int GetLeftCnt()
        {
            return maxCnt - cnt;
        }

        public bool CheckIsOutMax()
        {
            if (maxCnt == -1)
                return false;
            return cnt >= maxCnt;
        }

        public override string ToString()
        {
            return $"Id:{id} Cnt:{cnt} Max:{maxCnt}";
        }
    }

    public class BagCom : BaseCom
    {
        public List<BagItem> itemlist = new List<BagItem>();

        public bool AddItem(int itemId,int itemCnt)
        {
            return GetBagItem(itemId).Add(itemCnt);
        }

        public BagItem GetBagItem(int itemId)
        {
            foreach (var item in itemlist)
            {
                if (item.id == itemId)
                {
                    return item;
                }
            }
            BagItem bagItem = new BagItem();
            bagItem.id      = itemId;
            bagItem.cnt     = 0;
            itemlist.Add(bagItem);
            return bagItem;
        }

        public bool RemoveItem(int itemId, int itemCnt)
        {
            return GetBagItem(itemId).Remove(itemCnt);
        }

        public bool CheckItemIsOutMax(int itemId)
        {
            return GetBagItem(itemId).CheckIsOutMax();
        }

        public void CoverItem(BagItem bagItem)
        {
            for (int i = 0; i < itemlist.Count; i++)
            {
                if (itemlist[i].id == bagItem.id)
                {
                    itemlist[i] = bagItem;
                }
            }
        }
    }
}