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

        public BagItem(int id,int cnt,int maxCnt = -1)
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

        #region Get

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
            bagItem.id = itemId;
            bagItem.cnt = 0;
            itemlist.Add(bagItem);
            return bagItem;
        }

        public int GetItemCnt(int itemId)
        {
            if (CheckHasItem(itemId))
            {
                return GetBagItem(itemId).cnt;
            }
            return 0;
        }

        public int GetItemLeftAddCnt(int itemId)
        {
            if (CheckHasItem(itemId))
            {
                return GetBagItem(itemId).GetLeftCnt();
            }
            return 0;
        }

        #endregion

        #region Add

        public bool AddItem(int itemId, int itemCnt)
        {
            return GetBagItem(itemId).Add(itemCnt);
        }

        public void AddItem(BagItem pItem)
        {
            BagItem currItem = GetBagItem(pItem.id);
            int currLeftCnt = currItem.GetLeftCnt();
            if (currLeftCnt >= pItem.cnt)
            {
                currItem.Add(pItem.cnt);
                pItem.cnt = 0;
            }
            else
            {
                currItem.Add(currLeftCnt);
                pItem.cnt = pItem.cnt - currLeftCnt;
            }
        }

        #endregion

        #region Remove

        public bool RemoveItem(int itemId, int itemCnt)
        {
            return GetBagItem(itemId).Remove(itemCnt);
        }

        #endregion

        #region Check

        public bool CheckItemIsOutMax(int itemId)
        {
            return GetBagItem(itemId).CheckIsOutMax();
        }


        public bool CheckHasItem(int itemId)
        {
            foreach (var item in itemlist)
            {
                if (item.id == itemId)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

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