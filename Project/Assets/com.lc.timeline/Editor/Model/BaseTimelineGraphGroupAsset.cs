using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;
using LCToolkit;

namespace LCTimeline
{
    [Serializable]
    public abstract class BaseTimelineGraphGroupAsset<T> : InternalTimelineGraphGroupAsset where T : InternalTimelineGraphAsset
    {
        public override List<InternalTimelineGraphAsset> GetAllGraph()
        {
            List<InternalTimelineGraphAsset> graphs = new List<InternalTimelineGraphAsset>();
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(this));
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] is T)
                {
                    graphs.Add((T)objs[i]);
                }
            }
            return graphs;
        }

        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入{DisplayName}名：", (string name) =>
            {
                CreateGraph(name);
            });
        }

        public override bool CheckHasGraph(string name)
        {
            List<InternalTimelineGraphAsset> graphs = GetAllGraph();
            for (int i = 0; i < graphs.Count; i++)
            {
                if (graphs[i].name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public override InternalTimelineGraphAsset CreateGraph(string name)
        {
            if (CheckHasGraph(name))
            {
                Debug.LogError($"创建视图失败，重复视图>>{name}");
                return null;
            }

            T graph = CreateInstance<T>();
            graph.name = name;
            AssetDatabase.AddObjectToAsset(graph, this);
            EditorUtility.SetDirty(graph);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return graph;
        }

        public override void RemoveGraph(InternalTimelineGraphAsset graph)
        {
            DestroyImmediate(graph, true);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}