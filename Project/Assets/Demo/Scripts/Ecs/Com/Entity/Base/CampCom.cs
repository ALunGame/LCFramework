using LCECS.Core;
using System;
using System.Collections;
using UnityEngine;

namespace Demo.Com
{
    public enum CampType
    {
        /// <summary>
        /// 中立
        /// </summary>
        Neutral = 1,    
        /// <summary>
        /// 友方
        /// </summary>
        Friend  = 2,
        /// <summary>
        /// 敌方
        /// </summary>
        Enemy   = 3,
    }

    /// <summary>
    /// 阵营组件
    /// </summary>
    [Serializable]
    public class CampCom : BaseCom
    {
        /// <summary>
        /// 阵营
        /// </summary>
        public CampType Camp;
    }
}