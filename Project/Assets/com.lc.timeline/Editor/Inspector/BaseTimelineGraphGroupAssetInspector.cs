using LCToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace LCTimeline.Inspector
{
    [CustomEditor(typeof(InternalTimelineGraphGroupAsset), true)]
    public class BaseTimelineGraphGroupAssetInspector : Editor
    {
        static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!ContextDataCache.TryGetContextData<GUIStyle>("BigLabel", out var bigLabel))
            {
                bigLabel.value = new GUIStyle(GUI.skin.label);
                bigLabel.value.fontSize = 18;
                bigLabel.value.fontStyle = FontStyle.Bold;
                bigLabel.value.alignment = TextAnchor.MiddleLeft;
                bigLabel.value.stretchWidth = true;
            }

            InternalTimelineGraphGroupAsset groupAsset = target as InternalTimelineGraphGroupAsset;

            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label($"{groupAsset.DisplayName}:{groupAsset.name}", bigLabel.value);
            });

            GUILayoutExtension.VerticalGroup(() => {
                List<InternalTimelineGraphAsset> graphs = groupAsset.GetAllGraph();
                for (int i = 0; i < graphs.Count; i++)
                {
                    DrawGraph(groupAsset, graphs[i]);
                }
            });

            GUILayoutExtension.VerticalGroup(() =>
            {
                if (GUILayout.Button($"创建{groupAsset.DisplayName}", GUILayout.Height(50)))
                {
                    groupAsset.OnClickCreateBtn();
                }

                if (GUILayout.Button($"导出{groupAsset.DisplayName}配置", GUILayout.Height(50)))
                {
                    groupAsset.OnClickExport();
                }
            });
        }

        public void DrawGraph(InternalTimelineGraphGroupAsset groupAsset, InternalTimelineGraphAsset graphAsset)
        {
            GUILayoutExtension.HorizontalGroup(() => {
                EditorGUILayout.LabelField(graphAsset.name, GUILayout.Width(150));

                if (GUILayout.Button("打开", GUILayout.Width(50)))
                    TimelineWindow.Open(graphAsset);

                if (GUILayout.Button("删除", GUILayout.Width(50)))
                    groupAsset.RemoveGraph(graphAsset);

                if (GUILayout.Button("重命名", GUILayout.Width(50)))
                {
                    MiscHelper.Input($"输入{groupAsset.DisplayName}名：", (string name) =>
                    {
                        graphAsset.name = name;
                        EditorUtility.SetDirty(graphAsset);
                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    });
                }
            });
        }
    }
}
