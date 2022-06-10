using System;
using UnityEngine;

namespace LCNode.Model
{
    public abstract partial class BaseNode
    {
        /// <summary> 唯一标识 </summary>
        internal string guid;
        /// <summary> 位置坐标 </summary>
        internal Vector2 position;
        /// <summary> 输入索引 </summary>
        internal int inIndex = -1;
        /// <summary> 输出索引 </summary>
        internal int outIndex = -1;
    }
}
