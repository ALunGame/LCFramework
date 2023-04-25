using System;
using System.Collections.Generic;
using LCToolkit;
using UnityEngine;

namespace LCSkill.Timeline
{
    /// <summary>
    /// Timeline环境
    /// </summary>
    public class TimelineContext
    {

    }
    
    public class TimelineExecutor
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        protected T GetContext<T>(SkillTimelineSpec pSpec) where T : TimelineContext, new()
        {
            string uniqueKey = GetHashCode() + typeof(T).FullName;
            T thisContext;
            if (pSpec.Context.ContainsKey(uniqueKey) == false)
            {
                thisContext = new T();
                pSpec.Context.Add(uniqueKey, thisContext);
            }
            else
            {
                try
                {
                    thisContext = (T)pSpec.Context[uniqueKey];
                }
                catch (Exception e)
                {
                    thisContext = new T();
                    LCSkill.SkillLocate.Log.LogError("Timeline上下文转换失败》》》》》》", uniqueKey, typeof(T).FullName);
                }
            }
            return thisContext;
        }
        
        public virtual void Start(SkillTimelineSpec pSpec)
        {
            
        }

        public virtual void UpdateFrame(SkillTimelineSpec pSpec, int pFrame)
        {
            
        }

        public virtual void End(SkillTimelineSpec pSpec)
        {
            
        }
    }
    
    
    public class BaseTimeline : TimelineExecutor
    {
        /// <summary>
        /// Timeline资源名
        /// </summary>
        public string name = "";

        /// <summary>
        /// 总帧数
        /// </summary>
        public int totalFrame;
        
        /// <summary>
        /// 轨道组
        /// </summary>
        [HideInInspector]
        public List<BaseTrackGroup> trackGroups = new List<BaseTrackGroup>();

        public override void Start(SkillTimelineSpec pSpec)
        {
            foreach (BaseTrackGroup trackGroup in trackGroups)
            {
                trackGroup.Start(pSpec);
            }
        }

        public override void UpdateFrame(SkillTimelineSpec pSpec, int pFrame)
        {
            foreach (BaseTrackGroup trackGroup in trackGroups)
            {
                trackGroup.UpdateFrame(pSpec,pFrame);
            }
        }

        public override void End(SkillTimelineSpec pSpec)
        {
            foreach (BaseTrackGroup trackGroup in trackGroups)
            {
                trackGroup.End(pSpec);
            }
        }
    }


}