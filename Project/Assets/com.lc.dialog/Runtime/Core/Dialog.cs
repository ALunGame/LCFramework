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
        /// 选项内容
        /// </summary>
        public string content;

        /// <summary>
        /// 返回第几步对话
        /// </summary>
        public int backToStep;

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
        /// 对话类型
        /// </summary>
        public DialogType DialogType;

        /// <summary>
        /// 数据
        /// </summary>
        public DialogModel Model;

        /// <summary>
        /// 对话Id
        /// </summary>
        public int DialogId { get; private set; }

        /// <summary>
        /// 当前对话第几步
        /// </summary>
        public int CurrStep { get; private set; }

        /// <summary>
        /// 对话的发起者
        /// </summary>
        public Actor Sponsor { get; private set; }

        /// <summary>
        /// 对话目标
        /// </summary>
        public List<Actor> Targets { get; private set; }

        public DialogObj(string uid, DialogType dialogType, int dialogId,int dialogStep, DialogModel model)
        {
            this.Uid = uid;
            this.DialogType = dialogType;
            this.DialogId = dialogId;
            this.CurrStep = dialogStep;
            this.Model = model;
        }

        public void SetSponsor(Actor actor)
        {
            Sponsor = actor;
        }

        public void SetTargets(List<Actor> actors)
        {
            Targets = actors;
        }

        public void SetStep(int step)
        {
            CurrStep = step;
        }
    }

    /// <summary>
    /// 创建一个对话
    /// </summary>
    public class AddDialogInfo
    {
        /// <summary>
        /// 对话的发起者（可以为空）
        /// </summary>
        public Actor Sponsor;

        /// <summary>
        /// 对话的目标（可以为空）
        /// </summary>
        public List<Actor> Targets;

        /// <summary>
        /// 对话类型
        /// </summary>
        public DialogType DialogType;

        /// <summary>
        /// 对话Id
        /// </summary>
        public int DialogId;

        /// <summary>
        /// 对话步骤
        /// </summary>
        public int DialogStep;

        /// <summary>
        /// 对话影响的演员Uids
        /// </summary>
        public List<int> ActorUids;

        public AddDialogInfo(DialogType dialogType,int dialogId,int dialogStep)
        {
            this.DialogType = dialogType;
            this.DialogId = dialogId;
            this.DialogStep = dialogStep;
        }

        public override string ToString()
        {
            if (Sponsor == null)
            {
                return $"{DialogType}_{DialogId}";
            }
            Actor sponsor = Sponsor;
            return $"{DialogType}_{DialogId}_{sponsor.Uid}";
        }
    }
}
