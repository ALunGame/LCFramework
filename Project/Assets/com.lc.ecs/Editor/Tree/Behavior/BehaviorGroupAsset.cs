using LCECS.Layer.Behavior;
using LCToolkit;
using LCJson;
using LCNode.Model;
using LCNode.Model.Internal;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace LCECS.Tree
{
    [CreateAssetMenu(fileName = "行为组", menuName = "配置组/行为组", order = 3)]
    public class BehaviorGroupAsset : BaseGraphGroupAsset<BehaviorAsset>
    {
        public override string DisplayName => "行为树";

        public override void OnClickCreateBtn()
        {
            List<string> requests = new List<string>();
            foreach (var item in Enum.GetNames(typeof(RequestId)))
            {
                requests.Add(item);
            }
            MiscHelper.Menu(requests, (int x) =>
            {
                RequestId requestId = (RequestId)x;
                string assetName = "behavior_" + requestId.ToString();
                BehaviorAsset asset = CreateGraph(assetName) as BehaviorAsset;
                asset.ReqId = (RequestId)x;
            });
        }

        public override void ExportGraph(InternalBaseGraphAsset graph)
        {
            BehaviorAsset behaviorAsset = graph as BehaviorAsset;
            BaseGraph graphData = behaviorAsset.DeserializeGraph();

            //运行时数据结构
            BehaviorTree model = new BehaviorTree(behaviorAsset.ReqId, SerializeHelp.SerializeToTree(graphData));

            string filePath = ECSDefPath.GetBevTreePath(behaviorAsset.ReqId);
            IOHelper.WriteText(JsonMapper.ToJson(model), filePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"行为树生成成功>>>>{filePath}");
        }
    }
}
