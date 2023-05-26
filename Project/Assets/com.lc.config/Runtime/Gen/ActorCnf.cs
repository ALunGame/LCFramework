using MemoryPack;
using System;
using LCMap;
using System.Collections.Generic;
using System;

namespace LCMap
{
    
    /// <summary>
    /// 演员信息
    /// </summary>
    [MemoryPackable]
    public partial class ActorCnf
    {
        
        /// <summary>
        /// 演员Id
        /// </summary>
        public int id;

        /// <summary>
        /// 演员名
        /// </summary>
        public string name;

        /// <summary>
        /// 演员类型
        /// </summary>
        public ActorType type;

        /// <summary>
        /// 实体Id
        /// </summary>
        public int entityId;

        /// <summary>
        /// 预制体
        /// </summary>
        public string prefab;

        /// <summary>
        /// 交互范围
        /// </summary>
        public int interactiveRange;

        /// <summary>
        /// 移动速度
        /// </summary>
        public int moveSpeed;

        /// <summary>
        /// 默认技能
        /// </summary>
        public List<int> defaultSkills;

        /// <summary>
        /// 默认Buff
        /// </summary>
        public List<int> defaultBuffs;

    }

}

