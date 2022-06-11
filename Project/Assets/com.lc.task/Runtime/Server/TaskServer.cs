using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCTask
{
    public class TaskServer : ITaskServer
    {
        private TaskCom taskCom;

        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="pTaskId"></param>
        /// <returns></returns>
        public bool AddTask(int pTaskId, TaskStage taskStage = TaskStage.Accept)
        {
            TaskObj taskObj = taskCom.GetTask(pTaskId);
            if (taskObj != null)
            {
                TaskLocate.Log.LogWarning("�������ʧ��,�ظ�����Id",pTaskId);
                return false;
            }
            if (TaskLocate.Config.GetTaskModel(pTaskId,out var model))
            {
                taskObj = new TaskObj(pTaskId, model);
                taskObj.ExecuteTargetsDisplay(taskStage);
                taskCom.AddTask(taskObj);
                return true;
            }
            else
            {
                TaskLocate.Log.LogWarning("�������ʧ��,û����������", pTaskId);
                return false;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public bool AcceptTask(int pTaskId)
        {
            TaskObj taskObj = taskCom.GetTask(pTaskId);
            if (taskObj == null)
            {
                TaskLocate.Log.LogError("��������ʧ��,û�д�����", pTaskId);
                return false;
            }
            return taskObj.Accept();
        }

        /// <summary>
        /// �ύ����
        /// </summary>
        /// <returns></returns>
        public bool ExecuteTask(int pTaskId)
        {
            TaskObj taskObj = taskCom.GetTask(pTaskId);
            if (taskObj == null)
            {
                TaskLocate.Log.LogError("��������ʧ��,û�д�����", pTaskId);
                return false;
            }
            return taskObj.Execute();
        }

        /// <summary>
        /// �������ʱ
        /// </summary>
        /// <param name="pTaskId"></param>
        public void OnTaskFinish(int pTaskId)
        {
            taskCom.RemoveTask(pTaskId);
        }
    }
}