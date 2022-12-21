using LCMap;
using System;

namespace Demo
{
    public abstract class WorkCommand
    {
        //命令发起者
        protected Actor originator;

        //命令执行者
        protected Actor executor;

        public event Action<WorkCommand> workFinishCallBack;
        private bool isExcuting = false;

        public void SetOriginator(Actor pActor)
        {
            originator = pActor;
        }

        public void SetExecutor(Actor pActor)
        {
            executor = pActor;
            OnCommandTook();
        }

        public void Execute(Action<WorkCommand> pFinishCallBack)
        {
            if (isExcuting)
                return;
            isExcuting = true;
            
            if (pFinishCallBack != null)
                workFinishCallBack += pFinishCallBack;
            
            OnExecute();
        }

        public void ExecuteFinish()
        {
            workFinishCallBack?.Invoke(this);
        }

        public void ExecuteFail()
        {
            isExcuting = false;
        }

        /// <summary>
        /// 是否可以持有命令
        /// </summary>
        /// <param name="pWorkActor">工作演员</param>
        /// <returns>是否持有</returns>
        public abstract bool CanTakeCommand(Actor pWorkActor);

        /// <summary>
        /// 当命令被持有
        /// </summary>
        protected abstract void OnCommandTook();

        /// <summary>
        /// 命令是否可以执行
        /// </summary>
        /// <returns></returns>
        public abstract bool CanExecute();

        /// <summary>
        /// 提交工作请求
        /// </summary>
        protected abstract void OnExecute();
    }
}
