using Demo.Com;
using Demo.System;
using LCECS;
using LCECS.Core;
using LCSkill;
using UnityEngine;
using Demo;
using LCToolkit;
using System;

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
        [NonSerialized]
        public const string ParamName = "SkillTimeline_Move";
        public Vector2 movePos;

        public override void Enter(TimelineObj timelineObj)
        {
            if (timelineObj.ower == null)
                return;
            Entity entity = ECSLocate.ECS.GetEntity(timelineObj.ower.EntityUid);
            if (entity == null)
                return;
            TransCom transformCom = entity.GetCom<TransCom>();
            Vector3 targetPos = transformCom.Pos + movePos.ToVector3();
            if (!timelineObj.timelineParam.ContainsKey(ParamName))
                timelineObj.timelineParam.Add(ParamName, targetPos);
            timelineObj.timelineParam[ParamName] = targetPos;
        }

        public override void Tick(TimelineObj timelineObj)
        {
            if (timelineObj.ower == null)
                return;
            Entity entity = ECSLocate.ECS.GetEntity(timelineObj.ower.EntityUid);
            if (entity == null)
                return;
            TransCom transformCom = entity.GetCom<TransCom>();
            BasePropertyCom propertyCom = entity.GetCom<BasePropertyCom>();
            Vector3 targetPos = (Vector3)timelineObj.timelineParam[ParamName];
            transformCom.MoveTowards(targetPos, propertyCom.MoveSpeed.Curr);
        }

        public override void Exit(TimelineObj timelineObj)
        {
        }
    }
}
