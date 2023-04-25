using System;
using LCSkill.Timeline;

namespace LCSkill.Common
{
    [TimlineGroupElement(typeof(RaycastTrackGroup),"射线检测组")]
    public class RaycastTrackGroup_Element : TrackGroup_Element<RaycastTrackGroup>
    {
        public override string GroupName { get => "射线检测"; }
        public override string GroupToolTipName { get => "射线检测"; }
        
        protected override BaseTrack CreateNewTrack()
        {
            RaycastTrack track = new RaycastTrack();
            return track;
        }
    }
}