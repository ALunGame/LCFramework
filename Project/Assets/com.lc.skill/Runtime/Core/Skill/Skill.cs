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
        public SkillCondition nextCondition;

        public SkillCondition()
        {

        }

        public SkillCondition(bool checkValue, ConditionRelateType nextRelate)
        {
            this.checkValue = checkValue;
            this._type = nextRelate;
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
        public string id;

        /// <summary>
        /// ������
        /// </summary>
        public string name;

        /// <summary>
        /// ��������
        /// </summary>
        public SkillCondition condition;

        /// <summary>
        /// ��������
        /// </summary>
        public List<SkillCost> costs;

        /// <summary>
        /// ����Ч��
        /// </summary>
        public string timeline;

        /// <summary>
        /// ѧ�Ἴ��ʱ����õ�Buff
        /// </summary>
        public List<AddBuffModel> addBuffs;
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
