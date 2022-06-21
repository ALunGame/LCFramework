using Demo.Com;
using Demo.System;
using LCECS;
using LCECS.Core;
using LCSkill;
using UnityEngine;

namespace Demo.Skill.Timeline
{
    /// <summary>
    /// 播放动画
    /// </summary>
    public class SkillTimeline_PlayAnim : TimelineFunc
    {
        public string animName;

        public override void Enter(TimelineObj timelineObj)
        {
            if (timelineObj.ower == null)
                return;
            Entity entity = ECSLocate.ECS.GetEntity(timelineObj.ower.EntityUid);
            if (entity == null)
            {
                return;
            }
            AnimCom animCom = entity.GetCom<AnimCom>();
            animCom.SetReqAnim(animName);
        }

        public override void Exit(TimelineObj timelineObj)
        {
            if (timelineObj.ower == null)
                return;
            Entity entity = ECSLocate.ECS.GetEntity(timelineObj.ower.EntityUid);
            if (entity == null)
            {
                return;
            }
            AnimCom animCom = entity.GetCom<AnimCom>();
            animCom.SetReqAnim(AnimSystem.IdleState);
        }
    }

    /// <summary>
    /// 移动
    /// </summary>
    public class SkillTimeline_Move : TimelineFunc
    {
        public Vector2 movePos;

        public override void Enter(TimelineObj timelineObj)
        {            
        }

        public override void Tick(TimelineObj timelineObj)
        {
            if (timelineObj.ower == null)
                return;
            Entity entity = ECSLocate.ECS.GetEntity(timelineObj.ower.EntityUid);
            if (entity == null)
                return;
            TransformCom transformCom = entity.GetCom<TransformCom>();
            float xValue = (transformCom.CurrDir == DirType.Right ? 1 : -1) * movePos.x;
            Vector3 moveDelta = new Vector3(xValue, movePos.y, 0) * Time.deltaTime;
            entity.MovePos(moveDelta);
        }

        public override void Exit(TimelineObj timelineObj)
        {
        }
    }
}
