using LCJson;
using LCNode;
using LCNode.Model;
using LCNode.Model.Internal;
using LCToolkit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCSkill.BuffGraph
{
    [CreateAssetMenu(fileName = "Buff组", menuName = "配置组/Buff组", order = 1)]
    public class BuffGraphGroupAsset : BaseGraphGroupAsset<BuffGraphAsset>
    {
        public override string DisplayName => "Buff";

        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入BuffId：", (string x) =>
            {
                string assetName = "buff_" + x;
                BuffGraphAsset asset = CreateGraph(assetName) as BuffGraphAsset;
                asset.buffId = x;
            });
        }

        public override void ExportGraph(List<InternalBaseGraphAsset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i] is BuffGraphAsset)
                {
                    BuffGraphAsset asset = assets[i] as BuffGraphAsset;

                    BaseGraph graphData = asset.DeserializeGraph();

                    //运行时数据结构
                    BuffModel model = SerializeToBuffModel(graphData, asset);

                    string filePath = SkillDef.GetBuffCnfPath(asset.buffId);
                    IOHelper.WriteText(JsonMapper.ToJson(model), filePath);

                    Debug.Log($"Buff配置生成成功>>>>{filePath}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private BuffModel SerializeToBuffModel(BaseGraph graph, BuffGraphAsset asset)
        {
            List<Buff_Node> rootNodes = NodeHelper.GetNodes<Buff_Node>(graph);
            if (rootNodes.Count <= 0)
            {
                Debug.LogError($"试图序列化出错，没有根节点");
            }
            Buff_Node node = rootNodes[0];

            BuffModel buffModel = new BuffModel();
            buffModel.id = asset.buffId;
            buffModel.name = asset.name;
            buffModel.priority = node.priority;
            buffModel.maxStack = node.maxStack;
            buffModel.tickTime = node.tickTime;

            buffModel.onFreedFunc = node.GetOnFreedFuncs();
            buffModel.onOccurFunc = node.GetOnOccurFuncs();
            buffModel.onTickFunc = node.GetOnTickFuncs();   
            buffModel.onRemovedFunc = node.GetOnRemovedFuncs();
            buffModel.onHurtFunc = node.GetOnHurtFuncs();
            buffModel.onBeHurtFunc = node.GetOnBeHurtFuncs();
            buffModel.onKilledFunc = node.GetOnKilledFuncs();
            buffModel.onBeKilledFunc = node.GetOnBeKilledFuncs();

            return buffModel;
        }
    }
}
