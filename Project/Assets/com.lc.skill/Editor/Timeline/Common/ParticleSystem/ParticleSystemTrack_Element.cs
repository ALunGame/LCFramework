using LCSkill.Timeline;

namespace LCSkill.Common
{
    [TimlineTrackElement(typeof(ParticleSystemTrack))]
    public class ParticleSystemTrack_Element : Track_Element<ParticleSystemTrack>
    {
        protected override string CreateClipMenuName
        {
            get => "特效片段";
        }

        protected override BaseClip CreateNewClip()
        {
            ParticleSystemClip clip = new ParticleSystemClip();
            return clip;
        }
    }
}