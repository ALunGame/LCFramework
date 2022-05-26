using LCTimeline;
using UnityEngine;

namespace LCSkill
{
    public class SkillTimelineGraphAsset : BaseTimelineGraphAsset<SkillTimelineGraph>
    {
        [Header("Timeline名")]
        public string timelineName;
    }
}