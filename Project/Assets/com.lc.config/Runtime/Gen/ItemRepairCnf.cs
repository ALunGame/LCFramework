using System;
using System.Collections.Generic;
using Config;

namespace Demo.Config
{
    
    /// <summary>
    /// 物品修复配置
    /// </summary>
    public class ItemRepairCnf
    {
        
        /// <summary>
        /// 物品Id
        /// </summary>
        public int id;

        /// <summary>
        /// 每格生命修复需要的材料
        /// </summary>
        public List<ItemInfo> repairs;

    }

}

