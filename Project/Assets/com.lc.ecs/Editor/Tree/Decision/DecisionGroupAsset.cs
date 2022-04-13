using LCECS.Model;
using LCHelp;
using LCJson;
using LCNode.Model;
using LCNode.Model.Internal;
using UnityEditor;
using UnityEngine;

namespace LCECS.Tree
{
    [CreateAssetMenu(fileName = "决策组", menuName = "配置组/决策组", order = 2)]
    internal class DecisionGroupAsset : BaseGraphGroupAsset<DecisionAsset>
    {
        public override string DisplayName => "决策树";

        public override void ExportGraph(InternalBaseGraphAsset graph)
        {
            DecisionAsset decisionAsset = graph as DecisionAsset;
            BaseGraph graphData = decisionAsset.DeserializeGraph();

            //运行时数据结构
            DecisionModel model = new DecisionModel();
            model.id = decisionAsset.TreeId;
            model.tree = SerializeHelp.SerializeToTree(graphData);

            string filePath = ECSDefPath.GetDecTreePath(decisionAsset.name);
            LCIO.WriteText(JsonMapper.ToJson(model), filePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"决策树生成成功>>>>{filePath}");
        }
    }
}
