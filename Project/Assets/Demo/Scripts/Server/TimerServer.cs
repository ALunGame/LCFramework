using Demo.Com;
using System;

namespace Demo.Server
{
    public class TimerServer
    {
        private TimerCom timerCom;

        public void Init()
        {
            timerCom = LCECS.ECSLocate.ECS.GetWorld().GetCom<TimerCom>();
        }

        public TimerInfo WaitForSeconds(float pSecond,Action pCallBack)
        {
            TimerInfo timerInfo = new TimerInfo(TimerType.Time,pSecond,pCallBack);
            timerCom.AddTimer(timerInfo);
            return timerInfo;
        }

        public TimerInfo WaitForEndOfFrame(Action pCallBack)
        {
            TimerInfo timerInfo = new TimerInfo(TimerType.Frame, 1, pCallBack);
            timerCom.AddTimer(timerInfo);
            return timerInfo;
        }

        public TimerInfo WaitForFrame(int pFrame, Action pCallBack)
        {
            TimerInfo timerInfo = new TimerInfo(TimerType.Frame, pFrame, pCallBack);
            timerCom.AddTimer(timerInfo);
            return timerInfo;
        }

        public void StopTimer(TimerInfo pInfo)
        {
            if (pInfo == null)
            {
                return;
            }
            timerCom.StopTimer(pInfo);
        }
    }
}