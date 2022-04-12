using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// ����Timeline�ڵ�
    /// </summary>
    public struct TimelineNode
    {
        /// <summary>
        /// �ڵ�����ʱ��
        /// </summary>
        public float timeElapsed;
        public string funcName;
        public object[] funcParam;
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
    /// ����Timeline
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
        public TimelineNode[] nodes;

        /// <summary>
        /// ��ת�ڵ�
        /// </summary>
        public TimelineGoTo goToNode;

        public TimelineModel(int id, TimelineNode[] nodes, float duration)
        {
            this.id = id;
            this.nodes = nodes;
            this.duration = duration;
            this.goToNode = TimelineGoTo.Null;
        }

        public TimelineModel(int id, TimelineNode[] nodes, float duration, TimelineGoTo goToNode)
        {
            this.id = id;
            this.nodes = nodes;
            this.duration = duration;
            this.goToNode = goToNode;   
        }
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