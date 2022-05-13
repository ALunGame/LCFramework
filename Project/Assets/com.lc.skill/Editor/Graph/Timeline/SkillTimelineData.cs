using LCSkill;
using LCTimeline;

namespace SkillSystem.ED.Timeline
{
    public abstract class TLSK_TrackData : TrackModel
    {
        
    }

    public abstract class TLSK_ClipData : ClipModel
    {
        public TimelineFunc GetFunc()
        {
            TimelineFunc func = CreateFunc();
            func.timeElapsed = StartTime;
            return func;
        }

        /// <summary>
        /// 创建运行时函数
        /// </summary>
        /// <returns></returns>
        public abstract TimelineFunc CreateFunc();
    }
}