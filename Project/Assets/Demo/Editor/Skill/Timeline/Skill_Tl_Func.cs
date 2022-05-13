using Demo.Skill.Timeline;
using LCSkill;
using LCTimeline;
using SkillSystem.ED.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    [TrackMenu("技能播放动画")]
    public class Skill_Tl_PlayAnimTrack : TLSK_TrackData
    {
        public override Type ClipType => typeof(Skill_Tl_PlayAnimClip);
    }

    [ClipMenu("播放动画")]
    public class Skill_Tl_PlayAnimClip : TLSK_ClipData
    {
        public string animName = "";

        public override TimelineFunc CreateFunc()
        {
            SkillTimeline_PlayAnim func = new SkillTimeline_PlayAnim();
            func.animName = animName;
            return func;
        }
    }
}
