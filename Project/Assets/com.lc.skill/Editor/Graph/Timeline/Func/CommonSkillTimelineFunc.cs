using LCTimeline;
using SkillSystem.ED.Timeline;
using System;

namespace LCSkill
{
    [TrackMenu("创建Aoe")]
    public class Skill_Tl_CreateAoeTrack : TLSK_TrackData
    {
        public override string TitleName => "创建Aoe";

        public override Type ClipType => typeof(Skill_Tl_CreateAoeClip);
    }

    [ClipMenu("创建Aoe")]
    public class Skill_Tl_CreateAoeClip : TLSK_ClipData
    {
        public override string TitleName => $"创建Aoe：{aoeId}";
        public string aoeId = "";

        public override TimelineFunc CreateFunc()
        {
            SkillTimeline_CreateAoe func = new SkillTimeline_CreateAoe();
            func.aoeId = aoeId;
            return func;
        }
    }

    [TrackMenu("创建Bullet")]
    public class Skill_Tl_CreateBulletTrack : TLSK_TrackData
    {
        public override string TitleName => "创建Bullet";
        public override Type ClipType => typeof(Skill_Tl_CreateBulletClip);
    }

    [ClipMenu("创建Bullet")]
    public class Skill_Tl_CreateBulletClip : TLSK_ClipData
    {
        public override string TitleName => $"创建Bullet：{bulletId}";
        public string bulletId = "";

        public override TimelineFunc CreateFunc()
        {
            SkillTimeline_CreateBullet func = new SkillTimeline_CreateBullet();
            func.bulletId = bulletId;
            return func;
        }
    }
}
