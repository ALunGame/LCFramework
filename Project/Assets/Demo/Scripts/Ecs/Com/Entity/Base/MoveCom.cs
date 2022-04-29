using LCECS.Core;
using System;
using System.Collections;
using UnityEngine;

namespace Demo.Com
{
    /// <summary>
    /// 移动组件
    /// </summary>
    [Serializable]
    public class MoveCom : BaseCom
    {
        /// <summary>
        /// 移动的节点
        /// </summary>
        [NonSerialized]
        public Transform Trans;

        /// <summary>
        /// 当前速度
        /// </summary>
        public Vector2 Velocity;

        protected override void OnInit(GameObject go)
        {
            Trans = go.transform;
        }
    }
}