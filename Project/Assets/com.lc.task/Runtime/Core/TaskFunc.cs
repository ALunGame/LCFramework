using LCMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCTask
{
    /// <summary>
    /// ����Ŀ����ֺ���
    /// </summary>
    public abstract class TaskTargetDisplayFunc
    {
        public abstract void Execute(TaskObj taskObj, List<ActorObj> targets);

        /// <summary>
        /// ������,ÿ֡���ע������
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckFinish();
    }

    /// <summary>
    /// ������������
    /// </summary>
    public abstract class TaskConditionFunc
    {
        public bool checkValue = true;
        public ConditionType conditionType;

        public abstract bool CheckTure(TaskObj taskObj);
    }

    /// <summary>
    /// ������Ϊ����
    /// </summary>
    public abstract class TaskActionFunc
    {
        public abstract void Execute(TaskObj taskObj);

        /// <summary>
        /// ������,ÿ֡���ע������
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckFinish();
    }
}
