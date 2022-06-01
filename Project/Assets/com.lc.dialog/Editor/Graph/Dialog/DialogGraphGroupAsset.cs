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
        public override string DisplayName => "Dialog";

        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入Dialog分组名：", (string x) =>
            {
                string assetName = "dialog_" + x;
                DialogGraphAsset asset = CreateGraph(assetName) as DialogGraphAsset;
            });
        }

        public override void ExportGraph(List<InternalBaseGraphAsset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i] is DialogGraphAsset)
                {
                    DialogGraphAsset asset = assets[i] as DialogGraphAsset;

                    BaseGraph graphData = asset.DeserializeGraph();

                    //运行时数据结构
                    DialogModel model = SerializeToDialogModel(graphData, asset);

                    string filePath = DialogDef.GetDialogCnfPath();
                    IOHelper.WriteText(JsonMapper.ToJson(model), filePath);

                    Debug.Log($"Dialog配置生成成功>>>>{filePath}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private DialogModel SerializeToDialogModel(BaseGraph graph, DialogGraphAsset asset)
        {
            List<Dialog_Node> rootNodes = NodeHelper.GetNodes<Dialog_Node>(graph);
            if (rootNodes.Count <= 0)
            {
                Debug.LogError($"试图序列化出错，没有根节点");
            }
            Dialog_Node node = rootNodes[0];

            DialogModel aoeModel = new DialogModel();
            // aoeModel.id = asset.aoeId;
            // aoeModel.asset = node.asset;
            // aoeModel.areaShape = node.areaShape;
            // aoeModel.tickTime = node.tickTime;

            // aoeModel.moveFunc = node.GetMoveFunc(); 
            // aoeModel.onCreateFunc = node.GetOnCreateFuncs();
            // aoeModel.onTickFunc = node.GetOnTickFuncs();
            // aoeModel.onRemovedFunc = node.GetOnRemovedFuncs();

            // aoeModel.onActorEnterFunc = node.GetOnActorEnterFuncs();
            // aoeModel.onActorLeaveFunc = node.GetOnActorLeaveFuncs();

            // aoeModel.onBulletEnterFunc = node.GetOnBulletEnterFuncs();
            // aoeModel.onBulletLeaveFunc = node.GetOnBulletLeaveFuncs();

            return aoeModel;
        }
    }
}
