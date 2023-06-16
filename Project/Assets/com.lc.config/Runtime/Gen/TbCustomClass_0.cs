using MemoryPack;
using System;

namespace Cnf
{
    
    /// <summary>
    /// 物品信息
    /// </summary>
    [MemoryPackable]
    public partial class ItemInfo
    {
        
        /// <summary>
        /// 物品Id
        /// </summary>
        public int itemId;

        /// <summary>
        /// 物品数量
        /// </summary>
        public int itemCnt;

        [MemoryPackConstructor]
        public ItemInfo(){}

        public ItemInfo(int itemId,int itemCnt)
        {
            
            this.itemId=itemId;

            this.itemCnt=itemCnt;

        }

    }

}


namespace Cnf
{
    
    /// <summary>
    /// 动画信息
    /// </summary>
    [MemoryPackable]
    public partial class AnimInfo
    {
        
        /// <summary>
        /// 动画名
        /// </summary>
        public string animName;

        /// <summary>
        /// 动画播放次数
        /// </summary>
        public int animCnt;

        [MemoryPackConstructor]
        public AnimInfo(){}

        public AnimInfo(string animName,int animCnt)
        {
            
            this.animName=animName;

            this.animCnt=animCnt;

        }

    }

}


