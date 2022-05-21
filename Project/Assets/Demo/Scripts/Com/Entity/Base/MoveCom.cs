using Demo.BevNode;
using LCECS.Core;
using LCMap;
using LCToolkit;
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
        [NonSerialized]
        public Rigidbody2D Rig;

        [NonSerialized]
        public MoveType CurrMoveType;

        /// <summary>
        /// 移动类型冷却
        /// </summary>
        [NonSerialized]
        public float MoveTypeCd = 0;

        /// <summary>
        /// 没有请求移动
        /// </summary>
        [NonSerialized]
        public bool HasNoReqMove = false;

        /// <summary>
        /// 请求的移动速度
        /// </summary>
        [NonSerialized]
        public float ReqMoveSpeed;

        /// <summary>
        /// 请求的跳跃速度
        /// </summary>
        [NonSerialized]
        public float ReqJumpSpeed;

        /// <summary>
        /// 第几段跳跃
        /// </summary>
        [NonSerialized]
        public int JumpStep;

        protected override void OnInit(GameObject go)
        {
            Rig = go.GetComponent<Rigidbody2D>();
        }

        private void OnDisplayGoChange(GameObject displayGo)
        {
            Rig = displayGo.transform.Find("Collider").GetComponent<Rigidbody2D>();
        }
    }
}