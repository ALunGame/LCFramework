using System;
using LCMap;

namespace Demo.Hold
{
    /// <summary>
    /// 演员持有规则
    /// 1，切换的规则
    /// 2，持有的默认操作
    /// </summary>
    public static class ActorHoldRule
    {
        /// <summary>
        /// 持有演员
        /// </summary>
        /// <param name="pActor">演员</param>
        /// <param name="pReason">持有原因</param>
        /// <param name="pContext">持有环境</param>
        /// <param name="pHoldSuccessCallBack">持有成功回调</param>
        /// <returns>持有结果</returns>
        public static bool HoldActor(Actor pActor, ActorHoldReason pReason, ActorHoldContext pContext = null, Action pHoldSuccessCallBack = null)
        {
            if (pActor == null)
                return false;
            
            ActorHoldReason currReason = pActor.CurrHoldReason;
            int currWeight = ActorHoldMapping.ReasonWeight[currReason];
            
            int changeWeight = ActorHoldMapping.ReasonWeight[pReason];
            
            if (changeWeight >= currWeight)
            {
                ActorHoldMapping.OnReleaseActor(pActor);
                
                pHoldSuccessCallBack?.Invoke();

                pActor.CurrHoldReason = pReason;
                ActorHoldMapping.OnHoldActor(pActor,pContext == null ? ActorHoldContext.NullContext : pContext);
                
                return true;
            }
            return false;
        }


        /// <summary>
        /// 释放演员
        /// </summary>
        /// <param name="pActor">演员</param>
        /// <param name="pReason">原因</param>
        public static void ReleaseActor(Actor pActor, ActorHoldReason pReason)
        {
            if (pActor == null)
                return;
            
            if (pActor.CurrHoldReason != pReason)
                return;
            
            ActorHoldMapping.OnReleaseActor(pActor);
            
            pActor.CurrHoldReason = ActorHoldReason.None;
        }
    }
}