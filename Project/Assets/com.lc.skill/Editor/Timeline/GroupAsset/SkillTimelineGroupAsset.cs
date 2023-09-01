using System;
using IAToolkit;
using LCToolkit;
using UnityEngine;

namespace LCSkill.Timeline
{
    [CreateAssetMenu(fileName = "技能Timeline组", menuName = "SKill/Timeline组", order = 1)]
    public class SkillTimelineGroupAsset : GroupAsset<SkillTimelineAsset>
    {
        public override string DisplayName { get => "SkillTimeline";  }
        public override Type ChildType { get => typeof(SkillTimelineAsset); }

        public override string ExportChildAsset(GroupChildAsset pAsset)
        {
            Debug.LogWarning("没有重写导出方式");
            string str = "";
            return str;
        }
    }
}