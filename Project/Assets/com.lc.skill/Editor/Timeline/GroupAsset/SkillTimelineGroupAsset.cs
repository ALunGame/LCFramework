using LCToolkit;
using UnityEngine;

namespace LCSkill.Timeline
{
    [CreateAssetMenu(fileName = "技能Timeline组", menuName = "SKill/Timeline组", order = 1)]
    public class SkillTimelineGroupAsset : GroupAsset<SkillTimelineAsset>
    {
        public override string DisplayName { get => "SkillTimeline";  }
        
        public override string ExportChildAsset(GroupChildAsset pAsset)
        {
            string str = "";
            return str;
        }
    }
}