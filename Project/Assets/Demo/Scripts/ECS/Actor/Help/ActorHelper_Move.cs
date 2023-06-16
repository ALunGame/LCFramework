using System;
using System.Collections.Generic;
using LCMap;
using UnityEngine;

namespace Demo
{
    public static partial class ActorHelper
    {
        /// <summary>
        /// 设置移动回调
        /// </summary>
        /// <param name="pActor">演员</param>
        /// <param name="finishCallBack">完成回调</param>
        /// <param name="prePointCallBack">每一路径点回调</param>
        /// <param name="preframeCallBack">每帧回调</param>
        /// <param name="failCallBack">失败回调（寻路才会失败）</param>
        public static void SetMoveCallBack(Actor pActor, Action finishCallBack, Action<Vector3, Vector3, Vector3Int> prePointCallBack = null, Action<Vector3> preframeCallBack = null, Action failCallBack = null)
        {
            pActor?.MoveCom.SetCallBack(finishCallBack,prePointCallBack,preframeCallBack,failCallBack);
        }

        /// <summary>
        /// 寻路移动到指定位置
        /// </summary>
        /// <param name="pActor">演员</param>
        /// <param name="pTargetPos">世界坐标</param>
        public static void MoveByAStar(Actor pActor, Vector3 pTargetPos)
        {
            pActor?.MoveCom.MoveByAStar(pTargetPos);
        }

        /// <summary>
        /// 路径移动
        /// </summary>
        /// <param name="pActor">演员</param>
        /// <param name="pMovePoints">路径点</param>
        public static void MoveByPath(Actor pActor, List<Vector3> pMovePoints)
        {
            pActor?.MoveCom.MoveByPath(pMovePoints);
        }

        /// <summary>
        /// 移动到指定点
        /// </summary>
        /// <param name="pActor">演员</param>
        /// <param name="pTargetPoint"></param>
        public static void MoveToPoint(Actor pActor, Vector3 pTargetPoint)
        {
            pActor?.MoveCom.MoveToPoint(pTargetPoint);
        }
        
        /// <summary>
        /// 停止移动
        /// </summary>
        /// <param name="pActor"></param>
        public static void StopMove(Actor pActor)
        {
            pActor?.MoveCom.Stop();
        }
    }
}