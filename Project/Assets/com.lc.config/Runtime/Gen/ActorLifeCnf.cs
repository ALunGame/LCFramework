using MemoryPack;
using System;

namespace Cnf
{
    
    /// <summary>
    /// 演员生活
    /// </summary>
    [MemoryPackable]
    public partial class ActorLifeCnf
    {
        
        /// <summary>
        /// 演员Id
        /// </summary>
        public int id;

        /// <summary>
        /// 生产策略Id
        /// </summary>
        public int produceId;

        /// <summary>
        /// 采集策略Id
        /// </summary>
        public int collectId;

    }

}

