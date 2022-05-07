using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// ����Timeline����
    /// </summary>
    public abstract class TimelineFunc
    {
        /// <summary>
        /// �ڵ�����ʱ��
        /// </summary>
        public float timeElapsed;

        public abstract void Execute(TimelineObj timelineObj);
    }

    /// <summary>
    /// ������ת
    /// </summary>
    public struct TimelineGoTo
    {
        ///<summary>
        ///������ʱ���
        ///</summary>
        public float atDuration;

        ///<summary>
        ///��ת��ʱ���
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
    /// ����Timeline����
    /// </summary>
    public struct TimelineModel
    {
        /// <summary>
        /// ���ܱ���Id
        /// </summary>
        public int id;

        /// <summary>
        /// ��ʱ��
        /// </summary>
        public float duration;

        /// <summary>
        /// Timelineִ�нڵ�
        /// </summary>
        public List<TimelineFunc> nodes;

        /// <summary>
        /// ��ת�ڵ�
        /// </summary>
        public TimelineGoTo goToNode;
    }

    /// <summary>
    /// ����ʱ�����ļ���Timeline����
    /// ���Ǳ�Unity��������Timeline��ֻ�Ǽ�¼ĳ��ʱ�����̶�������
    /// </summary>
    public class TimelineObj
    {
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public TimelineModel model;

        /// <summary>
        /// ����Timeline��ӵ����
        /// </summary>
        public SkillCom ower;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public float timeElapsed = 0;

        /// <summary>
        /// ����
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