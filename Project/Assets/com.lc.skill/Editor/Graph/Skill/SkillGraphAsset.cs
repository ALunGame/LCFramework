using LCNode.Model;
using LCToolkit;
using UnityEngine;

namespace LCSkill.SkillGraph
{
    public class SkillGraphAsset : BaseGraphAsset<SkillGraph>
    {
        [EDReadOnly]
        [Header("技能Id")]
        public int skillId;
    }
}
