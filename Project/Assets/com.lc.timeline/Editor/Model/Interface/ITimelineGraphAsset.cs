using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCTimeline
{
    public interface ITimelineGraphAsset
    {
        Type GraphType { get; }

        void SaveGraph(BaseTimelineGraph graph);

        BaseTimelineGraph DeserializeGraph();
    }
}
