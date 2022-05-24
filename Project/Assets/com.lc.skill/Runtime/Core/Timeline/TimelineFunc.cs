namespace LCSkill
{
    /// <summary>
    /// 技能Timeline函数
    /// </summary>
    public abstract class TimelineFunc
    {
        /// <summary>
        /// 节点运行时间
        /// </summary>
        public float timeElapsed;

        public abstract void Execute(TimelineObj timelineObj);
    }

    /// <summary>
    /// 创建Aoe
    /// </summary>
    public class SkillTimeline_CreateAoe : TimelineFunc
    {
        public AddAoeModel addAoe;

        public override void Execute(TimelineObj timelineObj)
        {
            SkillLocate.Skill.CreateAoe(timelineObj.ower,addAoe);
        }
    }

    /// <summary>
    /// 创建子弹
    /// </summary>
    public class SkillTimeline_CreateBullet : TimelineFunc
    {
        public AddBulletModel addBullet;

        public override void Execute(TimelineObj timelineObj)
        {
            SkillLocate.Skill.CreateBullet(timelineObj.ower,addBullet);
        }
    }

    /// <summary>
    /// 创建Buff
    /// </summary>
    public class SkillTimeline_CreateBuff : TimelineFunc
    {
        public AddBuffModel addBuff;

        public override void Execute(TimelineObj timelineObj)
        {
            SkillLocate.Skill.CreateBuff(timelineObj.ower, timelineObj.ower, addBuff);
        }
    }
}
