using LCToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace LCToolkit.Inspector
{
    [CustomEditor(typeof(InternalGroupAsset), true)]
    internal class GroupAssetInspector : Editor
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

            InternalGroupAsset groupAsset = target as InternalGroupAsset;

            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label($"{groupAsset.DisplayName}:{groupAsset.name}", bigLabel.value);
            });

            GUILayoutExtension.VerticalGroup(() => {
                List<GroupChildAsset> assets = groupAsset.GetAllAssets();
                for (int i = 0; i < assets.Count; i++)
                {
                    DrawChildAsset(groupAsset, assets[i]);
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

        public void DrawChildAsset(InternalGroupAsset groupAsset, GroupChildAsset childAsset)
        {
            GUILayoutExtension.HorizontalGroup(() => {
                EditorGUILayout.LabelField(childAsset.FileName(groupAsset), GUILayout.Width(150));

                if (GUILayout.Button("打开", GUILayout.Width(50)))
                    groupAsset.OpenChildAsset(childAsset);

                if (GUILayout.Button("删除", GUILayout.Width(50)))
                    groupAsset.RemoveChildAsset(childAsset.name);

                if (GUILayout.Button("重命名", GUILayout.Width(50)))
                {
                    MiscHelper.Input($"输入{groupAsset.DisplayName}名：", (string name) =>
                    {
                        if (groupAsset.CheckHasAsset(name))
                        {
                            MiscHelper.Dialog("重命名失败","名字重复："+name);
                            return;
                        }
                        childAsset.name = name;
                        EditorUtility.SetDirty(childAsset);
                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    });
                }
            });
        }
    }
}