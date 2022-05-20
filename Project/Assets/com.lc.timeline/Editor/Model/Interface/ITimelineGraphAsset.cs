using System;

namespace LCTimeline
{
    public interface ITimelineGraphAsset
    {
        Type GraphType { get; }

        void SaveGraph(BaseTimelineGraph graph);

        BaseTimelineGraph DeserializeGraph();
    }
}
