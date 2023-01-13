using LCMap;
using System;
using Demo.Com;

namespace Demo
{
    public abstract class WorkCommand
    {
        //命令发起者
        protected Actor originator;

        //命令执行者
        protected Actor executor;
        protected WorkerCom executorWorkerCom;

        public event Action<WorkCommand> workFinishCallBack;
        private bool isExcuting = false;

        public override string ToString()
        {
            string originatorUid = originator == null ? "null" : originator.Uid;
            string executorUid = executor == null ? "null" : executor.Uid;
            return $"命令:{GetType()} 发起者:{originatorUid} --> 执行者:{executorUid}";
        }

        public void SetOriginator(Actor pActor)
        {
            originator = pActor;
        }

        public void SetExecutor(Actor pActor)
        {
            executor = pActor;
            executorWorkerCom = pActor.GetCom<WorkerCom>();
            OnCommandTook();
        }

        public void Execute()
        {
            if (isExcuting)
                return;
            isExcuting = true;
            OnExecute();
        }

        public void ExecuteFinish()
        {
            executorWorkerCom.WorkFinish(this);
            Action<WorkCommand> func = workFinishCallBack;
            workFinishCallBack = null;
            func?.Invoke(this);
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
