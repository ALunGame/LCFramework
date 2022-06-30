using LCECS.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Demo.Com
{
    public enum TimerType
    {
        /// <summary>
        /// 时间
        /// </summary>
        Time,

        /// <summary>
        /// 帧数
        /// </summary>
        Frame,
    }

    public class TimerInfo
    {
        public float timer;
        public TimerType type;
        public float waitTime;

        private int loopCnt = 1;
        private Action callBack;
        private int executeCnt;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="waitTime"></param>
        /// <param name="callBack"></param>
        /// <param name="loopCnt">循环次数，-1无限</param>
        public TimerInfo(TimerType type,float waitTime,Action callBack,int loopCnt = 1)
        {
            this.type = type;
            this.waitTime = waitTime;
            this.callBack = callBack;
            RefreshTimer();
        }

        private void RefreshTimer()
        {
            switch (type)
            {
                case TimerType.Time:
                    timer = Time.realtimeSinceStartup;
                    this.waitTime = timer + waitTime;
                    break;
                case TimerType.Frame:
                    timer = 0;
                    break;
                default:
                    break;
            }
        }

        public void ExecuteCallBack()
        {
            if (executeCnt >= loopCnt)
                return;
            executeCnt++;
            RefreshTimer();
            callBack?.Invoke();
        }

        public bool CheckIsFinish()
        {
            return executeCnt >= loopCnt;
        }
    }


    public class TimerCom : BaseCom
    {
        private List<TimerInfo> timerInfos = new List<TimerInfo>();

        public IReadOnlyList<TimerInfo> TimerInfos { get => timerInfos;}

        public void StopTimer(TimerInfo timerInfo)
        {
            timerInfos.Remove(timerInfo);
        }

        public void AddTimer(TimerInfo timerInfo)
        {
            if (timerInfos.Contains(timerInfo))
                timerInfos.Remove(timerInfo);
            timerInfos.Add(timerInfo);
        }
    } 
}
