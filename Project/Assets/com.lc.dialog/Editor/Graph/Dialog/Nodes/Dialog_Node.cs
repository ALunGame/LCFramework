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
    /// 对话选项函数 ,目前还没有实现
    /// </summary>
    
    //
    //[MemoryPackUnion(0, typeof(FooClass))]
    public abstract partial class Dialog_DisposeFuncNode : BaseNode
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
    public partial class Dialog_DisposeNode : BaseNode
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
    
    public partial class Dialog_SpeakerNode : Map_ActorNode
    {
        [OutputPort("父节点", BasePort.Capacity.Single, BasePort.Orientation.Vertical)]

        public MapActorData parentNode;
    }

    public class DialogStepData { }

    
    public abstract partial class Dialog_BaseStepNode : BaseNode
    {
        [InputPort("父节点", BasePort.Capacity.Single)]
        public DialogStepData parentNode;

        [OutputPort("下一步", BasePort.Capacity.Single)]
        public DialogStepData nextStep;

        public void GetNextStep(Dialog_BaseStepNode node, ref List<Dialog_BaseStepNode> resNodes)
        {
            resNodes.Add(node);
            List<Dialog_BaseStepNode> nodes = NodeHelper.GetNodeOutNodes<Dialog_BaseStepNode>(Owner, node, "下一步");
            if (nodes.Count > 0)
            {
                GetNextStep(nodes[0], ref resNodes);
            }
        }

        public abstract DialogStepModel GetStepModel();
    }

    /// <summary>
    /// 对话步骤
    /// </summary>
    [NodeMenuItem("步骤")]
    public partial class Dialog_StepNode : Dialog_BaseStepNode
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

        public override DialogStepModel GetStepModel()
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
    public partial class Dialog_DisposeStepNode : Dialog_BaseStepNode
    {
        public override string Title { get => "选项步骤"; set => base.Title = value; }
        public override Color TitleColor { get => Color.red; set => base.TitleColor = value; }
        public override string Tooltip { get => "选项步骤"; set => base.Tooltip = value; }

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

        public override DialogStepModel GetStepModel()
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
    public partial class Dialog_Node : BaseNode
    {
        [NodeValue("对话Id")]
        public int id;

        [OutputPort("对话步骤", BasePort.Capacity.Single)]
        public DialogStepData steps;

        public DialogModel GetDialogModel()
        {
            DialogModel model = new DialogModel();
            model.id = id;

            //步骤
            model.steps = new List<DialogStepModel>();

            List<Dialog_BaseStepNode> resNodes = new List<Dialog_BaseStepNode>();
            List<Dialog_BaseStepNode> nodes = NodeHelper.GetNodeOutNodes<Dialog_BaseStepNode>(Owner, this, "对话步骤");
            if (nodes.Count > 0)
            {
                nodes[0].GetNextStep(nodes[0], ref resNodes);
            }

            for (int i = 0; i < resNodes.Count; i++)
            {
                DialogStepModel stepModel = resNodes[i].GetStepModel();
                stepModel.step = i + 1;
                model.steps.Add(stepModel);
            }
            return model;
        }
    }
}
