using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.System
{
    public class TimerSystem : BaseSystem
    {
        protected override List<Type> RegContainListenComs()
        {
            return new List<Type>() { typeof(TimerCom)};
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            TimerCom timerCom = GetCom<TimerCom>(comList[0]);
            HandleTimer(timerCom);
        }

        private void HandleTimer(TimerCom timerCom)
        {
            for (int i = 0; i < timerCom.TimerInfos.Count; i++)
            {
                TimerInfo timerInfo = timerCom.TimerInfos[i];
                switch (timerInfo.type)
                {
                    case TimerType.Time:
                        timerInfo.timer += Time.deltaTime;
                        break;
                    case TimerType.Frame:
                        timerInfo.timer++;
                        break;
                    default:
                        break;
                }
                if (timerInfo.timer >= timerInfo.waitTime)
                    timerInfo.ExecuteCallBack();
                if (timerInfo.CheckIsFinish())
                    timerCom.StopTimer(timerInfo);
            }
        }
    }
}