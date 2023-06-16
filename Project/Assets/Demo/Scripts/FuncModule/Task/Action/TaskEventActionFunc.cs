using LCTask;

namespace Demo.Task
{
    /// <summary>
    /// 事件完成
    /// </summary>
    public class TaskFinishEventFunc : TaskActionFunc
    {
        public int eventId;
        public bool sucess;

        protected override TaskActionState OnStart(TaskObj taskObj)
        {
            GameLocate.FuncModule.Event.FinishEvent(eventId,sucess);
            return TaskActionState.Finished;
        }

        protected override void OnClear(TaskObj taskObj)
        {
        }
        
    }
}