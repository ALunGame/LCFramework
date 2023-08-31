using LCNode.Model.Internal;
using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;
using MemoryPack;

namespace LCNode.Model
{
    [Serializable]
    public abstract class BaseGraphAsset<GraphClass> : InternalBaseGraphAsset where GraphClass : BaseGraph, new()
    {
        public override Type GraphType => typeof(GraphClass);

        #region Serialize

        [HideInInspector]
        [SerializeField]
        [TextArea(20, 20)]
        string serializedGraph = String.Empty;

        public override void SaveGraph(BaseGraph graph)
        {
            serializedGraph = LCJson.JsonMapper.ToJson(graph);
        }

        public override sealed BaseGraph DeserializeGraph()
        {
            var graph = LCJson.JsonMapper.ToObject<BaseGraph>(serializedGraph);
            if (graph == null)
                graph = new GraphClass();
            if (graph.nodes != null && graph.nodes.Count > 0)
            {
                foreach (BaseNode node in graph.nodes.Values)
                {
                    if (node != null)
                    {
                        node.Owner = graph;
                    }
                }
            }
            return graph;
        }

        public GraphClass DeserializeTGraph()
        {
            return DeserializeGraph() as GraphClass;
        }

        public string GetSerializedStr()
        {
            return string.Copy(serializedGraph);
        }

        public void SetSerializedStr(string str)
        {
            serializedGraph = str;
        }

        #endregion
    }
}
