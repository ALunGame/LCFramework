using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 技能Timeline函数
    /// </summary>
    public abstract class TimelineFunc
    {
        /// <summary>
        /// 节点运行时间
        /// </summary>
        public float timeElapsed;

        public abstract void Execute(TimelineObj timelineObj);
    }

    /// <summary>
    /// 技能跳转
    /// </summary>
    public struct TimelineGoTo
    {
        ///<summary>
        ///自身处于时间点
        ///</summary>
        public float atDuration;

        ///<summary>
        ///跳转到时间点
        ///</summary>
        public float gotoDuration;

        public TimelineGoTo(float atDuration, float gotoDuration)
        {
            this.atDuration = atDuration;
            this.gotoDuration = gotoDuration;
        }

        public static TimelineGoTo Null = new TimelineGoTo(float.MaxValue, float.MaxValue);
    }

    /// <summary>
    /// 技能Timeline配置
    /// </summary>
    public struct TimelineModel
    {
        /// <summary>
        /// 技能表现Id
        /// </summary>
        public int id;

        /// <summary>
        /// 总时长
        /// </summary>
        public float duration;

        /// <summary>
        /// Timeline执行节点
        /// </summary>
        public List<TimelineFunc> nodes;

        /// <summary>
        /// 跳转节点
        /// </summary>
        public TimelineGoTo goToNode;
    }

    /// <summary>
    /// 运行时创建的技能Timeline对象
    /// 他是比Unity更轻量的Timeline，只是记录某个时刻做固定的事情
    /// </summary>
    public class TimelineObj
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public TimelineModel model;

        /// <summary>
        /// 播放Timeline的拥有者
        /// </summary>
        public SkillCom ower;

        /// <summary>
        /// 运行时长
        /// </summary>
        public float timeElapsed = 0;

        /// <summary>
        /// 倍速
        /// </summary>
        public float timeScale
        {
            get
            {
                return _timeScale;
            }
            set
            {
                _timeScale = Mathf.Max(0.100f, value);
            }
        }
        private float _timeScale = 1.00f;

        public TimelineObj(TimelineModel model, SkillCom ower)
        {
            this.model = model;
            this.ower = ower;
            this._timeScale = 1.00f;
        }
    }

}