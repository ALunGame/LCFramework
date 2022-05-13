﻿using LCNode.Model.Internal;
using LCToolkit;
using UnityEditor;
using UnityEngine;
using static LCToolkit.GUIHelper;

namespace LCNode.Inspector
{
    [CustomEditor(typeof(GraphSetting), true)]
    public class GraphSettingInspector : Editor
    {
        static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();
        static ContextData<GUIStyle> BigLabel;

        private GraphSetting settingAsset;

        private void OnEnable()
        {
            settingAsset = (GraphSetting)target;
            foreach (var item in ReflectionHelper.GetChildTypes<InternalGraphGroupAsset>())
            {
                if (item.IsAbstract)
                    continue;
                if (settingAsset.GetSearchPath(item.FullName) == null)
                {
                    settingAsset.groupPaths.Add(new GraphGroupPath(item));
                }
            }
        }

        public override void OnInspectorGUI()
        {
            if (!ContextDataCache.TryGetContextData<GUIStyle>("BigLabel", out BigLabel))
            {
                BigLabel.value = new GUIStyle(GUI.skin.label);
                BigLabel.value.fontSize = 18;
                BigLabel.value.fontStyle = FontStyle.Bold;
                BigLabel.value.alignment = TextAnchor.MiddleLeft;
                BigLabel.value.stretchWidth = true;
            }

            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label($"视图路径设置", BigLabel.value);
            });

            foreach (var item in settingAsset.groupPaths)
            {
                DrawGraphPath(item);
            }
        }

        private void DrawGraphPath(GraphGroupPath path)
        {
            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label($"{path.typeName}", BigLabel.value);
                string newPath = MiscHelper.FolderPath("分组目录:", path.searchPath);
                if (newPath != path.searchPath)
                {
                    path.searchPath = newPath;
                    EditorUtility.SetDirty(settingAsset);
                }
            });
        }
    }
}