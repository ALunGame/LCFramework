using MemoryPack;
using System;

namespace Cnf
{
    
    /// <summary>
    /// 物品信息
    /// </summary>
    [MemoryPackable]
    public partial class ItemCnf
    {
        
        /// <summary>
        /// 物品Id
        /// </summary>
        public int id;

        /// <summary>
        /// 物品名字
        /// </summary>
        public string name;

    }

}

