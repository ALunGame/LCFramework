using LCTimeline;
using LCToolkit;
using UnityEngine;

namespace LCSkill
{
    public class SkillTimelineGraphAsset : BaseTimelineGraphAsset<SkillTimelineGraph>
    {
        [ReadOnly]
        [Header("Timeline名")]
        public string timelineName;
    }
}