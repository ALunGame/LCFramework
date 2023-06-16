using MemoryPack;
using System;
using System.Collections.Generic;
using Cnf;
using Cnf;

namespace Cnf
{
    
    /// <summary>
    /// 采集工作策略
    /// </summary>
    [MemoryPackable]
    public partial class ActorCollectCnf
    {
        
        /// <summary>
        /// 策略Id
        /// </summary>
        public int id;

        /// <summary>
        /// 采集物品
        /// </summary>
        public List<ItemInfo> workeCollect;

        /// <summary>
        /// 采集耗时-秒
        /// </summary>
        public float time;

        /// <summary>
        /// 采集动画
        /// </summary>
        public AnimInfo anim;

    }

}

