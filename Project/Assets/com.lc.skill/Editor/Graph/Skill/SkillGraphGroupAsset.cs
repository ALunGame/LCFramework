using LCNode.Model;
using LCNode.Model.Internal;
using LCToolkit;
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
                int skillId = int.Parse(x);
                string assetName = "skill_" + skillId;
                SkillGraphAsset asset = CreateGraph(assetName) as SkillGraphAsset;
                asset.skillId = skillId;
            });
        }

        public override void ExportGraph(InternalBaseGraphAsset graph)
        {
            
        }
    }
}
