using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// ������ϵ
    /// </summary>
    public enum ConditionRelateType
    {
        AND,
        OR,
    }

    /// <summary>
    /// ��������
    /// </summary>
    public abstract class SkillCondition
    {
        private ConditionRelateType _type;
        private bool checkValue = true;
        private SkillCondition nextCondition;

        public SkillCondition(bool checkValue, ConditionRelateType nextRelate, SkillCondition nextCondition = null)
        {
            this.checkValue = checkValue;
            this._type = nextRelate;
            this.nextCondition = nextCondition;
        }

        /// <summary>
        /// �����Ƿ�����
        /// </summary>
        /// <returns></returns>
        public bool IsTrue()
        {
            bool resValue = OnMakeTrue();
            resValue = checkValue == resValue ? true : false;
            if (nextCondition != null)
            {
                switch (_type)
                {
                    case ConditionRelateType.AND:
                        return resValue && nextCondition.IsTrue();
                    case ConditionRelateType.OR:
                        return resValue || nextCondition.IsTrue();
                }
                return resValue && nextCondition.IsTrue();
            }
            else
            {
                return resValue;
            }
        }

        protected abstract bool OnMakeTrue();
    }

    /// <summary>
    /// ��������
    /// </summary>
    public class SkillCost
    {
        /// <summary>
        /// ��������
        /// </summary>
        public int costType;
        
        /// <summary>
        /// ��������
        /// </summary>
        public int costValue;
    }

    /// <summary>
    /// ���õ�Skill���ݽṹ
    /// </summary>
    public struct SkillModel
    {
        /// <summary>
        /// ����Id
        /// </summary>
        public int id;

        /// <summary>
        /// ������
        /// </summary>
        public int name;

        /// <summary>
        /// ��������
        /// </summary>
        public SkillCondition condition;

        /// <summary>
        /// ��������
        /// </summary>
        public SkillCost cost;

        /// <summary>
        /// ����Ч��
        /// </summary>
        public TimelineModel timeline;

        /// <summary>
        /// ѧ�Ἴ��ʱ����õ�Buff
        /// </summary>
        public AddBuffInfo[] addBuffs;
    } 

    /// <summary>
    /// ����ʱ�����ļ��ܶ���
    /// </summary>
    public class SkillObj
    {
        public SkillModel model;

        /// <summary>
        /// ���ܵȼ�
        /// </summary>
        public int level;

        /// <summary>
        /// ��ȴʱ��
        /// </summary>
        public float coldDown;

        public SkillObj(SkillModel model,int level)
        {
            this.model = model;
            this.level = level;
            this.coldDown = 0;
        }
    }
}
