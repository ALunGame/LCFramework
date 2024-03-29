﻿using Demo.Behavior;
using LCECS.Core;
using System;
using UnityEngine;

namespace Demo.Com
{
    /// <summary>
    /// 移动组件
    /// </summary>
    [Serializable]
    public class PlayerMoveCom : BaseCom
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

        /// <summary>
        /// 请求设置位置
        /// </summary>
        [NonSerialized]
        public Vector3 ReqMove;


        protected override void OnAwake(Entity pEntity)
        {
            BindGoCom bindGoCom = pEntity.GetCom<BindGoCom>();
            if (bindGoCom != null)
            {
                bindGoCom.RegGoChange((go) =>
                {
                    Rig = go.GetComponent<Rigidbody2D>();
                });
            }
        }
    }
}