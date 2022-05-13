using LCECS.Layer.Decision;
using LCToolkit;
using LCJson;
using LCNode.Model;
using LCNode.Model.Internal;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace LCECS.Tree
{
    [CreateAssetMenu(fileName = "决策组", menuName = "配置组/决策组", order = 2)]
    internal class DecisionGroupAsset : BaseGraphGroupAsset<DecisionAsset>
    {
        public override string DisplayName => "决策树";

        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入决策树Id：", (string x) =>
            {
                int treeId = int.Parse(x);
                string assetName = "decision_" + treeId;
                DecisionAsset asset = CreateGraph(assetName) as DecisionAsset;
                asset.TreeId = treeId;
            });
        }

        public override void ExportGraph(List<InternalBaseGraphAsset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                DecisionAsset decisionAsset = assets[i] as DecisionAsset;
                BaseGraph graphData = decisionAsset.DeserializeGraph();

                //运行时数据结构
                DecisionTree model = new DecisionTree(decisionAsset.TreeId, SerializeHelp.SerializeToTree(graphData));

                string filePath = ECSDefPath.GetDecTreePath(decisionAsset.TreeId);
                IOHelper.WriteText(JsonMapper.ToJson(model), filePath);
                Debug.Log($"决策树生成成功>>>>{filePath}");
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
           
        }
    }
}
