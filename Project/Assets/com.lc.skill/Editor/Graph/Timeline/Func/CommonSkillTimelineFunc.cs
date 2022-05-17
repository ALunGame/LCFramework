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
        public override string TitleName => $"创建Aoe：{addAoe.id}";
        public AddAoeModel addAoe = new AddAoeModel();

        public override TimelineFunc CreateFunc()
        {
            SkillTimeline_CreateAoe func = new SkillTimeline_CreateAoe();
            func.addAoe = addAoe;
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
        public override string TitleName => $"创建Bullet：{addBullet.id}";
        public AddBulletModel addBullet = new AddBulletModel();

        public override TimelineFunc CreateFunc()
        {
            SkillTimeline_CreateBullet func = new SkillTimeline_CreateBullet();
            func.addBullet = addBullet;
            return func;
        }
    }
}
