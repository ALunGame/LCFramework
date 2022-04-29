using LCToolkit;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCConfig
{
    [CustomEditor(typeof(ConfigAssetGroup), true)]
    public class ConfigAssetGroupInspector : Editor
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

            ConfigAssetGroup groupAsset = target as ConfigAssetGroup;

            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label($"Config:{groupAsset.name}", bigLabel.value);
            });

            GUILayoutExtension.VerticalGroup(() =>
            {
                List<Type> cnfTypes = new List<Type>();
                List<string> cnfNames = new List<string>();
                foreach (var item in ReflectionHelper.GetChildTypes<IConfig>())
                {
                    string displayName = item.Name;
                    if (AttributeHelper.TryGetTypeAttribute(item, out ConfigAttribute attr))
                        displayName = attr.DisplayName;
                    cnfNames.Add(displayName);
                    cnfTypes.Add(item);
                }
                if (groupAsset.configTypeFullName == null)
                {
                    EditorGUILayout.HelpBox("没有选择配置类！！！", MessageType.Error);
                    MiscHelper.Dropdown($"选择的配置类", cnfNames, (int a) => {
                        groupAsset.configTypeFullName = cnfTypes[a].FullName;
                        groupAsset.configTypeName = cnfTypes[a].Name;
                    },300);
                }
                else
                {
                    MiscHelper.Dropdown(groupAsset.configTypeFullName, cnfNames, (int a) => {
                        groupAsset.configTypeFullName = cnfTypes[a].FullName;
                        groupAsset.configTypeName = cnfTypes[a].Name;
                    }, 300);
                }
            });

            GUILayoutExtension.VerticalGroup(() => {
                List<ConfigAsset> assets = groupAsset.GetAllAsset();
                for (int i = 0; i < assets.Count; i++)
                {
                    DrawGraph(groupAsset, assets[i]);
                }
            });

            GUILayoutExtension.VerticalGroup(() =>
            {
                if (GUILayout.Button($"创建配置", GUILayout.Height(50)))
                {
                    MiscHelper.Input($"输入配置名：", (string name) =>
                    {
                        groupAsset.CreateAsset(name);
                    });
                }
            });
        }

        public void DrawGraph(ConfigAssetGroup groupAsset, ConfigAsset asset)
        {
            GUILayoutExtension.HorizontalGroup(() => {
                EditorGUILayout.LabelField(asset.name, GUILayout.Width(150));

                if (GUILayout.Button("打开", GUILayout.Width(50)))
                    ConfigAssetWindow.Open(asset);

                if (GUILayout.Button("删除", GUILayout.Width(50)))
                    groupAsset.RemoveAsset(asset);

                if (GUILayout.Button("重命名", GUILayout.Width(50)))
                {
                    MiscHelper.Input($"输入配置名：", (string name) =>
                    {
                        asset.name = name;
                        EditorUtility.SetDirty(asset);
                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    });
                }
            });
        }
    }
}
