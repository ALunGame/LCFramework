using Demo.Skill.Timeline;
using LCSkill;
using LCTimeline;
using SkillSystem.ED.Timeline;
using System;
using UnityEngine;
using LCToolkit;

namespace Demo
{
    [TrackMenu("技能播放动画")]
    public class Skill_Tl_PlayAnimTrack : TLSK_TrackData
    {
        public override string TitleName => "技能播放动画";
        public override Type ClipType => typeof(Skill_Tl_PlayAnimClip);
    }

    [ClipMenu("播放动画")]
    public class Skill_Tl_PlayAnimClip : TLSK_ClipData
    {
        public override string TitleName => $"播放动画{anim.ObjName}";
        public UnityObjectAsset anim = new UnityObjectAsset(UnityObjectAsset.AssetType.AnimClip); 

        public override TimelineFunc CreateFunc()
        {
            SkillTimeline_PlayAnim func = new SkillTimeline_PlayAnim();
            func.animName = anim.ObjName;
            return func;
        }
    }

    [TrackMenu("技能位移")]
    public class Skill_Tl_MoveTrack : TLSK_TrackData
    {
        public override string TitleName => "技能位移";
        public override Type ClipType => typeof(Skill_Tl_MoveClip);
    }

    [ClipMenu("技能位移")]
    public class Skill_Tl_MoveClip : TLSK_ClipData
    {
        public override string TitleName => "技能位移";
        public Vector2 movePos;

        public override TimelineFunc CreateFunc()
        {
            SkillTimeline_Move func = new SkillTimeline_Move();
            func.movePos = movePos;
            return func;
        }
    }
}
