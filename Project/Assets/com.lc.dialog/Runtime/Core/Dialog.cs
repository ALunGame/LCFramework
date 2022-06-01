using LCMap;
using System.Collections.Generic;

namespace LCDialog
{
    /// <summary>
    /// 对话选项配置
    /// </summary>
    public struct DialogDisposeModel
    {
        /// <summary>
        /// 选项Id
        /// </summary>
        public int id;

        /// <summary>
        /// 是否可以重复选择
        /// </summary>
        public bool canRepeat;

        /// <summary>
        /// 选项内容
        /// </summary>
        public string content;

        /// <summary>
        /// 点击选项调用
        /// </summary>
        public List<DialogDisposeFunc> onChooseFuncs;
    }

    /// <summary>
    /// 说话对象类型
    /// </summary>
    public enum SpeakerType
    {
        None,
        /// <summary>
        /// 对话发起者
        /// </summary>
        Sponsor,
        /// <summary>
        /// 对话目标
        /// </summary>
        Target,
    }

    /// <summary>
    /// 每一步对话配置
    /// </summary>
    public struct DialogStepModel
    {
        /// <summary>
        /// 第几步
        /// </summary>
        public int step;

        /// <summary>
        /// 说话的人
        /// </summary>
        public List<int> speakers;

        /// <summary>
        /// 说话对象类型,当指定说话对象时此字段无效
        /// </summary>
        public SpeakerType speakerType;

        /// <summary>
        /// 说话的内容
        /// </summary>
        public string content;

        /// <summary>
        /// 说话的人播放的动画
        /// </summary>
        public string anim;

        /// <summary>
        /// 播放对话时调用
        /// </summary>
        public List<DialogStepFunc> onPlayFuncs;

        /// <summary>
        /// 选项
        /// </summary>
        public List<DialogDisposeModel> disposes;
    }

    /// <summary>
    /// 对话配置数据
    /// </summary>
    public struct DialogModel
    {
        /// <summary>
        /// 对话Id
        /// </summary>
        public int id;

        public List<DialogStepModel> steps;
    }

    /// <summary>
    /// 运行时创建的一个对话对象
    /// </summary>
    public class DialogObj
    {
        /// <summary>
        /// 唯一Id
        /// </summary>
        public string Uid { get; private set; }

        /// <summary>
        /// 对话Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 数据
        /// </summary>
        public DialogModel Model;

        /// <summary>
        /// 当前对话第几步
        /// </summary>
        public int CurrStep { get; private set; }

        /// <summary>
        /// 对话的发起者
        /// </summary>
        public ActorObj Sponsor { get; private set; }

        /// <summary>
        /// 对话目标
        /// </summary>
        public List<ActorObj> Targets { get; private set; }
    }

    /// <summary>
    /// 创建一个对话
    /// </summary>
    public class AddDialogInfo
    {
        /// <summary>
        /// 对话的发起者（可以为空）
        /// </summary>
        public ActorObj Sponsor;

        /// <summary>
        /// 对话的目标（可以为空）
        /// </summary>
        public List<ActorObj> Targets;

        /// <summary>
        /// 对话Id
        /// </summary>
        public int DialogId;

        /// <summary>
        /// 对话步骤
        /// </summary>
        public int DialogStep;

        public AddDialogInfo(int dialogId,int dialogStep)
        {
            this.DialogId = dialogId;
            this.DialogStep = dialogStep;
        }
    }
}
