using MemoryPack;
using System;
using System.Collections.Generic;
using Cnf;

namespace Cnf
{
    
    /// <summary>
    /// 物品配方信息
    /// </summary>
    [MemoryPackable]
    public partial class ItemRecipeCnf
    {
        
        /// <summary>
        /// 物品Id
        /// </summary>
        public int id;

        /// <summary>
        /// 物品配方
        /// </summary>
        public List<ItemInfo> recipes;

    }

}

