using LCMap;
using LCNode;
using LCNode.Model;
using System.Collections.Generic;

namespace LCTask.TaskGraph
{
    #region ������������

    public class Task_ConditionFuncData { }
    /// <summary>
    /// ������������
    /// </summary>
    public abstract class Task_ConditionFuncNode : BaseNode
    {
        public override string Title { get => "������������"; set => base.Title = value; }
        public override string Tooltip { get => "������������"; set => base.Tooltip = value; }

        [OutputPort("���ڵ�", BasePort.Capacity.Single)]
        public Task_ConditionFuncData parentNode;

        [NodeValue("����ֵ")]
        public bool checkValue = true;

        [NodeValue("����һ��������ϵ")]
        public ConditionType conditionType = ConditionType.AND;

        public TaskConditionFunc GetFunc()
        {
            TaskConditionFunc func  = CreateFunc();
            func.checkValue         = checkValue;
            func.conditionType      = conditionType;
            return func;
        }

        public abstract TaskConditionFunc CreateFunc();
    }

    #endregion

    #region ����Ŀ����ֺ���

    public class Task_DisplayFuncData { }
    /// <summary>
    /// ����Ŀ����ֺ���
    /// </summary>
    public abstract class Task_TargetDisplayFuncNode : BaseNode
    {
        public override string Title { get => "������ֺ���"; set => base.Title = value; }
        public override string Tooltip { get => "������ֺ���"; set => base.Tooltip = value; }

        [OutputPort("���ڵ�", BasePort.Capacity.Single)]
        public Task_DisplayFuncData parentNode;

        public TaskTargetDisplayFunc GetFunc()
        {
            TaskTargetDisplayFunc func = CreateFunc();
            return func;
        }

        public abstract TaskTargetDisplayFunc CreateFunc();
    }

    #endregion

    #region �����������

    public class Task_ListenFuncData { }
    /// <summary>
    /// �����������
    /// </summary>
    public abstract class Task_ListenFuncNode : Map_ActorNode
    {
        [OutputPort("���ڵ�", BasePort.Capacity.Single)]
        public Task_ListenFuncData parentNode;

        public TaskListenFunc GetFunc()
        {
            TaskListenFunc func = CreateFunc();
            return func;
        }

        public abstract TaskListenFunc CreateFunc();
    }

    #endregion

    #region ������Ϊ����

    public class Task_ActionFuncData { }
    /// <summary>
    /// ����ͨ����Ϊ
    /// </summary>
    public abstract class Task_ActionFuncNode : BaseNode
    {
        public TaskActionFunc GetFunc()
        {
            TaskActionFunc func = CreateFunc();
            return func;
        }

        public abstract TaskActionFunc CreateFunc();
    }

    /// <summary>
    /// ����ͨ����Ϊ
    /// </summary>
    public abstract class Task_CommonActionFuncNode : Task_ActionFuncNode
    {
        [InputPort("���ڵ�", BasePort.Capacity.Single)]
        public Task_ActionFuncData parentNode;
    }

    public class Task_SuccessActionFuncData : Task_ActionFuncData { }

    /// <summary>
    /// ����׶���Ϊִ�гɹ���Ϊ
    /// 1����Ҫ���ڻ�õ�������
    /// </summary>
    public abstract class Task_SuccessActionFuncNode : Task_ActionFuncNode
    {
        [InputPort("���ڵ�", BasePort.Capacity.Single)]
        public Task_SuccessActionFuncData parentNode;
    }

    public class Task_AcceptActionFuncData : Task_ActionFuncData { }
    /// <summary>
    /// ���������Ϊ����
    /// 1������ֻ���ڽ��ܽ׶�ִ�е���Ϊ
    /// </summary>
    public abstract class Task_AcceptActionFuncNode : Task_ActionFuncNode
    {
        [InputPort("���ڵ�", BasePort.Capacity.Single)]
        public Task_AcceptActionFuncData parentNode;
    }

    public class Task_ExecuteActionFuncData : Task_ActionFuncData { }
    /// <summary>
    /// �����ύ��Ϊ����
    /// 1������ֻ�����ύ�׶�ִ�е���Ϊ
    /// </summary>
    public abstract class Task_ExecuteActionFuncNode : Task_ActionFuncNode
    {
        [InputPort("���ڵ�", BasePort.Capacity.Single)]
        public Task_ExecuteActionFuncData parentNode;
    }

    #endregion

    [NodeMenuItem("����Ŀ��")]
    public class Task_TargetNode : Map_ActorNode
    {
        [OutputPort("���ڵ�", BasePort.Capacity.Single)]
        public MapActorData parentNode;
    }

    public abstract class Task_Node : BaseNode
    {
        public int taskId;

        [InputPort("�׶�����", BasePort.Capacity.Multi)]
        public Task_ConditionFuncData conditionFuncs;

        [InputPort("�׶�Ŀ��", BasePort.Capacity.Single)]
        public MapActorData target;

        [InputPort("�׶�Ŀ�����", BasePort.Capacity.Multi)]
        public Task_DisplayFuncData targetDisplayFuncs;

        public int GetTargetMapId()
        {
            List<Task_TargetNode> targetNodes = NodeHelper.GetNodeOutNodes<Task_TargetNode>(Owner, this, "�׶�Ŀ��");
            if (targetNodes.Count > 0)
            {
                return (int)targetNodes[0].mapId;
            }
            return 0;
        }

        public List<int> GetTargetActorIds()
        {
            List<Task_TargetNode> targetNodes = NodeHelper.GetNodeOutNodes<Task_TargetNode>(Owner, this, "�׶�Ŀ��");
            if (targetNodes.Count > 0)
            {
                return targetNodes[0].GetActorIds();
            }
            return null;
        }

        public List<TaskConditionFunc> GetConditionFuncs()
        {
            List<TaskConditionFunc> funcs = new List<TaskConditionFunc>();
            List<Task_ConditionFuncNode> conditionNodes = NodeHelper.GetNodeOutNodes<Task_ConditionFuncNode>(Owner, this, "�׶�����");
            if (conditionNodes.Count > 0)
            {
                for (int i = 0; i < conditionNodes.Count; i++)
                {
                    funcs.Add(conditionNodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public List<TaskTargetDisplayFunc> GetDisplayFuncs()
        {
            List<TaskTargetDisplayFunc> funcs = new List<TaskTargetDisplayFunc>();
            List<Task_TargetDisplayFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_TargetDisplayFuncNode>(Owner, this, "�׶�Ŀ�����");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public TaskContent GetContent()
        {
            TaskContent content = CreateContent();
            content.mapId = GetTargetMapId();
            content.actorIds = GetTargetActorIds();
            content.displayFuncs = GetDisplayFuncs();
            content.conditionFuncs = GetConditionFuncs();
            return content;
        }

        public abstract TaskContent CreateContent();
    }

    [NodeMenuItem("�������")]
    public class Task_AcceptNode : Task_Node
    {
        public override string Title { get => $"����{taskId}����"; set => base.Title = value; }

        [OutputPort("������Ϊ", BasePort.Capacity.Multi)]
        public Task_AcceptActionFuncData actionFuncs;

        [OutputPort("���ܼ���", BasePort.Capacity.Multi)]
        public Task_ListenFuncData actionListenFuncs;

        [OutputPort("���ܳɹ�", BasePort.Capacity.Multi)]
        public Task_SuccessActionFuncData actionSuccess;

        [OutputPort("����ʧ��", BasePort.Capacity.Multi)]
        public Task_ActionFuncData actionFail;

        #region ����

        private List<TaskListenFunc> GetActionListenFuncs()
        {
            List<TaskListenFunc> funcs = new List<TaskListenFunc>();
            List<Task_ListenFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ListenFuncNode>(Owner, this, "���ܼ���");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        #endregion

        #region ��Ϊ

        public List<TaskActionFunc> GetActionFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "������Ϊ");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public List<TaskActionFunc> GetActionSuccessFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "���ܳɹ�");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public List<TaskActionFunc> GetActionFailFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "����ʧ��");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        #endregion

        public override TaskContent CreateContent()
        {
            TaskContent content = new TaskContent();
            content.actionFuncs = GetActionFuncs();
            content.actionListenFuncs = GetActionListenFuncs();
            content.actionSuccess = GetActionSuccessFuncs();
            content.actionFail = GetActionFailFuncs();
            return content;
        }
    }

    [NodeMenuItem("�����ύ")]
    public class Task_ExecuteNode : Task_Node
    {
        public override string Title { get => $"�ύ{taskId}����"; set => base.Title = value; }

        [OutputPort("�ύ��Ϊ", BasePort.Capacity.Multi)]
        public Task_ExecuteActionFuncData actionFuncs;

        [OutputPort("�ύ����", BasePort.Capacity.Multi)]
        public Task_ListenFuncData actionListenFuncs;

        [OutputPort("�ύ�ɹ�", BasePort.Capacity.Multi)]
        public Task_SuccessActionFuncData actionSuccess;

        [OutputPort("�ύʧ��", BasePort.Capacity.Multi)]
        public Task_ActionFuncData actionFail;


        #region ����

        private List<TaskListenFunc> GetActionListenFuncs()
        {
            List<TaskListenFunc> funcs = new List<TaskListenFunc>();
            List<Task_ListenFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ListenFuncNode>(Owner, this, "���ܼ���");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        #endregion

        #region ��Ϊ

        public List<TaskActionFunc> GetActionFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "�ύ��Ϊ");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public List<TaskActionFunc> GetActionSuccessFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "�ύ�ɹ�");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        public List<TaskActionFunc> GetActionFailFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            List<Task_ActionFuncNode> nodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "�ύʧ��");
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    funcs.Add(nodes[i].GetFunc());
                }
            }
            return funcs;
        }

        #endregion

        public override TaskContent CreateContent()
        {
            TaskContent content = new TaskContent();
            content.actionFuncs = GetActionFuncs();
            content.actionListenFuncs = GetActionListenFuncs();
            content.actionSuccess = GetActionSuccessFuncs();
            content.actionFail = GetActionFailFuncs();
            return content;
        }
    }
}
