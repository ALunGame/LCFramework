using MemoryPack;
using System;
using System.Collections.Generic;
using Cnf;

namespace Cnf
{
    
    /// <summary>
    /// 物品修复信息
    /// </summary>
    [MemoryPackable]
    public partial class ItemRepairCnf
    {
        
        /// <summary>
        /// 物品Id
        /// </summary>
        public int id;

        /// <summary>
        /// 每次增加的Hp
        /// </summary>
        public int addHp;

        /// <summary>
        /// 增加的Hp需要的材料
        /// </summary>
        public List<ItemInfo> repairs;

    }

}

