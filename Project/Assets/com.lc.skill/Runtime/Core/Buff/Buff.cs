using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// �������һ��Buff
    /// </summary>
    public class AddBuffModel
    {
        /// <summary>
        /// BuffId
        /// </summary>
        [Header("BuffId")]
        public string id = "";

        /// <summary>
        /// ��ӵĲ������������Ǽ���
        /// </summary>
        [Header("��ӵĲ���")]
        public int addStack;

        /// <summary>
        /// ����ʱ������ģʽ(true:���� false:�ۼ�)
        /// </summary>
        [Header("����ʱ������ģʽtrue:���� false:�ۼ�")]
        public bool durationSetType;

        /// <summary>
        /// ��ӵĳ���ʱ��
        /// </summary>
        [Header("����ʱ��")]
        public float duration;

        /// <summary>
        /// �Ƿ���һ�����õ�buff,�������ʹ�������ٵ�0��Ҳ�ᱻɾ��
        /// </summary>
        [Header("����buff")]
        public bool isPermanent;
    }

    /// <summary>
    /// ���õ�Buff���ݽṹ
    /// </summary>
    public struct BuffModel 
    {
        /// <summary>
        /// BuffId
        /// </summary>
        public string id;

        /// <summary>
        /// Buff����
        /// </summary>
        public string name;

        /// <summary>
        /// Buff�ı�ǩ
        /// </summary>
        public string[] tags;

        /// <summary>
        /// Buff���ȼ������ȼ�Խ�͵�buffԽ����ִ�У�
        /// </summary>
        public int priority;

        /// <summary>
        /// ���ѵ�����
        /// </summary>
        public int maxStack;

        /// <summary>
        /// Buff��ִ��OnTick���
        /// </summary>
        public float tickTime;

        /// <summary>
        /// ���ͷ�һ������ʱִ�еĺ���
        /// Ϊ�˴��������ͷ�ʱ�����ļ��ܱ��֣�����û��ħ����Ҫ�ͷţ���ִ��û��ħ���Ķ���
        /// </summary>
        public BuffOnFreedFunc onFreedFunc;


        #region �������ں���

        /// <summary>
        /// ��Buff����ӡ��ı����ʱִ�еĺ���
        /// </summary>
        public List<BuffLifeCycleFunc> onOccurFunc;

        /// <summary>
        /// Buff��tickTime���ִ�еĺ���
        /// </summary>
        public List<BuffLifeCycleFunc> onTickFunc;

        /// <summary>
        /// Buff��Ҫ���Ƴ�ʱִ�еĺ���
        /// </summary>
        public List<BuffLifeCycleFunc> onRemovedFunc;

        #endregion

        #region �˺�����

        /// <summary>
        /// ��ִ���˺�����ʱ��ӵ�����Buff��Ϊ������ִ�еĺ���
        /// </summary>
        public List<BuffHurtFunc> onHurtFunc;

        /// <summary>
        /// ��ִ���˺�����ʱ��ӵ�����Buff��Ϊ������ִ�еĺ���
        /// </summary>
        public List<BuffBeHurtFunc> onBeHurtFunc;

        /// <summary>
        /// ��ִ���˺�����ʱ�������ɱĿ��ִ�еĺ���
        /// </summary>
        public List<BuffKilledFunc> onKilledFunc;

        /// <summary>
        /// ��ִ���˺�����ʱ��ӵ�����Buff��ɱ��ִ�еĺ���
        /// </summary>
        public List<BuffBeKilledFunc> onBeKilledFunc; 

        #endregion
    }

    /// <summary>
    /// �������һ��Buff����Ϣ
    /// </summary>
    public struct AddBuffInfo
    {
        /// <summary>
        /// ��ӵķ�����
        /// </summary>
        public SkillCom ower;

        /// <summary>
        /// ��ӵ�Ŀ��
        /// </summary>
        public SkillCom target;

        /// <summary>
        /// ��ӵ�BuffId
        /// </summary>
        public BuffModel buffModel;

        /// <summary>
        /// ��ӵĲ������������Ǽ���
        /// </summary>
        public int addStack;

        /// <summary>
        /// ����ʱ������ģʽ(true:���� false:�ۼ�)
        /// </summary>
        public bool durationSetType;

        /// <summary>
        /// ��ӵĳ���ʱ��
        /// </summary>
        public float duration;

        /// <summary>
        /// �Ƿ���һ�����õ�buff,�������ʹ�������ٵ�0��Ҳ�ᱻɾ��
        /// </summary>
        public bool isPermanent;
    }

    /// <summary>
    /// �����й������ϵ�Buff
    /// </summary>
    public class BuffObj
    {
        /// <summary>
        /// ����
        /// </summary>
        public BuffModel model;

        /// <summary>
        /// ʣ��ʱ��
        /// </summary>
        public float duration;

        /// <summary>
        /// �Ƿ���һ������Buff
        /// </summary>
        public bool isPermanent;

        /// <summary>
        /// ��ǰ����
        /// </summary>
        public int stack;

        /// <summary>
        /// ���Buff��ͨ��˭��ӵģ������ǿգ�
        /// </summary>
        public SkillCom originer;

        /// <summary>
        /// ���Buff��ӵ����
        /// </summary>
        public SkillCom ower;

        /// <summary>
        /// ���Buff����ʱ��
        /// </summary>
        public float timeElapsed = 0;

        /// <summary>
        /// ���Buffִ�ж��ٴ�onTick
        /// </summary>
        public int tickCnt = 0;

        ///<summary>
        ///buff��һЩ��������Щ�������߼�ʹ�õģ�����wow����ʦ�Ķܻ������ն����˺����Ϳ��Լ�¼��buffParam����
        ///</summary>
        public Dictionary<string, object> buffParam = new Dictionary<string, object>();

        /// <summary>
        /// ����Buff����
        /// </summary>
        /// <param name="originer">Buff������(����Ϊ��)</param>
        /// <param name="model">Buff��������</param>
        /// <param name="ower">BuffЯ����</param>
        /// <param name="duration">����ʱ��</param>
        /// <param name="stack">Buff����</param>
        /// <param name="permanent">�ǲ�������Buff</param>
        /// <param name="buffParam">Buff����</param>
        public BuffObj(SkillCom originer,BuffModel model, SkillCom ower, float duration, int stack, bool permanent = false)
        {
            this.originer = originer;
            this.model = model;
            this.ower = ower;
            this.duration = duration;
            this.stack = stack; 
            this.isPermanent = permanent;
        }
    }
}