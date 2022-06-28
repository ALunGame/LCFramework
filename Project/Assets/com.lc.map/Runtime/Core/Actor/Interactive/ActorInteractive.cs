using System;

namespace LCMap
{
    public class ActorInteractive
    {
        [NonSerialized]
        protected ActorObj actor;

        public void Init(ActorObj owerActor)
        {
            this.actor = owerActor;
        }

        /// <summary>
        /// 执行交互
        /// </summary>
        /// <param name="executeActor">请求交互的演员</param>
        public void Execute(ActorObj executeActor)
        {
            actor.SetCurrInteractive(this);
            OnExecute(executeActor);
        }

        /// <summary>
        /// 交互完成
        /// </summary>
        public void ExecuteFinish()
        {
            actor.SetCurrInteractive(null);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void OnInit()
        {

        }

        /// <summary>
        /// 当执行交互时
        /// </summary>
        /// <param name="executeActor">请求交互的演员</param>
        protected virtual void OnExecute(ActorObj executeActor)
        {

        }
    }
}
