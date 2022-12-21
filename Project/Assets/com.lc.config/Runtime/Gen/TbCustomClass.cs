using System;

namespace Config
{
    
    /// <summary>
    /// 物品信息
    /// </summary>
    public class ItemInfo
    {
        
        /// <summary>
        /// 物品Id
        /// </summary>
        public int itemId;

        /// <summary>
        /// 物品数量
        /// </summary>
        public int itemCnt;

        public ItemInfo(){}

        public ItemInfo(int itemId,int itemCnt)
        {
            
            this.itemId=itemId;

            this.itemCnt=itemCnt;

        }

    }

}


