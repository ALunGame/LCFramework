using UnityEditor;
using UnityEngine;
using LCTimeline;
using LCToolkit;

namespace LCSkill
{
    public class SkillTimelineGraphAsset : BaseTimelineGraphAsset<SkillTimelineGraph>
    {
        [EDReadOnly]
        [Header("Timeline名")]
        public string timelineName;
    }
}