using LCMap;

namespace Demo.Life.State.Content
{
    public enum WorkState
    {
        /// <summary>
        /// 等待
        /// </summary>
        Wait,
        /// <summary>
        /// 正在做
        /// </summary>
        Doing,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 失败
        /// </summary>
        Fail,
    }
    
    /// <summary>
    /// Command 模式复杂了，
    /// 1，核心功能就是向工作者提供工作内容，完成工作后，告诉工作发起者工作完成
    ///
    ///
    /// </summary>
    public abstract class ActorWorkContent
    {
        /// <summary>
        /// 工作内容发起者
        /// </summary>
        public Actor Originator { get; set; }
        
        /// <summary>
        /// 工作内容执行者
        /// </summary>
        public Actor Executor { get; set; }
        public ActorLifeCom ExecutorLifeCom { get; set; }
        
        /// <summary>
        /// 工作中
        /// </summary>
        public WorkState State { get; set; }

        /// <summary>
        /// 是否可以做工作
        /// </summary>
        public abstract bool CanDoWork();

        /// <summary>
        /// 做事
        /// </summary>
        /// <returns></returns>
        public void DoWork()
        {
            State = WorkState.Doing;
            OnDoWork();
        }
        
        /// <summary>
        /// 做事
        /// </summary>
        /// <returns></returns>
        public abstract void OnDoWork();

        public void WorkWait()
        {
            State = WorkState.Wait;
        }

        public void WorkSuccess()
        {
            State = WorkState.Success;
        }

        public void WorkFail()
        {
            State = WorkState.Fail;
        }
    }
}