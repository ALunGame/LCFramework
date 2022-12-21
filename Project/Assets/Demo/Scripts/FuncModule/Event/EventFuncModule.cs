using Demo.Config;
using Demo.UserData;

namespace Demo.Event
{
    /// <summary>
    /// 事件模块
    /// </summary>
    public class EventFuncModule : FuncModule
    {
        public override void OnInit()
        {
            foreach (int eventId in LCConfig.Config.EventCnf.Keys)
            {
                SendEvent(eventId);
            }
        }

        private void SendEvent(int pEventId)
        {
            if (!CheckEventCanUnlock(pEventId))
                return;
            EventCnf cnf = LCConfig.Config.EventCnf[pEventId];
            //更新数据
            EventData.Instance.AddCurrEvent(cnf.id);
            TaskData.Instance.AddAcceptTask(cnf.startTaskId);
        }

        public bool CheckEventCanUnlock(int pEventId)
        {
            if (!LCConfig.Config.EventCnf.ContainsKey(pEventId))
            {
                return false;
            }
            
            EventCnf cnf = LCConfig.Config.EventCnf[pEventId];
            return GameLocate.FuncModule.Condition.CheckIsTrue(cnf.cond,cnf.condParam);
        }
    }
}