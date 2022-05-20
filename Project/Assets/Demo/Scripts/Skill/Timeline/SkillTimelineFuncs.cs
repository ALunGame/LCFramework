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
            if (timelineObj.ower == null)
                return;
            Entity entity = ECSLocate.ECS.GetEntity(timelineObj.ower.EntityId);
            if (entity == null)
            {
                ECSLocate.Log.LogError("SkillTimeline_PlayAnim>>>>", timelineObj.ower.EntityId);
                return;
            }
            AnimCom animCom = entity.GetCom<AnimCom>();
            animCom.SetReqAnim(animName);
        }
    }
}
