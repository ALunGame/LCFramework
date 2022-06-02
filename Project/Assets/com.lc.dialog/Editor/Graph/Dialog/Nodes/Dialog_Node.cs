using LCNode;
using LCNode.Model;
using System.Collections.Generic;

namespace LCDialog.DialogGraph
{
    #region 对话选项

    public class DialogDisposeFuncData { }

    /// <summary>
    /// 对话选项函数
    /// </summary>
    public abstract class Dialog_DisposeFuncNode : BaseNode
    {
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
        public override string Tooltip { get => "对话选项"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public DialogDisposeData parentNode;

        [NodeValue("返回第几步对话")]
        public int backToStep = -1;

        [NodeValue("选项内容")]
        public string content = "";

        [OutputPort("当点击选项时", BasePort.Capacity.Multi)]
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
        public override string Tooltip { get => "对话步骤函数"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public DialogStepFuncData parentNode;

        public abstract DialogStepFunc CreateFunc();
    }

    public class DialogSpeakerData { }

    /// <summary>
    /// 对话说话的说话演员
    /// </summary>
    [NodeMenuItem("说话对象")]
    public class Dialog_SpeakerNode : BaseNode
    {
        [NodeValue("说话对象Id")]
        public int speakerId = 0;

        [OutputPort("父节点", BasePort.Capacity.Single, BasePort.Orientation.Vertical)]
        public DialogSpeakerData parentNode;

        public int GetSpeakerId()
        {
            return speakerId;
        }
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


        public override string Tooltip { get => "对话步骤"; set => base.Tooltip = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public DialogStepData parentNode;

        [InputPort("说话的对象", BasePort.Capacity.Multi, BasePort.Orientation.Vertical)]
        public DialogSpeakerData speaker;

        /// <summary>
        /// 谈话内容
        /// </summary>
        public string content = "";

        [NodeValue("说话的对象类型", "当指定说话对象此字段无效")]
        public SpeakerType speakerType = SpeakerType.无;

        [NodeValue("动画")]
        public string anim = "";

        [OutputPort("对话选项", BasePort.Capacity.Multi)]
        public DialogDisposeData disposes;

        [OutputPort("当播放对话时", BasePort.Capacity.Multi)]
        public DialogStepFuncData onPlayFunc;

        public DialogStepModel GetStepModel()
        {
            DialogStepModel model = new DialogStepModel();
            model.content = content;
            model.anim = anim;

            //说话对象
            model.speakerType = (LCDialog.SpeakerType)((int)speakerType);
            model.speakers = new List<int>();
            List<Dialog_SpeakerNode> speakerNodes = NodeHelper.GetNodeInNodes<Dialog_SpeakerNode>(Owner, this, "说话的对象");
            if (speakerNodes.Count > 0)
            {
                for (int i = 0; i < speakerNodes.Count; i++)
                {
                    model.speakers.Add(speakerNodes[i].GetSpeakerId());
                }
            }

            //分支
            model.disposes = new List<DialogDisposeModel>();
            List<Dialog_DisposeNode> disposeNodes = NodeHelper.GetNodeOutNodes<Dialog_DisposeNode>(Owner, this, "对话选项");
            if (disposeNodes.Count > 0)
            {
                for (int i = 0; i < disposeNodes.Count; i++)
                {
                    DialogDisposeModel disposeModel = disposeNodes[i].GetDisposeModel();
                    disposeModel.id = i;
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

        [OutputPort("对话步骤", BasePort.Capacity.Multi)]
        public DialogStepData steps;

        public DialogModel GetDialogModel()
        {
            DialogModel model = new DialogModel();
            model.id = id;

            //步骤
            model.steps = new List<DialogStepModel>();
            List<Dialog_StepNode> stepNodes = NodeHelper.GetNodeOutNodes<Dialog_StepNode>(Owner, this, "对话步骤");
            if (stepNodes.Count > 0)
            {
                for (int i = 0; i < stepNodes.Count; i++)
                {
                    model.steps.Add(stepNodes[i].GetStepModel());
                }
            }
            return model;
        }
    }
}
