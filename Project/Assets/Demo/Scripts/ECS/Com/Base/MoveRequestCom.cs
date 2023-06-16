
using System;
using Demo.Com;
using LCECS;
using LCECS.Core;
using LCMap;
using UnityEngine;

namespace Demo
{
    public abstract class MoveRequestInfo
    {
        private Action finishCallBack;

        public void SetFinishCallBack(Action pFinishCallBack)
        {
            finishCallBack = pFinishCallBack;
        }
        
        public void Finish()
        {
            if (IsFinish())
            {
                Action func = finishCallBack;
                finishCallBack = null;
                func?.Invoke();
            }
        }

        /// <summary>
        /// 获得水平方向的位移
        /// </summary>
        /// <returns></returns>
        public abstract float GetXDeltaMove();
        
        /// <summary>
        /// 检测是否完成
        /// </summary>
        /// <returns></returns>
        public abstract bool IsFinish();
    }

    /// <summary>
    /// 移动到指定位置
    /// </summary>
    public class MoveToPosRequest : MoveRequestInfo
    {
        public const float FinishDis = 0.02f;
        
        public Vector3 targetPos;
        public TransCom transCom;
        public BasePropertyCom propertyCom;

        public override float GetXDeltaMove()
        {
            int dir = targetPos.x > transCom.Pos.x ? 1 : -1;
            return propertyCom.MoveSpeed.Curr * Time.deltaTime * dir;   
        }

        public override bool IsFinish()
        {
            if (transCom == null)
            {
                return true;
            }

            float dis = Vector3.Distance(transCom.Pos, targetPos);
            return dis <= FinishDis;
        }
    }
    
    /// <summary>
    /// 移动到演员交互内
    /// </summary>
    public class MoveToActorInteractiveRangeRequest : MoveRequestInfo
    {
        public const float FinishDis = 0.02f;

        public Actor targetActor;
        public TransCom targetTransCom;
        
        public TransCom transCom;
        public BasePropertyCom propertyCom;


        public MoveToActorInteractiveRangeRequest(Actor pTargetActor)
        {
            targetActor = pTargetActor;
            targetTransCom = pTargetActor.Trans;
        }

        public override float GetXDeltaMove()
        {
            int dir = targetTransCom.Pos.x > transCom.Pos.x ? 1 : -1;
            return propertyCom.MoveSpeed.Curr * Time.deltaTime * dir;
        }

        public override bool IsFinish()
        {
            if (transCom == null || targetActor==null)
                return true;

            float dis = Vector3.Distance(transCom.Pos, targetTransCom.Pos);
            return dis <= targetActor.InteractiveCom.interactiveDis;
        }
    } 
}

namespace Demo.Com
{
    public class MoveRequestCom : BaseCom
    {
        [NonSerialized]
        private TransCom transCom;
        [NonSerialized]
        private MoveCom moveCom;
        [NonSerialized]
        private BasePropertyCom propertyCom;
        [NonSerialized]
        private MoveRequestInfo currRequestInfo;

        protected override void OnAwake(Entity pEntity)
        {
            transCom = pEntity.GetCom<TransCom>();
            propertyCom = pEntity.GetCom<BasePropertyCom>();
            moveCom = pEntity.GetCom<MoveCom>();
        }

        public bool IsNull()
        {
            return currRequestInfo == null;
        }
        
        public bool IsFinish()
        {
            if (currRequestInfo == null)
            {
                return true;
            }

            return currRequestInfo.IsFinish();
        }

        public void Clear()
        {
            currRequestInfo = null;
        }
        
        public void Finsih()
        {
            if (currRequestInfo == null)
            {
                return;
            }
            moveCom.CurrentMoveInfo.Clear();
            MoveRequestInfo requestInfo = currRequestInfo;
            currRequestInfo = null;
            requestInfo.Finish();
        }

        public float GetXDeltaPos()
        {
            if (currRequestInfo == null)
            {
                return 0;
            }

            return currRequestInfo.GetXDeltaMove();
        }

        #region Move

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        /// <param name="pTargetPos"></param>
        /// <param name="pFInishCallBack"></param>
        public void MoveToPos(Vector3 pTargetPos,Action pFInishCallBack)
        {
            MoveToPosRequest request = new MoveToPosRequest();
            request.propertyCom = propertyCom;
            request.transCom = transCom;
            request.targetPos = pTargetPos;
            request.SetFinishCallBack(pFInishCallBack);
            currRequestInfo = request;
        }
        
        /// <summary>
        /// 移动到指定演员交互范围内
        /// </summary>
        /// <param name="pTargetActor"></param>
        /// <param name="pFInishCallBack"></param>
        public void MoveToActorInteractiveRange(Actor pTargetActor,Action pFInishCallBack)
        {
            MoveToActorInteractiveRangeRequest request = new MoveToActorInteractiveRangeRequest(pTargetActor);
            request.propertyCom = propertyCom;
            request.transCom = transCom;
            request.SetFinishCallBack(pFInishCallBack);
            currRequestInfo = request;
        }

        #endregion

        #region Static

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        /// <param name="pEntity"></param>
        /// <param name="pTargetPos"></param>
        /// <param name="pFInishCallBack"></param>
        public static void MoveToPos(Entity pEntity,Vector3 pTargetPos,Action pFInishCallBack)
        {
            if (pEntity.GetCom(out MoveRequestCom requestCom))
            {
                requestCom.MoveToPos(pTargetPos,pFInishCallBack);
            }
        }
        
        /// <summary>
        /// 移动到指定演员交互范围内
        /// </summary>
        /// <param name="pEntity"></param>
        /// <param name="pTargetActor"></param>
        /// <param name="pFInishCallBack"></param>
        public static void MoveToActorInteractiveRange(Entity pEntity,Actor pTargetActor,Action pFInishCallBack)
        {
            if (pEntity.GetCom(out MoveRequestCom requestCom))
            {
                requestCom.MoveToActorInteractiveRange(pTargetActor,pFInishCallBack);
            }
        }

        #endregion
    }
}