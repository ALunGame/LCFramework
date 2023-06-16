using LCECS.Core;
using System;
using UnityEngine;

namespace Demo.Com
{
    /// <summary>
    /// 注视环绕
    /// </summary>
    public class GazeSurroundCom : BaseCom
    {
        /// <summary>
        /// 注视的实体Uid
        /// </summary>
        [NonSerialized]
        public string gazeUid;

        [NonSerialized]
        public DirType moveDir = DirType.None;

        /// <summary>
        /// 环绕范围
        /// </summary>
        [NonSerialized]
        public Vector2 gazeRange;
    }
}