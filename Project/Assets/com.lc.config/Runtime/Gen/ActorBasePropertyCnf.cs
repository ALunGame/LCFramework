using MemoryPack;
using System;

namespace Cnf
{
    
    /// <summary>
    /// 演员基础属性
    /// </summary>
    [MemoryPackable]
    public partial class ActorBasePropertyCnf
    {
        
        /// <summary>
        /// 演员Id
        /// </summary>
        public int id;

        /// <summary>
        /// 生命值
        /// </summary>
        public int hp;

        /// <summary>
        /// 移动速度
        /// </summary>
        public float moveSpeed;

        /// <summary>
        /// 跳跃速度
        /// </summary>
        public float jumpSpeed;

        /// <summary>
        /// 攻击
        /// </summary>
        public float attack;

        /// <summary>
        /// 防御
        /// </summary>
        public float defense;

        /// <summary>
        /// 行动速度
        /// </summary>
        public float actionSpeed;

    }

}

