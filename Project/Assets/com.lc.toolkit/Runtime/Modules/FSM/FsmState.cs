﻿using LCMap;

namespace LCToolkit.FSM
{
    public class FsmStateContext
    {
    }
    
    /// <summary>
    /// 有限状态机状态基类。
    /// </summary>
    public abstract class FsmState
    {
        /// <summary>
        /// 状态拥有者
        /// </summary>
        public Actor Owner { get; private set; }

        /// <summary>
        /// 状态机
        /// </summary>
        protected Fsm Fsm;
        
        /// <summary>
        /// 上下文数据
        /// </summary>
        private FsmStateContext context;
        
        protected T GetContext<T>() where T : FsmStateContext
        {
            if (context == null)
            {
                return null;
            }

            return context as T;
        }

        public void SetContext(FsmStateContext pContext)
        {
            context = pContext;
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pOwner"></param>
        /// <param name="pFsm"></param>
        public void Init(Actor pOwner, Fsm pFsm)
        {
            Owner = pOwner;
            Fsm = pFsm;
            OnInit();
        }

        public bool Evaluate()
        {
            return OnEvaluate();
        }

        public void Enter()
        {
            OnEnter();
        }

        public void Update(float pDeltaTime, float pRealElapseSeconds)
        {
            OnUpdate(pDeltaTime,pRealElapseSeconds);
        }

        public void Leave()
        {
            context = null;
            OnLeave();
        }

        public void Destroy()
        {
            context = null;
            OnDestroy();
        }

        #region 子类重写

        /// <summary>
        /// 初始化
        /// </summary>
        protected internal virtual void OnInit()
        {
        }
        
        /// <summary>
        /// 评估状态是否可以进入，当其他状态离开时，检测
        /// </summary>
        /// <returns></returns>
        public virtual bool OnEvaluate()
        {
            return true;
        }

        /// <summary>
        /// 状态进入
        /// </summary>
        protected internal virtual void OnEnter()
        {
        }
        
        /// <summary>
        /// 状态更新
        /// </summary>
        protected internal virtual void OnUpdate(float pDeltaTime, float pRealElapseSeconds)
        {
        }

        /// <summary>
        /// 状态离开
        /// </summary>
        protected internal virtual void OnLeave()
        {
        }
        
        /// <summary>
        /// 状态销毁
        /// </summary>
        protected internal virtual void OnDestroy()
        {
        }

        /// <summary>
        /// 自动切换状态
        /// </summary>
        protected void AutoChangeState()
        {
            Fsm.OnStateLeave(this);
        }

        #endregion
        
    }
}