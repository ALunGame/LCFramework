using Demo.Com;
using LCECS;
using LCECS.Core;
using LCSkill;

namespace Demo.Skill.Timeline
{
    /// <summary>
    /// 播放动画
    /// </summary>
    public class SkillTimeline_PlayAnim : TimelineFunc
    {
        public string animName;

        public override void Execute(TimelineObj timelineObj)
        {
            ECSLocate.Log.Log("SkillTimeline_PlayAnim>>>>", timelineObj.ower.EntityId);

            if (timelineObj.ower == null)
                return;
            Entity entity = ECSLocate.ECS.GetEntity(timelineObj.ower.EntityId);
            if (entity == null)
            {
                return;
            }
            AnimCom animCom = entity.GetCom<AnimCom>();
            animCom.SetReqAnim(animName);
        }
    }
}
