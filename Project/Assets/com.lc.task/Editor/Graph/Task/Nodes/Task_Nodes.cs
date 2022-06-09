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

        [InputPort("��һ������", BasePort.Capacity.Single)]
        public Task_ConditionFuncData nextNode;

        public List<TaskConditionFunc> GetFuncs()
        {
            List<TaskConditionFunc> funcs = new List<TaskConditionFunc>();
            funcs.Add(CreateFunc());
            List<Task_ConditionFuncNode> nextNodes = NodeHelper.GetNodeOutNodes<Task_ConditionFuncNode>(Owner, this, "��һ������");
            if (nextNodes.Count > 0)
            {
                funcs.AddRange(nextNodes[0].GetFuncs());
            }
            return funcs;
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

        [InputPort("��һ������", BasePort.Capacity.Single)]
        public Task_DisplayFuncData nextNode;

        public List<TaskTargetDisplayFunc> GetFuncs()
        {
            List<TaskTargetDisplayFunc> funcs = new List<TaskTargetDisplayFunc>();
            funcs.Add(CreateFunc());
            List<Task_TargetDisplayFuncNode> nextNodes = NodeHelper.GetNodeOutNodes<Task_TargetDisplayFuncNode>(Owner, this, "��һ������");
            if (nextNodes.Count > 0)
            {
                funcs.AddRange(nextNodes[0].GetFuncs());
            }
            return funcs;
        }

        public abstract TaskTargetDisplayFunc CreateFunc();
    }

    #endregion

    #region ������Ϊ����

    public class Task_ActionFuncData { }
    /// <summary>
    /// ������Ϊ����
    /// </summary>
    public abstract class Task_ActionFuncNode : BaseNode
    {
        public abstract List<TaskActionFunc> GetFuncs();

        public abstract TaskActionFunc CreateFunc();
    }

    /// <summary>
    /// ����ͨ����Ϊ
    /// </summary>
    public abstract class Task_CommonActionFuncNode : Task_ActionFuncNode
    {
        public override string Title { get => "����ͨ����Ϊ"; set => base.Title = value; }
        public override string Tooltip { get => "����ͨ����Ϊ"; set => base.Tooltip = value; }

        [InputPort("���ڵ�", BasePort.Capacity.Single)]
        public Task_ActionFuncData parentNode;

        [InputPort("��һ����Ϊ", BasePort.Capacity.Single)]
        public Task_ActionFuncData nextNode;

        public override List<TaskActionFunc> GetFuncs()
        {
            List<TaskActionFunc> funcs = new List<TaskActionFunc>();
            funcs.Add(CreateFunc());
            List<Task_ActionFuncNode> nextNodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "��һ����Ϊ");
            if (nextNodes.Count > 0)
            {
                funcs.AddRange(nextNodes[0].GetFuncs());
            }
            return funcs;
        }
    }

    /// <summary>
    /// ���������Ϊ����
    /// </summary>
    public abstract class Task_AcceptActionFuncNode : Task_ActionFuncNode
    {
        public override string Title { get => "���������Ϊ����"; set => base.Title = value; }
        public override string Tooltip { get => "���������Ϊ����"; set => base.Tooltip = value; }

        [InputPort("���ڵ�", BasePort.Capacity.Single)]
        public Task_ActionFuncData parentNode;
    } 

    /// <summary>
    /// �����ύ��Ϊ����
    /// </summary>
    public abstract class Task_ExecuteActionFuncNode : Task_ActionFuncNode
    {
        public override string Title { get => "�����ύ��Ϊ����"; set => base.Title = value; }
        public override string Tooltip { get => "�����ύ��Ϊ����"; set => base.Tooltip = value; }

        [InputPort("���ڵ�", BasePort.Capacity.Single)]
        public Task_ActionFuncData parentNode;
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

        [InputPort("�׶�����", BasePort.Capacity.Single)]
        public Task_ConditionFuncData conditionFuncs;

        [InputPort("�׶�Ŀ��", BasePort.Capacity.Single)]
        public MapActorData target;

        [InputPort("�׶�Ŀ�����", BasePort.Capacity.Single)]
        public Task_DisplayFuncData targetDisplayFuncs;

        [OutputPort("�׶���Ϊ", BasePort.Capacity.Single)]
        public Task_ActionFuncData actionFuncs;

        [OutputPort("�׶γɹ�", BasePort.Capacity.Single)]
        public Task_ActionFuncData actionSuccess;

        [OutputPort("�׶�ʧ��", BasePort.Capacity.Single)]
        public Task_ActionFuncData actionFail;

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
            List<Task_ConditionFuncNode> conditionNodes = NodeHelper.GetNodeOutNodes<Task_ConditionFuncNode>(Owner, this, "�׶�����");
            if (conditionNodes.Count > 0)
            {
                return conditionNodes[0].GetFuncs();
            }
            return null;
        }

        public List<TaskTargetDisplayFunc> GetDisplayFuncs()
        {
            List<Task_TargetDisplayFuncNode> conditionNodes = NodeHelper.GetNodeOutNodes<Task_TargetDisplayFuncNode>(Owner, this, "�׶�Ŀ�����");
            if (conditionNodes.Count > 0)
            {
                return conditionNodes[0].GetFuncs();
            }
            return null;
        }

        #region ��Ϊ

        public List<TaskActionFunc> GetActionFuncs()
        {
            List<Task_ActionFuncNode> actNodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "�׶���Ϊ");
            if (actNodes.Count > 0)
            {
                return actNodes[0].GetFuncs();
            }
            return null;
        }

        public List<TaskActionFunc> GetActionSuccessFuncs()
        {
            List<Task_ActionFuncNode> actNodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "�׶γɹ�");
            if (actNodes.Count > 0)
            {
                return actNodes[0].GetFuncs();
            }
            return null;
        }

        public List<TaskActionFunc> GetActionFailFuncs()
        {
            List<Task_ActionFuncNode> actNodes = NodeHelper.GetNodeOutNodes<Task_ActionFuncNode>(Owner, this, "�׶�ʧ��");
            if (actNodes.Count > 0)
            {
                return actNodes[0].GetFuncs();
            }
            return null;
        }
        #endregion

        public TaskContent GetContent()
        {
            TaskContent content = new TaskContent();
            content.mapId = GetTargetMapId();
            content.actorIds = GetTargetActorIds();
            content.displayFuncs = GetDisplayFuncs();
            content.conditionFuncs = GetConditionFuncs();
            content.actionFuncs = GetActionFuncs();
            content.actionSuccess = GetActionSuccessFuncs();
            content.actionFail = GetActionFailFuncs();
            return content;
        }
    }

    [NodeMenuItem("�������")]
    public class Task_AcceptNode : Task_Node
    {
        public override string Title { get => $"����{taskId}����"; set => base.Title = value; }
    }

    [NodeMenuItem("�����ύ")]
    public class Task_ExecuteNode : Task_Node
    {
        public override string Title { get => $"�ύ{taskId}����"; set => base.Title = value; }

    }
}
