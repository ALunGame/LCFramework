using LCECS.Core;
using System;
using System.Collections.Generic;

namespace LCSkill
{
    public class TimelineSystem : BaseSystem
    {
        protected override List<Type> RegContainListenComs()
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
            TimelineObj timeline = skillCom.Timeline;
            if (timeline == null)
                return;
            //记录运行时长
            float wasTimeElapsed = timeline.timeElapsed;
            timeline.timeElapsed = wasTimeElapsed + SkillLocate.DeltaTime * timeline.timeScale;

            //已经完成
            if (timeline.isFinish)
            {
                skillCom.RemoveTimeline(timeline);
                return;
            }

            //判断跳转点
            if (timeline.model.goToNode != null)
            {
                if (timeline.model.goToNode.atDuration >= wasTimeElapsed &&
                    timeline.model.goToNode.atDuration < timeline.timeElapsed)
                {
                    timeline.timeElapsed = timeline.model.goToNode.gotoDuration;
                    return;
                }
            }

            //执行节点函数
            for (int j = 0; j < timeline.model.nodes.Count; j++)
            {
                TimelineFunc timelineFunc = timeline.model.nodes[j];
                if (timelineFunc.timeStart >= wasTimeElapsed &&
                    timelineFunc.timeStart < timeline.timeElapsed)
                {
                    timelineFunc.Enter(timeline);
                }
                timelineFunc.Tick(timeline);
                float endTime = timelineFunc.timeStart + timelineFunc.timeContinue;
                if (endTime >= wasTimeElapsed &&
                    endTime < timeline.timeElapsed)
                {
                    timelineFunc.Exit(timeline);
                }
            }

            //判断是否结束
            if (timeline.timeElapsed >= timeline.model.duration)
            {
                timeline.isFinish = true;
                skillCom.RemoveTimeline(timeline);
            }
        }
    }
}
