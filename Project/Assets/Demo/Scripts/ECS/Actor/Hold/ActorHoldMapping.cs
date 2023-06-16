using System.Collections.Generic;

namespace Demo
{
    /// <summary>
    /// 演员持有原因
    /// 1，值大大于等于覆盖
    /// </summary>
    public enum ActorHoldReason
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        
        /// <summary>
        /// 被AI持有
        /// </summary>
        AI,
        
        /// <summary>
        /// 移动
        /// </summary>
        Move,
        
        /// <summary>
        /// 播放动画
        /// </summary>
        Anim,
    }
    
    public static class ActorHoldMapping
    {
        /// <summary>
        /// 持有原因权重
        /// 1，大于等于覆盖
        /// </summary>
        public static Dictionary<ActorHoldReason, int> ReasonWeight = new Dictionary<ActorHoldReason, int>()
        {
            {ActorHoldReason.None, 0},
            {ActorHoldReason.AI, 0},
            {ActorHoldReason.Move, 1},
            {ActorHoldReason.Anim, 1},
        };


        /// <summary>
        /// 持有默认执行函数
        /// </summary>
        public static Dictionary<ActorHoldReason, IActorHoldFunc> ReasonFunc =
            new Dictionary<ActorHoldReason, IActorHoldFunc>()

            {
                {ActorHoldReason.None, new ActorHoldFunc_None()},
                {ActorHoldReason.AI, new ActorHoldFunc_None()},
                {ActorHoldReason.Move, new ActorHoldFunc_Move()},
            };


        public static void OnHoldActor(LCMap.Actor pActor, ActorHoldContext pContext)
        {
            if (pActor == null)
                return;
            
            ActorHoldReason currReason = pActor.CurrHoldReason;
            if (!ReasonFunc.ContainsKey(currReason))
                return;
            
            ReasonFunc[currReason].Enter(pActor, pContext);
        }
        
        
        public static void OnReleaseActor(LCMap.Actor pActor)
        {
            if (pActor == null)
            {
                return;
            }
            
            ActorHoldReason currReason = pActor.CurrHoldReason;
            if (!ReasonFunc.ContainsKey(currReason))
                return;
            
            ReasonFunc[currReason].Exit(pActor);
        }
        
    }
    
    /// <summary>
    /// 演员持有环境
    /// </summary>
    public class ActorHoldContext
    {
        /// <summary>
        /// 空环境
        /// </summary>
        public static ActorHoldContext NullContext = new ActorHoldContext();
    }

    public interface IActorHoldFunc
    {
        void Enter(LCMap.Actor pActor, ActorHoldContext pContext);
        void Exit(LCMap.Actor pActor);
    }
    
    /// <summary>
    /// 演员持有时，默认函数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActorHoldFunc<T> : IActorHoldFunc where T : ActorHoldContext
    {
        public T Context { get; set; }

        public void Enter(LCMap.Actor pActor, ActorHoldContext pContext)
        {
            if (pContext is T)
            {
                Context = (T)pContext;
                OnEnter(pActor);
            }
            else
            {
                GameLocate.Log.LogError("持有出错，环境不匹配",typeof(T),pContext.GetType());
            }
            
        }
        
        protected virtual void OnEnter(LCMap.Actor pActor){}
        

        public void Exit(LCMap.Actor pActor)
        {
            OnExit(pActor);
        }
        
        protected virtual void OnExit(LCMap.Actor pActor){}
    }

    /// <summary>
    /// 空的执行函数
    /// </summary>
    public class ActorHoldFunc_None : ActorHoldFunc<ActorHoldContext>
    {
    }
}