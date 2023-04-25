using System;
using LCSkill.Timeline;

namespace LCSkill.Common
{
    [TimlineGroupElement(typeof(AnimTrackGroup),"动画组")]
    public class AnimTrackGroup_Element : TrackGroup_Element<AnimTrackGroup>
    {
        public override string GroupName { get => "动画"; }
        public override string GroupToolTipName { get => "动画"; }
        
        protected override BaseTrack CreateNewTrack()
        {
            AnimTrack animTrack = new AnimTrack();
            return animTrack;
        }
    }
}