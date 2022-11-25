using DG.Tweening;
using LCECS.Core;
using LCToolkit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

    public class DayNightStageInfo
    {
        public DayNightStage stage;
        public float intensity;
        public Color color;
    }

    /// <summary>
    /// 昼夜组件
    /// </summary>
    public class DayNightCom : BaseCom
    {
        /// <summary>
        /// 早晨总时长
        /// </summary>
        [NonSerialized]
        public int MorningTotalSecond   = 60;
        /// <summary>
        /// 白天总时长
        /// </summary>
        [NonSerialized]
        public int DayTotalSecond       = 1200;
        /// <summary>
        /// 黄昏总时长
        /// </summary>
        [NonSerialized]
        public int NightFullTotalSecond = 60;
        /// <summary>
        /// 夜晚总时长
        /// </summary>
        [NonSerialized]
        public int NightTotalSecond     = 540;

        [NonSerialized]
        private BindableValue<DayNightStage> currStage = new BindableValue<DayNightStage>();

        [NonSerialized]
        public Light2D GlobalLight;
        [NonSerialized]
        public Tween Tw_GlobalLightIntensity;
        [NonSerialized]
        public Tween Tw_GlobalLightColor;
        [NonSerialized]
        public float GlobalLightFadeTime = 30;  //全局灯光渐变时间


        public List<DayNightStageInfo> stageInfos = new List<DayNightStageInfo>();

        /// <summary>
        /// 当前阶段剩余时间
        /// </summary>
        public float CurrStageLeftTime { get; private set; }

        protected override void OnInit(Entity pEntity)
        {
            GlobalLight = GameLocate.Center.GetComponentInChildren<Light2D>();
            currStage.SetValueWithoutNotify(DayNightStage.NightFull);
            CurrStageLeftTime = GetStageTotalSecond(DayNightStage.NightFull);
            SetLight(GetStage());
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
            GameLocate.Log.LogWarning($"昼夜变化{currStage.Value}>>>{stage}");
            currStage.Value   = stage;
            CurrStageLeftTime = GetStageTotalSecond(stage);
            PlayStageLight(stage);
        }

        private void PlayStageLight(DayNightStage stage)
        {
            DayNightStageInfo stageInfo = GetStageInfo(stage);
            Tw_GlobalLightColor.Complete(false);
            Tw_GlobalLightIntensity.Complete(false);

            Tw_GlobalLightColor = DOTween.To(() => GlobalLight.color, x => GlobalLight.color = x, stageInfo.color, GlobalLightFadeTime);
            Tw_GlobalLightIntensity = DOTween.To(() => GlobalLight.intensity, x => GlobalLight.intensity = x, stageInfo.intensity, GlobalLightFadeTime);
        }

        private void SetLight(DayNightStage stage)
        {
            DayNightStageInfo stageInfo = GetStageInfo(stage);
            GlobalLight.color = stageInfo.color;
            GlobalLight.intensity = stageInfo.intensity;
        }

        #endregion

        #region Get

        /// <summary>
        /// 获得当前昼夜阶段
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

        /// <summary>
        /// 获得昼夜阶段信息
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        public DayNightStageInfo GetStageInfo(DayNightStage stage)
        {
            for (int i = 0; i < stageInfos.Count; i++)
            {
                if (stageInfos[i].stage == stage)
                {
                    return stageInfos[i];
                }
            }
            GameLocate.Log.LogError("获得昼夜阶段信息出错，没有该阶段信息", stage);
            return stageInfos[0];
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