using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 技能跳转
    /// </summary>
    public class TimelineGoTo
    {
        ///<summary>
        ///自身处于时间点
        ///</summary>
        public float atDuration;

        ///<summary>
        ///跳转到时间点
        ///</summary>
        public float gotoDuration;
    }

    /// <summary>
    /// 技能Timeline配置
    /// </summary>
    public struct TimelineModel
    {
        /// <summary>
        /// 技能表现名
        /// </summary>
        public string name;

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
        /// 通过那个技能创建的
        /// </summary>
        public string skillId;

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

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool isFinish = false;

        public TimelineObj(string skillId, TimelineModel model, SkillCom ower)
        {
            this.skillId = skillId;
            this.model = model;
            this.ower = ower;
            this._timeScale = 1.00f;
        }
    }

}