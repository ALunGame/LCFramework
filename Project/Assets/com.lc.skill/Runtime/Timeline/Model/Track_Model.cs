using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill.Timeline
{
    public class BaseTrackGroup : TimelineExecutor
    {
        [HideInInspector]
        public List<BaseTrack> tracks = new List<BaseTrack>();
        
        /// <summary>
        /// Timeline开始
        /// </summary>
        /// <param name="pSpec"></param>
        public override void Start(SkillTimelineSpec pSpec)
        {
            foreach (BaseTrack track in tracks)
            {
                track.Start(pSpec);
            }
        }

        /// <summary>
        /// 更新帧
        /// </summary>
        /// <param name="pSpec"></param>
        /// <param name="pFrame"></param>
        public override void UpdateFrame(SkillTimelineSpec pSpec, int pFrame)
        {
            foreach (BaseTrack track in tracks)
            {
                track.UpdateFrame(pSpec,pFrame);
            }
        }

        /// <summary>
        /// Timeline结束
        /// </summary>
        /// <param name="pSpec"></param>
        public override void End(SkillTimelineSpec pSpec)
        {
            foreach (BaseTrack track in tracks)
            {
                track.End(pSpec);
            }
        }
    }
    
    public class BaseTrack : TimelineExecutor
    {
        [HideInInspector] public string trackName;
        
        [HideInInspector] public List<BaseClip> clips = new List<BaseClip>();
        
        
        public override void Start(SkillTimelineSpec pSpec)
        {
            foreach (BaseClip clip in clips)
            {
                clip.Start(pSpec);
            }
        }

        public override void UpdateFrame(SkillTimelineSpec pSpec, int pFrame)
        {
            foreach (BaseClip clip in clips)
            {
                clip.UpdateFrame(pSpec,pFrame);
            }
        }

        public override void End(SkillTimelineSpec pSpec)
        {
            foreach (BaseClip clip in clips)
            {
                clip.End(pSpec);
            }
        }
    }
}