﻿using LCNode.Model.Internal;
using LCNode.View;
using LCToolkit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCECS.EntityGraph
{
    [CustomEditor(typeof(EntityGraphGroupAsset), true)]
    public class EntityGraphGroupAssetInspector : Editor
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

            EntityGraphGroupAsset groupAsset = target as EntityGraphGroupAsset;

            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label($"{groupAsset.DisplayName}:{groupAsset.name}", bigLabel.value);
            });

            DrawTemplateGraph(groupAsset);

            GUILayoutExtension.VerticalGroup(() => {
                List<InternalBaseGraphAsset> graphs = groupAsset.GetAllGraph();
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

        public void DrawTemplateGraph(EntityGraphGroupAsset groupAsset)
        {
            if (groupAsset.entity_template == null)
            {
                if (GUILayout.Button("创建实体模板", GUILayout.Height(50)))
                {
                    groupAsset.entity_template = CreateInstance<EntityGraphAsset>();
                    EditorUtility.SetDirty(groupAsset.entity_template);
                    EditorUtility.SetDirty(groupAsset);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
            else
            {
                GUILayoutExtension.HorizontalGroup(() => {
                    EditorGUILayout.LabelField("实体模板", GUILayout.Width(150));

                    if (GUILayout.Button("打开", GUILayout.Width(50)))
                        BaseGraphWindow.Open(groupAsset.entity_template);

                    if (GUILayout.Button("删除", GUILayout.Width(50)))
                    {
                        DestroyImmediate(groupAsset.entity_template, true);
                        groupAsset.entity_template = null;
                        EditorUtility.SetDirty(groupAsset);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                });
            }
        }

        public void DrawGraph(EntityGraphGroupAsset groupAsset, InternalBaseGraphAsset graphAsset)
        {
            GUILayoutExtension.HorizontalGroup(() => {
                EditorGUILayout.LabelField(graphAsset.name, GUILayout.Width(150));

                if (GUILayout.Button("打开", GUILayout.Width(50)))
                    BaseGraphWindow.Open(graphAsset);

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
