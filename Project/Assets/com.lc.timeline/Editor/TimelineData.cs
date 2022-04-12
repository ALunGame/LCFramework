using System.Collections.Generic;
using UnityEngine;
using LCToolkit;

namespace LCTimeline
{
    public class TimelineData
    {
        /// <summary>
        /// Timeline资源名
        /// </summary>
        public string Name;

        /// <summary>
        /// Timeline持续时间
        /// </summary>
        public float DurationTime;

        [HideInInspector]
        public List<TrackData> Tracks = new List<TrackData>();
    }

    public class TrackData
    {
        [HideInInspector]
        public List<ClipData> Clips = new List<ClipData>();
    }

    public class ClipData
    {
        public float StartTime;
        public float EndTime;
        public float DurationTime;
    }
}