using LCECS.Core;
using System.Collections.Generic;

namespace Demo
{
    public class BagItem
    {
        public int id;
        public int cnt;
        public int maxCnt;
    }

    public class BagCom : BaseCom
    {
        public List<BagItem> itemlist = new List<BagItem>();

        public void AddItem(int itemId,int itemCnt)
        {
            for (int i = 0; i < itemlist.Count; i++)
            {
                if (itemlist[i].id == itemId)
                {
                    int resCnt = itemlist[i].cnt + itemCnt;
                    if (resCnt > itemlist[i].maxCnt)
                    {
                        return;
                    }
                    itemlist[i].cnt = resCnt;
                    return;
                }
            }
            BagItem bagItem = new BagItem();
            bagItem.id = itemId;
            bagItem.cnt = itemCnt;
            itemlist.Add(bagItem);
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
            return null;
        }

        public bool RemoveItem(int itemId, int itemCnt)
        {
            for (int i = 0; i < itemlist.Count; i++)
            {
                if (itemlist[i].id == itemId)
                {
                    if (itemlist[i].cnt < itemCnt)
                    {
                        return false;
                    }
                    else
                    {
                        itemlist[i].cnt -= itemCnt;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckItemIsOutMax(int itemId)
        {
            BagItem bagItem = GetBagItem(itemId);
            if (bagItem == null)
            {
                return false;
            }
            return bagItem.cnt>=bagItem.maxCnt;
        }
    }
}