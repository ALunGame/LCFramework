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
        
        /// <summary>
        /// 循环间隔多少秒执行
        /// </summary>
        /// <param name="pFrameCnt">间隔帧</param>
        /// <param name="pLoopCnt">循环次数</param>
        /// <param name="pCallBack">回调</param>
        /// <param name="pInstantCallBack">先立即执行一次</param>
        /// <returns></returns>
        public TimerInfo LoopSecond(float pIntervalSecond, int pLoopCnt, Action pCallBack, bool pInstantCallBack = false)
        {
            TimerInfo timerInfo = new TimerInfo(TimerType.Time,pIntervalSecond,pCallBack,pLoopCnt);
            timerCom.AddTimer(timerInfo);
            return timerInfo;
        }
        
        /// <summary>
        /// 循环间隔多少帧执行
        /// </summary>
        /// <param name="pFrameCnt">间隔帧</param>
        /// <param name="pLoopCnt">循环次数</param>
        /// <param name="pCallBack">回调</param>
        /// <param name="pInstantCallBack">先立即执行一次</param>
        /// <returns></returns>
        public TimerInfo LoopFrame(int pFrameCnt, int pLoopCnt, Action pCallBack, bool pInstantCallBack = false)
        {
            TimerInfo timerInfo = new TimerInfo(TimerType.Frame,pFrameCnt,pCallBack,pLoopCnt);
            timerCom.AddTimer(timerInfo);
            return timerInfo;
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