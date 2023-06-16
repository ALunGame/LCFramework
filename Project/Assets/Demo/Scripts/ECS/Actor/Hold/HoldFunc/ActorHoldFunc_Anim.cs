using System;
using Demo.Com;
using Demo.Help;
using Demo.Hold;
using LCMap;

namespace Demo
{
    public class ActorHoldContext_Anim : ActorHoldContext
    {
        #region Static

        public const string PlayAnimFuncName = "PlayAnim";
        public const string PlayAnimCntFuncName = "PlayAnim";

        #endregion

        
        /// <summary>
        /// 动画名
        /// </summary>
        public string AnimName { get; private set; }

        private AnimLayer animLayer = AnimLayer.Side;
        /// <summary>
        /// 动画层级
        /// </summary>
        public AnimLayer AnimLayer
        {
            get
            {
                return animLayer;
            }
            private set
            {
                animLayer = value;
            }
        }

        /// <summary>
        /// 动画播放次数
        /// </summary>
        public int AnimCnt { get; private set; }
        
        /// <summary>
        /// 每次播放回调
        /// </summary>
        public Action PrePlayAnimCallBack { get; set; }
        
        /// <summary>
        /// 播放完成
        /// </summary>
        public Action FinishCallBack { get; set; }
        
        /// <summary>
        /// 动画方式
        /// </summary>
        public string AnimMode { get; private set; }

        public void PlayAnim(string pAnimName, AnimLayer pLayer = AnimLayer.Side)
        {
            AnimMode = PlayAnimFuncName;
            AnimName = pAnimName;
            animLayer = pLayer;
        }

        public void PlayAnimCnt(string pAnimName,int pCnt,AnimLayer pLayer = AnimLayer.Side)
        {
            AnimMode = PlayAnimCntFuncName;
            AnimName = pAnimName;
            AnimCnt = pCnt;
            animLayer = pLayer;
        }
    }
    
    public class ActorHoldFunc_Anim : ActorHoldFunc<ActorHoldContext_Anim>
    {
        protected override void OnEnter(Actor pActor)
        {
            if (Context.AnimMode == ActorHoldContext_Anim.PlayAnimFuncName)
            {
                ActorHelper.PlayAnim(pActor,Context.AnimName,Context.AnimLayer);
                ActorHoldRule.ReleaseActor(pActor,ActorHoldReason.Anim);
            }
            else if (Context.AnimMode == ActorHoldContext_Anim.PlayAnimCntFuncName)
            {
                ActorHelper.PlayAnimCnt(pActor,Context.AnimName,Context.AnimLayer,Context.AnimCnt,Context.PrePlayAnimCallBack,
                    () =>
                    {
                        ActorHoldRule.ReleaseActor(pActor,ActorHoldReason.Anim);
                    });
            }
        }

        protected override void OnExit(Actor pActor)
        {
            
        }
    }
}