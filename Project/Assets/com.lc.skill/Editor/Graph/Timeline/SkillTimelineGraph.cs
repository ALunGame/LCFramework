using LCTimeline;
using LCTimeline.View;
using LCToolkit;
using SkillSystem.ED.Timeline;
using System;
using System.Collections.Generic;

namespace LCSkill
{
    public class SkillTimelineGraph : BaseTimelineGraph
    {
        [NonSerialized]
        private List<Type> NodeTypes = new List<Type>();

        protected override void OnEnabled()
        {
            base.OnEnabled();
            CollectTrackTypes();
        }

        private void CollectTrackTypes()
        {
            NodeTypes.Clear();
            AddTracks<TLSK_TrackData>();
        }

        private void AddTracks<T>()
        {
            foreach (var type in ReflectionHelper.GetChildTypes<T>())
            {
                if (type.IsAbstract)
                    continue;
                if (!AttributeHelper.TryGetTypeAttribute(type, out TrackMenuAttribute attr))
                    continue;
                NodeTypes.Add(type);
            }
        }

        public override IEnumerable<Type> GetTracks()
        {
            foreach (var type in NodeTypes)
            {
                yield return type;
            }
        }
    }
}
