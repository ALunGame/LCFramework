using Demo.Com;
using System.Collections;
using UnityEngine;

namespace Demo.Server
{
    public class TimerServer
    {
        private TimerCom timerCom;

        public void Init()
        {
            timerCom = LCECS.ECSLocate.ECS.GetWorld().GetCom<TimerCom>();
        }


    }
}