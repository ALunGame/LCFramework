using MemoryPack;
using System;
using System.Collections.Generic;
using System;
using Cnf;

namespace Cnf
{
    
    /// <summary>
    /// 生产工作策略
    /// </summary>
    [MemoryPackable]
    public partial class ActorProduceCnf
    {
        
        /// <summary>
        /// 策略Id
        /// </summary>
        public int id;

        /// <summary>
        /// 产出物品Id
        /// </summary>
        public List<int> workeOutput;

        /// <summary>
        /// 生产耗时-秒
        /// </summary>
        public float time;

        /// <summary>
        /// 生产动画
        /// </summary>
        public AnimInfo anim;

    }

}

