using LCSkill.Timeline;

namespace LCSkill.Common
{
    [TimlineTrackElement(typeof(AnimTrack))]
    public class AnimTrack_Element : Track_Element<AnimTrack>
    {
        protected override string CreateClipMenuName
        {
            get => "动画片段";
        }

        protected override BaseClip CreateNewClip()
        {
            AnimClip animClip = new AnimClip();
            animClip.name = animClip.clip.ObjName;
            return animClip;
        }
    }
}