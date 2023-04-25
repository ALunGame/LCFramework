using System;
using LCSkill.Timeline;

namespace LCSkill.Common
{
    [TimlineGroupElement(typeof(ParticleSystemTrackGroup),"特效组")]
    public class ParticleSystemGroup_Element: TrackGroup_Element<ParticleSystemTrackGroup>
    {
        public override string GroupName { get => "特效"; }
        public override string GroupToolTipName { get => "特效"; }
        
        protected override BaseTrack CreateNewTrack()
        {
            ParticleSystemTrack track = new ParticleSystemTrack();
            return track;
        }
    }
}