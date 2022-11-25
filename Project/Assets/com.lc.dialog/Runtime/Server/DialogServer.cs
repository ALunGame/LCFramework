using LCMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCDialog
{
    public class DialogServer : IDialogServer
    {
        //正在说话的演员列表
        private List<string> speakIngActors = new List<string>();
        //对话字典
        private Dictionary<string, DialogObj> dialogObjs = new Dictionary<string, DialogObj>();

        public string CreateDialog(AddDialogInfo addDialogInfo)
        {
            string uid = CalcDialogUid(addDialogInfo);
            if (dialogObjs.ContainsKey(uid))
            {
                DialogLocate.Log.LogError("创建对话失败>>>>Uid重复", uid);
                return "";
            }
            if (DialogLocate.Config.GetDialogModel(addDialogInfo.DialogType, addDialogInfo.DialogId, out var model))
            {
                List<string> actorUids = GetDialogActorUids(addDialogInfo, model);
                for (int i = 0; i < actorUids.Count; i++)
                {
                    if (speakIngActors.Contains(actorUids[i]))
                    {
                        DialogLocate.Log.Log("创建对话失败>>>>对话需要的演员正在对话", uid, actorUids[i]);
                        return "";
                    }
                }
                DialogObj dialogObj = new DialogObj(uid, addDialogInfo.DialogType, addDialogInfo.DialogId, addDialogInfo.DialogStep, model);
                dialogObj.SetSponsor(addDialogInfo.Sponsor);
                dialogObj.SetTargets(addDialogInfo.Targets);
                dialogObjs.Add(uid, dialogObj);
                DialogLocate.Display.OnCreateDialog(dialogObj, actorUids);
                return uid;
            }
            else
            {
                DialogLocate.Log.LogError("创建对话失败>>>>没有配置", uid);
                return "";
            }
        }

        public void Play(string uid)
        {
            if (!dialogObjs.ContainsKey(uid))
            {
                DialogLocate.Log.LogError("播放对话失败>>>>没有此对话", uid);
                return;
            }
            DialogObj dialog = dialogObjs[uid];
            if (DialogLocate.Config.GetDialogStepModel(dialog.Model, dialog.CurrStep,out var model))
            {
                //执行函数
                if (model.onPlayFuncs != null && model.onPlayFuncs.Count > 0)
                {
                    for (int i = 0; i < model.onPlayFuncs.Count; i++)
                    {
                        model.onPlayFuncs[i].Execute(dialog, dialog.CurrStep);
                    }
                }
                //执行表现
                DialogLocate.Display.OnPlayDialog(dialog,model);
            }
            else
            {
                Close(uid);
            }
        }

        public void PlayNext(string uid)
        {
            if (!dialogObjs.ContainsKey(uid))
            {
                DialogLocate.Log.LogError("播放下一步对话失败>>>>没有此对话", uid);
                return;
            }
            DialogObj dialog = dialogObjs[uid];
            dialog.SetStep(dialog.CurrStep + 1);
            Play(uid);
        }

        public void Close(string uid)
        {
            if (!dialogObjs.ContainsKey(uid))
            {
                DialogLocate.Log.LogError("关闭对话失败>>>>没有此对话", uid);
                return;
            }
            DialogObj dialog = dialogObjs[uid];
            dialogObjs.Remove(uid);
            //执行表现
            DialogLocate.Display.OnCloseDialog(dialog);
        }

        public void ClickDispose(string uid, int disposeId)
        {
            if (!dialogObjs.ContainsKey(uid))
            {
                DialogLocate.Log.LogError("点击选项失败>>>>没有此对话", uid);
                return;
            }
            DialogObj dialog = dialogObjs[uid];
            if (DialogLocate.Config.GetDialogStepModel(dialog.Model, dialog.CurrStep, out var model))
            {
                if (DialogLocate.Config.GetDialogDisposeModel(model, disposeId, out var disposeModel))
                {
                    //返回判断
                    if (disposeModel.backToStep > 0)
                    {
                        if (DialogLocate.Config.GetDialogStepModel(dialog.Model, disposeModel.backToStep, out var nextStepModel))
                        {
                            dialog.SetStep(disposeModel.backToStep);
                            Play(uid);
                        }
                        else
                        {
                            DialogLocate.Log.LogError("点击选项失败>>>>返回步骤出错", uid, disposeModel.backToStep);
                            return;
                        }
                    }
                    else
                    {
                        Close(uid);
                    }
                    //执行函数
                    if (disposeModel.onChooseFuncs != null && disposeModel.onChooseFuncs.Count > 0)
                    {
                        for (int i = 0; i < disposeModel.onChooseFuncs.Count; i++)
                        {
                            disposeModel.onChooseFuncs[i].Execute(dialog, disposeId);
                        }
                    }
                    //执行表现
                    DialogLocate.Display.OnClickDispose(dialog, disposeId);
                }
                else
                {
                    DialogLocate.Log.LogError("点击选项失败>>>>没有此选项", uid, disposeId);
                }
            }
            else
            {
                DialogLocate.Log.LogError("点击选项失败>>>>没有此步骤", uid, dialog.CurrStep);
            }
        }

        /// <summary>
        /// 计算对话Uid
        /// </summary>
        /// <param name="addDialogInfo"></param>
        private string CalcDialogUid(AddDialogInfo addDialogInfo)
        {
            return addDialogInfo.ToString();
        }

        /// <summary>
        /// 获得对话影响所有的演员Uid
        /// </summary>
        /// <param name="addDialogInfo"></param>
        /// <param name="dialogModel"></param>
        /// <returns></returns>
        private List<string> GetDialogActorUids(AddDialogInfo addDialogInfo, DialogModel dialogModel)
        {
            List<string> actorUids = new List<string>();
            if (addDialogInfo.Sponsor != null)
                actorUids.Add(addDialogInfo.Sponsor.Uid);
            if (addDialogInfo.Targets != null && addDialogInfo.Targets.Count > 0)
            {
                for (int i = 0; i < addDialogInfo.Targets.Count; i++)
                {
                    if (!actorUids.Contains(addDialogInfo.Targets[i].Uid))
                    {
                        actorUids.Add(addDialogInfo.Targets[i].Uid);
                    }
                }
            }

            List<int> actorIds = CollectDialogActorIds(dialogModel);
            for (int i = 0; i < actorIds.Count; i++)
            {
                List<Actor> actors = MapLocate.Map.GetActors(actorIds[i]);
                for (int j = 0; j < actors.Count; j++)
                {
                    if (!actorUids.Contains(actors[j].Uid))
                    {
                        actorUids.Add(actors[j].Uid);
                    }
                }
            }

            return actorUids;
        }

        /// <summary>
        /// 收集对话配置的演员Id
        /// </summary>
        /// <returns></returns>
        private List<int> CollectDialogActorIds(DialogModel dialogModel)
        {
            List<int> actors = new List<int>(); 
            if (dialogModel.steps == null || dialogModel.steps.Count <= 0)
            {
                return actors;
            }
            for (int i = 0; i < dialogModel.steps.Count; i++)
            {
                DialogStepModel stepModel = dialogModel.steps[i];
                if (stepModel.speakers != null && stepModel.speakers.Count > 0)
                {
                    for (int j = 0; j < stepModel.speakers.Count; j++)
                    {
                        if (!actors.Contains(stepModel.speakers[j]))
                        {
                            actors.Add(stepModel.speakers[j]);
                        }
                    }
                }
            }
            return actors;
        }
    }
}
