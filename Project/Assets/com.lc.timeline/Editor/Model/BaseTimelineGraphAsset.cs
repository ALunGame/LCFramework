using LCToolkit;
using System;
using UnityEngine;

namespace LCTimeline
{
    [Serializable]
    public abstract class BaseTimelineGraphAsset<GraphClass> : InternalTimelineGraphAsset where GraphClass : BaseTimelineGraph, new()
    {
        public override Type GraphType => typeof(GraphClass);

        public override GameObject DisplayGo => Go;

        #region Serialize

        [Header("编辑器下辅助显示")]
        public GameObject Go;

        [HideInInspector]
        [SerializeField]
        [TextArea(20, 20)]
        string serializedGraph = String.Empty;

        public override void SaveGraph(BaseTimelineGraph graph)
        {
            serializedGraph = LCJson.JsonMapper.ToJson(graph);
        }

        public override sealed BaseTimelineGraph DeserializeGraph()
        {
            var graph = LCJson.JsonMapper.ToObject<BaseTimelineGraph>(serializedGraph);
            if (graph == null)
                graph = new GraphClass();
            graph.Enable();
            return graph;
        }

        public GraphClass DeserializeTGraph()
        {
            return DeserializeGraph() as GraphClass;
        }
        #endregion
    }
}
