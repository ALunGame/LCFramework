using LCToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LCTimeline
{
    public abstract class InternalTimelineGraphAsset : ScriptableObject, ITimelineGraphAsset
    {
        public abstract GameObject DisplayGo { get; }
        public abstract Type GraphType { get; }
        public abstract void SaveGraph(BaseTimelineGraph graph);
        public abstract BaseTimelineGraph DeserializeGraph();
    }
}
