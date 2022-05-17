using LCNode.Model;
using LCToolkit;
using UnityEngine;

namespace LCSkill.SkillGraph
{
    public class SkillGraphAsset : BaseGraphAsset<SkillGraph>
    {
        [ReadOnly]
        [Header("技能Id")]
        public string skillId;
    }
}
