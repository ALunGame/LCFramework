using LCJson;
using LCLoad;
using System;
using System.Collections.Generic;

namespace LCDialog
{
    public class DialogConfigServer : IDialogConfigServer
    {
        private Dictionary<DialogType, TBDialogCnf> configDict = new Dictionary<DialogType, TBDialogCnf>();

        public void Init()
        {
            foreach (var item in Enum.GetValues(typeof(DialogType)))
            {
                string assetName = DialogDef.GetDialogCnfName((DialogType)item);
                string jsonStr = LoadHelper.LoadString(assetName);
                if (string.IsNullOrEmpty(jsonStr))
                {
                    DialogLocate.Log.LogError("没有对话类型的配置", item);
                }
                else
                {
                    TBDialogCnf cnf = JsonMapper.ToObject<TBDialogCnf>(jsonStr);
                    configDict.Add((DialogType)item, cnf);
                }
            }
        }

        public void Clear()
        {
            configDict.Clear();
        }

        public TBDialogCnf GetCnf(DialogType dialogType)
        {
            return configDict[dialogType];
        }

        public bool GetDialogModel(DialogType dialogType,int dialogId,out DialogModel model)
        {
            TBDialogCnf cnf = configDict[dialogType];
            foreach (var item in cnf)
            {
                if (item.Value.id == dialogId)
                {
                    model = item.Value;
                    return true;
                }
            }
            DialogLocate.Log.LogError("获得对话配置失败>>>>", dialogId);
            model = default;
            return false;
        }

        public bool GetDialogStepModel(DialogModel model, int step, out DialogStepModel stepModel)
        {
            stepModel = default;
            if (model.steps == null || model.steps.Count <= 0)
            {
                DialogLocate.Log.LogError("获得对话步骤配置失败>>>>", model.id);
                return false;
            }
            for (int i = 0; i < model.steps.Count; i++)
            {
                if (model.steps[i].step == step)
                {
                    stepModel = model.steps[i];
                    return true;
                }
            }
            return false;
        }

        public bool GetDialogDisposeModel(DialogStepModel model, int disposeId, out DialogDisposeModel disposeModel)
        {
            disposeModel = default;
            if (model.disposes == null || model.disposes.Count <= 0)
            {
                DialogLocate.Log.LogError("获得对话选项配置失败>>>>", model.step);
                return false;
            }
            for (int i = 0; i < model.disposes.Count; i++)
            {
                if (model.disposes[i].id == disposeId)
                {
                    disposeModel = model.disposes[i];
                    return true;
                }
            }
            return false;
        }
    }
}
