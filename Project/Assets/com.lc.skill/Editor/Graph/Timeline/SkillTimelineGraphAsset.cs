using UnityEditor;
using UnityEngine;
using LCTimeline;
using LCToolkit;

namespace LCSkill
{
    public class SkillTimelineGraphAsset : BaseTimelineGraphAsset<SkillTimelineGraph>
    {
        [ReadOnly]
        [Header("Timeline名")]
        public string timelineName;
    }
}