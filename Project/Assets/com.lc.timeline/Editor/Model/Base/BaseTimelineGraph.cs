using LCToolkit;
using SkillSystem.ED.Timeline;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCTimeline
{
    public class BaseTimelineGraph
    {
        /// <summary>
        /// Timeline资源名
        /// </summary>
        public string Name = "";

        /// <summary>
        /// Timeline持续时间
        /// </summary>
        public float DurationTime;

        /// <summary>
        /// 节点
        /// </summary>
        [HideInInspector]
        [NonSerialized]
        public GameObject Go;

        [HideInInspector]
        public List<TrackModel> Tracks = new List<TrackModel>();

        public void Enable()
        {
            OnEnabled();
        }

        #region Overrides

        protected virtual void OnEnabled() { }

        public virtual IEnumerable<Type> GetTracks()
        {
            foreach (var type in ReflectionHelper.GetChildTypes<TLSK_TrackData>())
            {
                if (type.IsAbstract)
                    continue;
                if (!AttributeHelper.TryGetTypeAttribute(type, out TrackMenuAttribute trackAttribute))
                    continue;
                yield return type;
            }
        }

        #endregion
    }

    public abstract class TrackModel
    {
        public abstract Type ClipType { get; }

        public virtual string TitleName { get => "Track"; }

        [HideInInspector]
        public List<ClipModel> Clips = new List<ClipModel>();

    }

    public class ClipModel
    {
        public virtual string TitleName { get => "Clip"; }

        public float StartTime;
        public float EndTime;
        public float DurationTime;
    }
}
