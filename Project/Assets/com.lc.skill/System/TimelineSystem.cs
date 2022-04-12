﻿using LCECS.Core;
using System;
using System.Collections.Generic;

namespace LCSkill
{
    [System(InFixedUpdate = true)]
    public class TimelineSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(SkillCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            SkillCom skillCom = GetCom<SkillCom>(comList[0]);
            UpdateTimeline(skillCom);
        }

        private void UpdateTimeline(SkillCom skillCom)
        {
            IReadOnlyList<TimelineObj> timelines = skillCom.Timelines;
            if (timelines == null || timelines.Count <= 0)
                return;
            for (int i = 0; i < timelines.Count; i++)
            {
                TimelineObj timeline = timelines[i];
                //记录运行时长
                float wasTimeElapsed = timeline.timeElapsed;
                timeline.timeElapsed = wasTimeElapsed + SkillLocate.Skill.GetSkillDeltaTime() * timeline.timeScale;
                //判断跳转点
                if (timeline.model.goToNode.atDuration >= wasTimeElapsed &&
                    timeline.model.goToNode.atDuration < timeline.timeElapsed)
                {
                    timeline.timeElapsed = timeline.model.goToNode.gotoDuration;
                    continue;
                }
                //执行节点函数
                for (int j = 0; j < timeline.model.nodes.Length; j++)
                {
                    TimelineNode timelineNode = timeline.model.nodes[j];
                    if (timelineNode.timeElapsed >= wasTimeElapsed &&
                        timelineNode.timeElapsed < timeline.timeElapsed)
                    {
                        TimelineNodeFunc.ExecuteFunc(timelineNode.funcName, timeline, timelineNode.funcParam);
                    }
                }
                //判断是否结束
                if (timeline.timeElapsed >= timeline.model.duration)
                {
                    skillCom.RemoveTimeline(timeline);
                }
            }
        }
    }
}
