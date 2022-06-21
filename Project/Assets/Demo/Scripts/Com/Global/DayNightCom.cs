using LCECS.Core;
using LCToolkit;
using System;
using UnityEngine;

namespace Demo.Com
{
    /// <summary>
    /// 昼夜阶段
    /// </summary>
    public enum DayNightStage
    {
        /// <summary>
        /// 早晨
        /// </summary>
        Morning,   
        /// <summary>
        /// 白天
        /// </summary>
        Day,
        /// <summary>
        /// 黄昏
        /// </summary>
        NightFull,
        /// <summary>
        /// 夜晚
        /// </summary>
        Night,
    }

    /// <summary>
    /// 昼夜组件
    /// </summary>
    public class DayNightCom : BaseCom
    {
        /// <summary>
        /// 早晨总时长
        /// </summary>
        public int MorningTotalSecond   = 60;
        /// <summary>
        /// 白天总时长
        /// </summary>
        public int DayTotalSecond       = 1200;
        /// <summary>
        /// 黄昏总时长
        /// </summary>
        public int NightFullTotalSecond = 60;
        /// <summary>
        /// 夜晚总时长
        /// </summary>
        public int NightTotalSecond     = 540;

        [NonSerialized]
        private BindableValue<DayNightStage> currStage = new BindableValue<DayNightStage>();

        /// <summary>
        /// 当前阶段剩余时间
        /// </summary>
        public float CurrStageLeftTime { get; private set; }

        protected override void OnInit(GameObject go)
        {
            base.OnInit(go);
            currStage.SetValueWithoutNotify(DayNightStage.Morning);
            CurrStageLeftTime = GetStageTotalSecond(DayNightStage.Morning);
        }

        #region Set

        /// <summary>
        /// 设置昼夜阶段剩余时间
        /// </summary>
        /// <param name="leftTime"></param>
        public void SetStageLeftTime(float leftTime)
        {
            CurrStageLeftTime = leftTime;
        }

        /// <summary>
        /// 设置昼夜阶段
        /// </summary>
        public void SetStage(DayNightStage stage)
        {
            if (currStage.Value == stage)
                return;
            currStage.Value   = stage;
            CurrStageLeftTime = GetStageTotalSecond(stage);
        }

        #endregion

        #region Get

        /// <summary>
        /// 设置昼夜阶段
        /// </summary>
        public DayNightStage GetStage()
        {
            return currStage.Value;
        } 

        /// <summary>
        /// 获得昼夜阶段总时长
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        public int GetStageTotalSecond(DayNightStage stage)
        {
            switch (stage)
            {
                case DayNightStage.Morning:
                    return MorningTotalSecond;
                case DayNightStage.Day:
                    return DayTotalSecond;
                case DayNightStage.NightFull:
                    return NightFullTotalSecond;
                case DayNightStage.Night:
                    return NightTotalSecond;
                default:
                    break;
            }
            return 0;
        }

        #endregion

        /// <summary>
        /// 注册昼夜变换监听
        /// </summary>
        /// <param name="callBack"></param>
        public void RegStageChange(Action<DayNightStage> callBack)
        {
            currStage.RegisterValueChangedEvent(callBack);
        }

        /// <summary>
        /// 清理昼夜变换监听
        /// </summary>
        /// <param name="callBack"></param>
        public void ClearReqAnimChangeCallBack()
        {
            currStage.ClearChangedEvent();
        }
    }
}