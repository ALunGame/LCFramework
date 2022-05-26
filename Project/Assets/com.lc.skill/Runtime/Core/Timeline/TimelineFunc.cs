namespace LCSkill
{
    /// <summary>
    /// 技能Timeline函数
    /// </summary>
    public abstract class TimelineFunc
    {
        /// <summary>
        /// 节点开始时间
        /// </summary>
        public float timeStart;

        /// <summary>
        /// 节点持续时间
        /// </summary>
        public float timeContinue;

        /// <summary>
        /// 进入时
        /// </summary>
        /// <param name="timelineObj"></param>
        public abstract void Enter(TimelineObj timelineObj);

        /// <summary>
        /// 离开时
        /// </summary>
        /// <param name="timelineObj"></param>
        public virtual void Exit(TimelineObj timelineObj)
        {

        }

        /// <summary>
        /// 每帧调用
        /// </summary>
        /// <param name="timelineObj"></param>
        public virtual void Tick(TimelineObj timelineObj)
        {

        }
    }

    /// <summary>
    /// 创建Aoe
    /// </summary>
    public class SkillTimeline_CreateAoe : TimelineFunc
    {
        public AddAoeModel addAoe;

        public override void Enter(TimelineObj timelineObj)
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

        public override void Enter(TimelineObj timelineObj)
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

        public override void Enter(TimelineObj timelineObj)
        {
            SkillLocate.Skill.CreateBuff(timelineObj.ower, timelineObj.ower, addBuff);
        }
    }
}
