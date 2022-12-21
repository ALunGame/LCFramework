using Demo.Com;
using LCECS;
using LCECS.Core;
using LCMap;
using LCToolkit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public enum MoveState
    {
        Stay_Ground,            //在地上待着
        Move_Ground,            //在地上移动
        Jump_Ground,            //在地上跳
        MoveJump_Ground,        //在地上移动和跳
        
        Move_Wall,              //在墙上移动
        Jump_Wall,              //在墙上跳
        MoveJump_Wall,          //在墙上移动和跳
        Grab_Wall,              //抓墙
        GrabJump_Wall,          //抓墙跳
        Hang_Wall,              //挂墙
        HangJump_Wall,          //挂墙跳
        
        Fall_Sky,               //空中掉落
        Move_Sky,               //在空中移动
        Jump_Sky,               //在空中跳
        MoveJump_Sky,           //在空中移动和跳
    }

    public class MoveInfo
    {
        //水平位移
        public float xDelta;
        
        public float moveSpeed;
        public float jumpSpeed;
        public bool needJump;

        public void Clear()
        {
            xDelta = 0;
            moveSpeed = 0;
            jumpSpeed = 0;
        }

        public bool IsZero()
        {
            return xDelta == 0 && moveSpeed == 0 && jumpSpeed == 0;
        }
    }
    
}

namespace Demo.Com
{
    public class MoveCom : BaseCom
    {
        [NonSerialized]
        private static Dictionary<MoveState, string> moveStateAnimDict = new Dictionary<MoveState, string>()
        {
            {MoveState.Stay_Ground,"idle"},
            {MoveState.Move_Ground,"run"},
            {MoveState.Jump_Ground,"jumpUp"},
            {MoveState.MoveJump_Ground,"jumpUp"},
            
            {MoveState.Grab_Wall,"climb"},
            {MoveState.Move_Wall,"climb"},
            {MoveState.Jump_Wall,"jumpUp"},
            {MoveState.MoveJump_Wall,"jumpUp"},
            
            {MoveState.Fall_Sky,"jumpUp"},
            {MoveState.Move_Sky,"jumpUp"},
            {MoveState.Jump_Sky,"jumpUp"},
            {MoveState.MoveJump_Sky,"jumpUp"},
        };

        public const float DefaultMass = 1;
        
        [NonSerialized]
        private Entity moveEntity;

        [NonSerialized]
        private MoveInfo currMoveInfo = new MoveInfo();
        public MoveInfo CurrentMoveInfo { get { return currMoveInfo; } }

        public Vector2 CurrVelocity;

        [NonSerialized]
        private TransCom moveTrans;
        
        [NonSerialized]
        private AnimCom animCom;

        [NonSerialized]
        private BasePropertyCom moveProperty;

        [NonSerialized]
        private BindableValue<MoveState> currMoveState = new BindableValue<MoveState>();
        public MoveState CurrMoveState { get { return currMoveState.Value; } }

        //质量
        public float Mass = DefaultMass;

        protected override void OnAwake(Entity pEntity)
        {
            moveEntity = pEntity;
            moveTrans = pEntity.GetCom<TransCom>();
            moveProperty = pEntity.GetCom<BasePropertyCom>();
            animCom = pEntity.GetCom<AnimCom>();
            currMoveState.SetValueWithoutNotify(MoveState.Stay_Ground);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            currMoveState.ClearChangedEvent();
        }

        #region Move

        /// <summary>
        /// 水平移动偏移
        /// </summary>
        /// <param name="pXDelta"></param>
        public void MoveDeltaX(float pXDelta)
        {
            currMoveInfo.xDelta = pXDelta;
        }

        /// <summary>
        /// 跳跃
        /// </summary>
        /// <param name="pYDelta"></param>
        public void Jump(float pJumpVelocity)
        {
            currMoveInfo.jumpSpeed = pJumpVelocity;
            currMoveInfo.needJump = true;
        }
        
        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="pYDelta"></param>
        public void Move(float pMoveVelocity)
        {
            currMoveInfo.moveSpeed = pMoveVelocity;
        }

        #endregion

        #region MoveState

        public void SetCurrMoveState(MoveState pMoveState)
        {
            currMoveState.Value= pMoveState;
            
            if (animCom != null)
            {
                if (moveStateAnimDict.ContainsKey(pMoveState))
                {
                    string animName = moveStateAnimDict[pMoveState];
                    if (animName == "jumpUp")
                    {
                        if (currMoveInfo.jumpSpeed <= 0)
                        {
                            animName = "jumpDown";
                        }
                    }
                    animCom.SetReqAnim(animName);
                }
            }
        }

        public void RegCurrMoveStateChange(Action<MoveState> pMoveStateChange)
        {
            currMoveState.RegisterValueChangedEvent(pMoveStateChange);
        }

        #endregion
    }
}
