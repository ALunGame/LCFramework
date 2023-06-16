using System;
using Demo.Com;
using LCMap;

namespace Demo
{
    /// <summary>
    /// 对于Actor的辅助方法
    /// </summary>
    public static partial class ActorHelper
    {
        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="pActor">演员</param>
        /// <param name="pAnimName">动画名</param>
        /// <param name="pLayer">动画层</param>
        public static void PlayAnim(Actor pActor, string pAnimName, AnimLayer pLayer = AnimLayer.Side)
        {
            pActor?.AnimCom.PlayAnim(pAnimName,pLayer);
        }
        
        /// <summary>
        /// 播放指定次数的动画
        /// </summary>
        /// <param name="pActor">演员</param>
        /// <param name="pAnimName">动画名</param>
        /// <param name="pCnt">次数</param>
        /// <param name="pPreCallBack">每次回调</param>
        /// <param name="pFinishCallBack">完成回调</param>
        /// <param name="pLayer">动画层</param>
        public static void PlayAnimCnt(Actor pActor, string pAnimName,int pCnt, Action pPreCallBack, Action pFinishCallBack, AnimLayer pLayer = AnimLayer.Side)
        {
            PlayAnimCnt(pActor, pAnimName, pLayer, pCnt, pPreCallBack, pFinishCallBack);
        }
        
        /// <summary>
        /// 播放指定次数的动画
        /// </summary>
        /// <param name="pActor">演员</param>
        /// <param name="pAnimName">动画名</param>
        /// <param name="pLayer">动画层</param>
        /// <param name="pCnt">次数</param>
        /// <param name="pPreCallBack">每次回调</param>
        /// <param name="pFinishCallBack">完成回调</param>
        public static void PlayAnimCnt(Actor pActor, string pAnimName, AnimLayer pLayer, int pCnt, Action pPreCallBack, Action pFinishCallBack)
        {
            pActor?.AnimCom.PlayAnimCnt(pAnimName,pLayer,pCnt,pPreCallBack,pFinishCallBack).Forget();
        }
    }
}