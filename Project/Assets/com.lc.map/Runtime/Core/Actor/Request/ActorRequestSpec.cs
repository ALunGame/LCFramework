using System;

namespace LCMap
{
    public class ActorRequestSpec
    {
        /// <summary>
        /// 模板数据
        /// </summary>
        public ActorRequest Model { get; private set; }

        /// <summary>
        /// 持有者
        /// </summary>
        public Actor Owner { get; private set; }
        
        private Action RequestFinishCallBack;

        public ActorRequestSpec(Actor pOwnerActor, ActorRequest pModel)
        {
            Model = pModel;
            Owner = pOwnerActor;
        }
        
        public virtual void OnEnter(params object[] pParams)
        {
            Model.OnEnter(this,pParams);
        }
        
        public virtual void OnExit()
        {
            Model.OnExit(this);
        }

        public int ReqId()
        {
            return Model.RequestId;
        }

        public void SetFinishCallBack(Action pCallBack)
        {
            RequestFinishCallBack = pCallBack;
        }

        public void ExecuteFinishCallBack()
        {
            Action func = RequestFinishCallBack;
            RequestFinishCallBack = null;
            func?.Invoke();
        }

        public void ClearFinishCallBack()
        {
            RequestFinishCallBack = null;
        }
    }
}