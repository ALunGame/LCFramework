using System.Collections.Generic;
using Cnf;
using Demo.Config;
using Demo.UserData;
using LCToolkit;

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

        public void FinishEvent(int pEventId,bool pSucess)
        {
            EventCnf cnf = LCConfig.Config.EventCnf[pEventId];
            
            if (pSucess)
            {
                EventData.Instance.AddFinsihEvent(pEventId);
            }
            else
            {
                EventData.Instance.RemoveEvent(pEventId);
            }

            int nextEventId = pSucess ? cnf.nextSuccess : cnf.nextFail;
            List<ItemInfo> rewards = pSucess ? cnf.successReward : cnf.failReward;

            //奖励
            if (rewards.IsLegal())
            {
                if (LCECS.ECSLocate.Player.GetPlayerEntity().GetCom<BagCom>(out BagCom bagCom))
                {
                    for (int i = 0; i < rewards.Count; i++)
                    {
                        bagCom.AddItem(rewards[i]);
                    }
                }
                
            }

            //下个事件
            if (LCConfig.Config.EventCnf.ContainsKey(nextEventId))
            {
                SendEvent(nextEventId);
            }
        }
    }
}