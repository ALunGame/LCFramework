using System;

namespace LCSkill.Timeline
{
    public class BaseClip : TimelineExecutor
    {
        public string name = "Clip";
        public int startFrame;
        public int endFrame;

        /// <summary>
        /// 持续多少帧
        /// </summary>
        public int DurationFrame
        {
            get
            {
                return endFrame - startFrame + 1;
            }
        }

        public override string ToString()
        {
            return $"Name: {name} Start:{startFrame} End:{endFrame} Dur:{DurationFrame}";
        }

        public sealed override void Start(SkillTimelineSpec pSpec)
        {
            OnStart(pSpec);
        }

        public sealed override void UpdateFrame(SkillTimelineSpec pSpec, int pFrame)
        {
            if (pFrame == startFrame)
            {
                OnEnter(pSpec);
            }

            if (pFrame >= startFrame && pFrame <= endFrame)
            {
                OnStay(pSpec);
            }

            if (pFrame == endFrame)
            {
                OnExit(pSpec);
            }
        }

        public sealed override void End(SkillTimelineSpec pSpec)
        {
            OnEnd(pSpec);
        }


        #region 子类覆盖

        /// <summary>
        /// 整个Timeline开始
        /// </summary>
        public virtual void OnStart(SkillTimelineSpec pSpec)
        {
            
        }
        
        /// <summary>
        /// 当进入片段
        /// </summary>
        public virtual void OnEnter(SkillTimelineSpec pSpec)
        {
            
        }
        
        /// <summary>
        /// 当在片段内
        /// </summary>
        public virtual void OnStay(SkillTimelineSpec pSpec)
        {
            
        }

        /// <summary>
        /// 当离开片段
        /// </summary>
        public virtual void OnExit(SkillTimelineSpec pSpec)
        {
            
        }
        
        /// <summary>
        /// 整个Timeline结束
        /// </summary>
        public virtual void OnEnd(SkillTimelineSpec pSpec)
        {
            
        }
        

        

        #endregion
    }
}