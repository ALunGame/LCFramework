using LCJson;
using LCNode;
using LCNode.Model;
using LCNode.Model.Internal;
using LCToolkit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCDialog.DialogGraph
{
    [CreateAssetMenu(fileName = "Dialog组", menuName = "配置组/Dialog组", order = 1)]
    public class DialogGraphGroupAsset : BaseGraphGroupAsset<DialogGraphAsset>
    {
        [Header("对话类型")]
        public DialogType dialogType;

        public override string DisplayName => "Dialog";

        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入Dialog分组名：", (string x) =>
            {
                string assetName = "dialog_" + x;
                DialogGraphAsset asset = CreateGraph(assetName) as DialogGraphAsset;
                asset.dialogType = dialogType;
            });
        }

        public override void ExportGraph(List<InternalBaseGraphAsset> assets)
        {
            Dictionary<DialogType, List<DialogGraphAsset>> dialogGroupDict = new Dictionary<DialogType, List<DialogGraphAsset>>();
            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i] is DialogGraphAsset)
                {
                    DialogGraphAsset asset = assets[i] as DialogGraphAsset;
                    if (!dialogGroupDict.ContainsKey(asset.dialogType))
                        dialogGroupDict.Add(asset.dialogType, new List<DialogGraphAsset>());
                    dialogGroupDict[asset.dialogType].Add(asset);
                }
            }

            foreach (var item in dialogGroupDict)
            {
                
                TBDialogCnf cnfTab = new TBDialogCnf();

                List<DialogModel> dialogs = new List<DialogModel>();

                DialogType dialogType = item.Key;
                List<DialogGraphAsset> dialogAssets = item.Value;
                foreach (var dialogAsset in dialogAssets)
                {
                    BaseGraph graphData = dialogAsset.DeserializeGraph();
                    dialogs.AddRange(SerializeToDialogModel(graphData, dialogAsset));
                }

                for (int i = 0; i < dialogs.Count; i++)
                {
                    DialogModel tModel = dialogs[i];
                    if (cnfTab.ContainsKey(tModel.id))
                    {
                        Debug.LogError($"Dialog配置生成失败，重复的对话Id>>>>{tModel.id}");
                        return;
                    }
                    else
                    {
                        cnfTab.Add(tModel.id, tModel);
                    }
                }

                string filePath = DialogDef.GetDialogCnfPath(dialogType);
                IOHelper.WriteText(JsonMapper.ToJson(dialogs), filePath);
                Debug.Log($"对话生成成功》》》{filePath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private List<DialogModel> SerializeToDialogModel(BaseGraph graph, DialogGraphAsset asset)
        {
            List<DialogModel> models = new List<DialogModel>();
            List<Dialog_Node> rootNodes = NodeHelper.GetNodes<Dialog_Node>(graph);
            if (rootNodes.Count <= 0)
            {
                Debug.LogError($"试图序列化出错，没有根节点");
                return models;
            }
            for (int i = 0; i < rootNodes.Count; i++)
            {
                models.Add(rootNodes[i].GetDialogModel());
            }
            return models;
        }
    }
}
