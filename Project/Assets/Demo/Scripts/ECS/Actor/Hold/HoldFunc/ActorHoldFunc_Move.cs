
using System;
using System.Collections.Generic;
using Demo.Hold;
using LCMap;
using UnityEngine;

namespace Demo
{
    
    public class ActorHoldFunc_MoveContext : ActorHoldContext
    {
        #region Static

        public const string MoveByAStar = "MoveByAStar";
        public const string MoveByPath = "MoveByPath";
        public const string MoveToPoint = "MoveToPoint";
        
        #endregion
        
        /// <summary>
        /// 移动完成回调
        /// </summary>
        public Action FinishCallBack { get; set; }
        
        /// <summary>
        /// 移动失败回调
        /// </summary>
        public Action FailCallBack { get; set; }
        
        /// <summary>
        /// 移动中每帧回调
        /// </summary>
        public Action<Vector3> PreframeCallBack { get; set; }
        
        /// <summary>
        /// 移动中切换路径点回调
        /// </summary>
        public Action<Vector3, Vector3, Vector3Int> PrePointCallBack { get; set; }

        /// <summary>
        /// 移动方式
        /// </summary>
        public string MoveMode { get; private set; }
        
        /// <summary>
        /// 目标位置
        /// </summary>
        public Vector3 TargetPos { get; private set; }
        
        /// <summary>
        /// 指定路径
        /// </summary>
        public List<Vector3> MovePoints { get; private set; }
        
        public void SetAStarTarget(Vector3 pTargetPos)
        {
            MoveMode = MoveByAStar;
            TargetPos = pTargetPos;
        }

        public void SetMovePath(List<Vector3> pMovePoints)
        {
            MoveMode = MoveByPath;
            MovePoints = pMovePoints;
        }
        
        public void SetMovePoint(Vector3Int pTargetPos)
        {
            MoveMode = MoveToPoint;
            TargetPos = pTargetPos;
        }
    }
    
    public class ActorHoldFunc_Move : ActorHoldFunc<ActorHoldFunc_MoveContext>
    {
        protected override void OnEnter(Actor pActor)
        {
            pActor.MoveCom.SetCallBack(() =>
            {
                ActorHoldRule.ReleaseActor(pActor,ActorHoldReason.None);
                Context.FinishCallBack?.Invoke();
            },Context.PrePointCallBack,Context.PreframeCallBack,Context.FailCallBack);
            
            if (Context.MoveMode == ActorHoldFunc_MoveContext.MoveByAStar)
            {
                ActorHelper.MoveByAStar(pActor,Context.TargetPos);
            }
            else if (Context.MoveMode == ActorHoldFunc_MoveContext.MoveByPath)
            {
                ActorHelper.MoveByPath(pActor,Context.MovePoints);
            }
            else if (Context.MoveMode == ActorHoldFunc_MoveContext.MoveToPoint)
            {
                ActorHelper.MoveToPoint(pActor,Context.TargetPos);
            }
        }

        protected override void OnExit(Actor pActor)
        {
            ActorHelper.StopMove(pActor);
        }
    }
}