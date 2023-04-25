using System;
using LCSkill.Timeline;

namespace LCSkill.Common
{
    public class BreakTrackGroup : BaseTrackGroup
    {
        public int startFrame;
        public int endFrame;

        /// <summary>
        /// 持续多少帧
        /// </summary>
        public int DurationFrame
        {
            get
            {
                return endFrame - startFrame + 1;
            }
        }

        public override void Start(SkillTimelineSpec pSpec)
        {
            base.Start(pSpec);
        }

        public override void UpdateFrame(SkillTimelineSpec pSpec, int pFrame)
        {
            if (pFrame >= startFrame && pFrame <= endFrame)
            {
                pSpec.CanBreak = false;
            }
            else
            {
                pSpec.CanBreak = true;
            }
        }

        public override void End(SkillTimelineSpec pSpec)
        {
            base.End(pSpec);
        }
    }
}