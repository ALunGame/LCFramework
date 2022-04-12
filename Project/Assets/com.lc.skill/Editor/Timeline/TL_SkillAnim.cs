using LCSkill;
using LCTimeline;
using LCTimeline.Player;
using LCTimeline.View;
using UnityEngine;
using LCToolkit;
using LCToolkit.Help;

namespace SkillSystem.ED.Timeline
{
    #region Track

    public class TLSK_AnimTrackData : TLSK_TrackData
    {
    }


    [TimelineView(typeof(TLSK_AnimTrackData))]
    [TimelineTrack("通用动画轨道", typeof(TLSK_AnimClipView))]
    public class TLSK_AnimTrackView : BaseTrackView
    {
        public override string DisplayName => "通用动画轨道";
    }

    #endregion

    #region Clip

    /// <summary>
    /// 通用动画
    /// </summary>
    public class TLSK_AnimClipData : TLSK_ClipData
    {
        public string animName;
    }

    public class TLSK_AnimClipPlayer : BaseClipPlayer
    {
        public TLSK_AnimClipPlayer(BaseClipView clipView) : base(clipView)
        {
        }

        public override void OnStart()
        {
        }

        public override void OnPlaying()
        {
            PlayAnim(clipRunningTime);
        }

        public override void OnEnd()
        {
        }

        private void PlayAnim(double animTime)
        {
            //GameObject go = GetAnimGo();
            //if (go == null)
            //    return;

            //TLSK_AnimClipData data = (TLSK_AnimClipData)View.Data;
            //string animName = data.animName;
            //if (string.IsNullOrEmpty(animName))
            //    return;

            //EditorPlayHelper.PlayAnim(go, animName, (float)animTime);
        }
    }

    /// <summary>
    /// 通用动画片段
    /// </summary>
    [TimelineClip("通用动画片段")]
    [TimelineView(typeof(TLSK_AnimClipData), typeof(TLSK_AnimClipPlayer))]
    public class TLSK_AnimClipView : BaseClipView
    {
        public override string DisplayName => ((TLSK_AnimClipData)Data).animName;
    }

    #endregion
}
