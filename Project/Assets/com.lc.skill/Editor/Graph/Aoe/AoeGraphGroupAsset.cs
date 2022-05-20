using LCJson;
using LCNode;
using LCNode.Model;
using LCNode.Model.Internal;
using LCToolkit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCSkill.AoeGraph
{
    [CreateAssetMenu(fileName = "Aoe组", menuName = "配置组/Aoe组", order = 1)]
    public class AoeGraphGroupAsset : BaseGraphGroupAsset<AoeGraphAsset>
    {
        public override string DisplayName => "Aoe";

        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入AoeId：", (string x) =>
            {
                string assetName = "aoe_" + x;
                AoeGraphAsset asset = CreateGraph(assetName) as AoeGraphAsset;
                asset.aoeId = x;
            });
        }

        public override void ExportGraph(List<InternalBaseGraphAsset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i] is AoeGraphAsset)
                {
                    AoeGraphAsset asset = assets[i] as AoeGraphAsset;

                    BaseGraph graphData = asset.DeserializeGraph();

                    //运行时数据结构
                    AoeModel model = SerializeToAoeModel(graphData, asset);

                    string filePath = SkillDef.GetAoeCnfPath(asset.aoeId);
                    IOHelper.WriteText(JsonMapper.ToJson(model), filePath);

                    Debug.Log($"Aoe配置生成成功>>>>{filePath}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private AoeModel SerializeToAoeModel(BaseGraph graph, AoeGraphAsset asset)
        {
            List<Aoe_Node> rootNodes = NodeHelper.GetNodes<Aoe_Node>(graph);
            if (rootNodes.Count <= 0)
            {
                Debug.LogError($"试图序列化出错，没有根节点");
            }
            Aoe_Node node = rootNodes[0];

            AoeModel aoeModel = new AoeModel();
            aoeModel.id = asset.aoeId;
            aoeModel.asset = node.asset;
            aoeModel.areaShape = node.areaShape;
            aoeModel.tickTime = node.tickTime;

            aoeModel.moveFunc = node.GetMoveFunc(); 
            aoeModel.onCreateFunc = node.GetOnCreateFuncs();
            aoeModel.onTickFunc = node.GetOnTickFuncs();
            aoeModel.onRemovedFunc = node.GetOnRemovedFuncs();

            aoeModel.onActorEnterFunc = node.GetOnActorEnterFuncs();
            aoeModel.onActorLeaveFunc = node.GetOnActorLeaveFuncs();

            aoeModel.onBulletEnterFunc = node.GetOnBulletEnterFuncs();
            aoeModel.onBulletLeaveFunc = node.GetOnBulletLeaveFuncs();

            return aoeModel;
        }
    }
}
