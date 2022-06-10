using LCMap;
using LCNode;
using LCNode.Model;
using System.Collections.Generic;
using UnityEngine;

namespace LCDialog.DialogGraph
{
    #region 对话选项

    public class DialogDisposeFuncData { }

    /// <summary>
    /// 对话选项函数
    /// </summary>
    public abstract class Dialog_DisposeFuncNode : BaseNode
    {
        public override string Title { get => "对话选项函数"; set => base.Title = value; }

        public override string Tooltip { get => "对话选项函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public DialogDisposeFuncData parentNode;

        public abstract DialogDisposeFunc CreateFunc();
    }

    public class DialogDisposeData { }

    /// <summary>
    /// 对话选项
    /// </summary>
    [NodeMenuItem("选项")]
    public class Dialog_DisposeNode : BaseNode
    {
        public override string Title { get => "对话选项"; set => base.Title = value; }
        public override string Tooltip { get => "对话选项"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public DialogDisposeData parentNode;

        [NodeValue("返回第几步对话")]
        public int backToStep = -1;

        [NodeValue("选项内容")]
        public string content = "";

        [OutputPort("当点击选项时", BasePort.Capacity.Multi, setIndex = true)]
        public DialogDisposeFuncData onChooseFunc;

        public DialogDisposeModel GetDisposeModel()
        {
            DialogDisposeModel model = new DialogDisposeModel();
            model.content = content;
            model.backToStep = backToStep;

            //函数
            model.onChooseFuncs = new List<DialogDisposeFunc>();
            List<Dialog_DisposeFuncNode> funcNodes = NodeHelper.GetNodeOutNodes<Dialog_DisposeFuncNode>(Owner, this, "当点击选项时");
            if (funcNodes.Count > 0)
            {
                for (int i = 0; i < funcNodes.Count; i++)
                {
                    model.onChooseFuncs.Add(funcNodes[i].CreateFunc());
                }
            }

            return model;
        }
    }

    #endregion

    #region 对话步骤

    public class DialogStepFuncData { }

    /// <summary>
    /// 对话步骤函数
    /// </summary>
    public abstract class Dialog_StepFuncNode : BaseNode
    {
        public override string Title { get => "对话步骤函数"; set => base.Title = value; }
        public override string Tooltip { get => "对话步骤函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public DialogStepFuncData parentNode;

        public abstract DialogStepFunc CreateFunc();
    }

    /// <summary>
    /// 对话说话的说话演员
    /// </summary>
    [NodeMenuItem("说话对象")]
    public class Dialog_SpeakerNode : Map_ActorNode
    {
        [OutputPort("父节点", BasePort.Capacity.Single, BasePort.Orientation.Vertical)]
        public MapActorData parentNode;
    }

    public class DialogStepData { }

    /// <summary>
    /// 对话步骤
    /// </summary>
    [NodeMenuItem("步骤")]
    public class Dialog_StepNode : BaseNode
    {
        public enum SpeakerType
        {
            无,
            发起者,
            目标者,
        }

        public override string Title { get => "对话步骤"; set => base.Title = value; }
        public override Color TitleColor { get => Color.green; set => base.TitleColor = value; }
        public override string Tooltip { get => "对话步骤"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public DialogStepData parentNode;

        [InputPort("说话的对象", BasePort.Capacity.Single, BasePort.Orientation.Vertical)]
        public MapActorData speaker;

        /// <summary>
        /// 谈话内容
        /// </summary>
        public string content = "";

        [NodeValue("说话的对象类型", "当指定说话对象此字段无效")]
        public SpeakerType speakerType = SpeakerType.无;

        [NodeValue("动画")]
        public string anim = "";

        [OutputPort("当播放对话时", BasePort.Capacity.Multi, setIndex = true)]
        public DialogStepFuncData onPlayFunc;

        public DialogStepModel GetStepModel()
        {
            DialogStepModel model = new DialogStepModel();
            model.content = content;
            model.anim = anim;

            //说话对象
            model.speakerType = (LCDialog.SpeakerType)((int)speakerType);
            model.speakers = new List<int>();
            List<Dialog_SpeakerNode> speakerNodes = NodeHelper.GetNodeOutNodes<Dialog_SpeakerNode>(Owner, this, "说话的对象");
            if (speakerNodes.Count > 0)
            {
                model.speakers = speakerNodes[0].GetActorIds();
            }

            //函数
            model.onPlayFuncs = new List<DialogStepFunc>();
            List<Dialog_StepFuncNode> funcNodes = NodeHelper.GetNodeOutNodes<Dialog_StepFuncNode>(Owner, this, "当播放对话时");
            if (funcNodes.Count > 0)
            {
                for (int i = 0; i < funcNodes.Count; i++)
                {
                    model.onPlayFuncs.Add(funcNodes[i].CreateFunc());
                }
            }
            return model;
        }
    }

    /// <summary>
    /// 对话选项
    /// </summary>
    [NodeMenuItem("选项步骤")]
    public class Dialog_DisposeStepNode : BaseNode
    {
        public override string Title { get => "选项步骤"; set => base.Title = value; }
        public override Color TitleColor { get => Color.red; set => base.TitleColor = value; }
        public override string Tooltip { get => "选项步骤"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public DialogStepData parentNode;

        [InputPort("说话的对象", BasePort.Capacity.Single, BasePort.Orientation.Vertical)]
        public MapActorData speaker;

        [NodeValue("说话的对象类型", "当指定说话对象此字段无效")]
        public Dialog_StepNode.SpeakerType speakerType = Dialog_StepNode.SpeakerType.无;

        [NodeValue("动画")]
        public string anim = "";

        [OutputPort("当播放对话时", BasePort.Capacity.Multi, setIndex = true)]
        public DialogStepFuncData onPlayFunc;

        [OutputPort("对话选项", BasePort.Capacity.Multi, setIndex = true)]
        public DialogDisposeData disposes;

        public DialogStepModel GetStepModel()
        {
            DialogStepModel model = new DialogStepModel();
            model.content = "";
            model.anim = anim;

            //说话对象
            model.speakerType = (LCDialog.SpeakerType)((int)speakerType);
            model.speakers = new List<int>();
            List<Dialog_SpeakerNode> speakerNodes = NodeHelper.GetNodeOutNodes<Dialog_SpeakerNode>(Owner, this, "说话的对象");
            if (speakerNodes.Count > 0)
            {
                model.speakers = speakerNodes[0].GetActorIds();
            }

            //分支
            model.disposes = new List<DialogDisposeModel>();
            List<Dialog_DisposeNode> disposeNodes = NodeHelper.GetNodeOutNodes<Dialog_DisposeNode>(Owner, this, "对话选项");
            if (disposeNodes.Count > 0)
            {
                for (int i = 0; i < disposeNodes.Count; i++)
                {
                    DialogDisposeModel disposeModel = disposeNodes[i].GetDisposeModel();
                    disposeModel.id = i + 1;
                    model.disposes.Add(disposeModel);
                }
            }

            //函数
            model.onPlayFuncs = new List<DialogStepFunc>();
            List<Dialog_StepFuncNode> funcNodes = NodeHelper.GetNodeOutNodes<Dialog_StepFuncNode>(Owner, this, "当播放对话时");
            if (funcNodes.Count > 0)
            {
                for (int i = 0; i < funcNodes.Count; i++)
                {
                    model.onPlayFuncs.Add(funcNodes[i].CreateFunc());
                }
            }

            return model;
        }
    }


    #endregion

    [NodeMenuItem("对话")]
    public class Dialog_Node : BaseNode
    {
        [NodeValue("对话Id")]
        public int id;

        [OutputPort("对话步骤", BasePort.Capacity.Multi, setIndex = true)]
        public DialogStepData steps;

        public DialogModel GetDialogModel()
        {
            DialogModel model = new DialogModel();
            model.id = id;

            //步骤
            model.steps = new List<DialogStepModel>();
            List<BaseNode> childNodes = NodeHelper.GetNodeOutNodes(Owner, this, "对话步骤");
            for (int i = 0; i < childNodes.Count; i++)
            {
                if (childNodes[i] is Dialog_StepNode)
                {
                    DialogStepModel stepModel = ((Dialog_StepNode)childNodes[i]).GetStepModel();
                    stepModel.step = i + 1;
                    model.steps.Add(stepModel);
                }
                else if (childNodes[i] is Dialog_DisposeStepNode)
                {
                    DialogStepModel stepModel = ((Dialog_DisposeStepNode)childNodes[i]).GetStepModel();
                    stepModel.step = i + 1;
                    model.steps.Add(stepModel);
                }
            }
            return model;
        }
    }
}
