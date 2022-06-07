using LCMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCTask
{
    /// <summary>
    /// ����׶�
    /// </summary>
    public enum TaskStage
    {
        /// <summary>
        /// ����ǰ��ִ�н���Ŀ�����
        /// </summary>
        PreAccept,

        /// <summary>
        /// ���ܣ��жϽ���������ִ�н�����Ϊ
        /// </summary>
        Accept,

        /// <summary>
        /// �ύǰ��ִ���ύĿ�����
        /// </summary>
        PreExecute,

        /// <summary>
        /// �ύ���ж��ύ������ִ���ύ��Ϊ
        /// </summary>
        Execute,
    }

    /// <summary>
    /// ����Ŀ��
    /// </summary>
    public struct TaskTarget
    {
        public int mapId;
        public List<int> actorIds;
    }

    /// <summary>
    /// ������������
    /// </summary>
    public struct TaskModel
    {
        /// <summary>
        /// ����Id
        /// </summary>
        public int id;

        /// <summary>
        /// ǰ�ý�������
        /// </summary>
        public List<int> preUnlockTasks;

        /// <summary>
        /// ��������
        /// </summary>
        public List<int> unlockTasks;

        /// <summary>
        /// ����Ŀ��
        /// </summary>
        public TaskTarget acceptTarget;


    } 

    /// <summary>
    /// ����ʱ�������������
    /// </summary>
    public class TaskObj
    {
        /// <summary>
        /// ����Id
        /// </summary>
        public int TaskId { get; private set; }

        /// <summary>
        /// ����
        /// </summary>
        public TaskModel Model;

        /// <summary>
        /// ��ǰ����׶�Ŀ��
        /// </summary>
        public List<ActorObj> Targets { get; private set; }
    }
}
