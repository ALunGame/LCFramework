using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Demo.System
{
    /// <summary>
    /// 昼夜系统
    /// </summary>
    public class DayNightSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(DayNightCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            DayNightCom dayNightCom = GetCom<DayNightCom>(comList[0]);
            HandleDayNightCom(dayNightCom);
        }

        private void HandleDayNightCom(DayNightCom dayNightCom)
        {
            float leftTime = dayNightCom.CurrStageLeftTime - Time.deltaTime;
            if (leftTime <= 0)
            {
                DayNightStage nextStage = GetNextStage(dayNightCom);
                dayNightCom.SetStage(nextStage);
            }
            else
            {
                dayNightCom.SetStageLeftTime(leftTime);
            }
        }

        private DayNightStage GetNextStage(DayNightCom dayNightCom)
        {
            DayNightStage currStage = dayNightCom.GetStage();
            if (currStage == DayNightStage.Night)
            {
                return DayNightStage.Morning;
            }
            return currStage + 1;
        }
    }
}