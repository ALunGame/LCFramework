using System.Collections;
using UnityEngine;

namespace Demo
{
    public enum InteractiveType
    {
        /// <summary>
        /// 添加物品，移除交互演员物品
        /// </summary>
        AddItem,
        
        /// <summary>
        /// 移除物品，添加交互演员物品
        /// </summary>
        RemoveItem,
    }
}