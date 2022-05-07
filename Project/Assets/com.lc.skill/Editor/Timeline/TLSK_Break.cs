using LCTimeline;
using LCTimeline.View;

namespace SkillSystem.ED.Timeline
{
    #region Track

    public class TLSK_BreakTrackData : TLSK_TrackData
    {
    }


    [TimelineView(typeof(TLSK_BreakTrackData))]
    [TimelineTrack("技能打断", typeof(TLSK_BreakClipView))]
    public class TLSK_BreakTrackView : BaseTrackView
    {
        public override string DisplayName => "技能打断";
    }

    #endregion

    #region Clip

    /// <summary>
    /// 通用动画
    /// </summary>
    public class TLSK_BreakClipData : TLSK_ClipData
    {
        public string animName;
    }

    /// <summary>
    /// 通用动画片段
    /// </summary>
    [TimelineClip("技能打断片段")]
    [TimelineView(typeof(TLSK_BreakClipData))]
    public class TLSK_BreakClipView : BaseClipView
    {
        public override string DisplayName => "技能打断";
    }

    #endregion
}