using LCSkill.Timeline;

namespace LCSkill.Common
{
    [TimlineTrackElement(typeof(RaycastTrack))]
    public class RaycastTrack_Element : Track_Element<RaycastTrack>
    {
        protected override string CreateClipMenuName
        {
            get => "特效片段";
        }

        protected override BaseClip CreateNewClip()
        {
            RaycastClip clip = new RaycastClip();
            return clip;
        }
    }
}