using LCJson;
using LCNode;
using LCNode.Model;
using LCNode.Model.Internal;
using LCToolkit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCSkill.SkillGraph
{
    [CreateAssetMenu(fileName = "技能组", menuName = "配置组/技能组", order = 1)]
    public class SkillGraphGroupAsset : BaseGraphGroupAsset<SkillGraphAsset>
    {
        public override string DisplayName => "技能";

        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入技能Id：", (string x) =>
            {
                string assetName = "skill_" + x;
                SkillGraphAsset asset = CreateGraph(assetName) as SkillGraphAsset;
                asset.skillId = x;
            });
        }

        public override void ExportGraph(List<InternalBaseGraphAsset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i] is SkillGraphAsset)
                {
                    SkillGraphAsset asset = assets[i] as SkillGraphAsset;

                    BaseGraph graphData = asset.DeserializeGraph();

                    //运行时数据结构
                    SkillModel model = SerializeToSkillModel(graphData, asset);

                    string filePath = SkillDef.GetSkillCnfPath(asset.skillId);
                    IOHelper.WriteText(JsonMapper.ToJson(model), filePath);

                    Debug.Log($"技能配置生成成功>>>>{filePath}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private SkillModel SerializeToSkillModel(BaseGraph graph, SkillGraphAsset asset)
        {
            List<Skill_Node> rootNodes = NodeHelper.GetNodes<Skill_Node>(graph);
            if (rootNodes.Count <= 0)
            {
                Debug.LogError($"试图序列化出错，没有根节点");
            }
            Skill_Node node = rootNodes[0];

            SkillModel skillModel   = new SkillModel();
            skillModel.id           = asset.skillId;
            skillModel.timeline     = node.timeline;
            skillModel.name         = asset.name;
            skillModel.condition    = node.GetCondition();
            skillModel.costs        = node.GetSkillCosts();
            skillModel.addBuffs     = node.GetAddBuffs();
            return skillModel;
        }
    }
}
