using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCConfig
{
    [CustomInspectorDrawer(typeof(IConfig))]
    public class IConfigInspector : ObjectInspectorDrawer
    {
        private static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();

        private IConfig config;
        private ConfigAssetWindow window;

        public override void OnEnable()
        {
            base.OnEnable();
            config = Target as IConfig;
            window = Owner as ConfigAssetWindow;
        }

        public override void OnInspectorGUI()
        {
            if (!ContextDataCache.TryGetContextData<GUIStyle>("BigLabel", out var bigLabel))
            {
                bigLabel.value = new GUIStyle(GUI.skin.label);
                bigLabel.value.fontSize = 18;
                bigLabel.value.fontStyle = FontStyle.Bold;
                bigLabel.value.alignment = TextAnchor.MiddleLeft;
                bigLabel.value.stretchWidth = true;
            }
            if (window == null && config == null)
            {
                return;
            }
            GUILayoutExtension.VerticalGroup(() =>
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Script", Script, typeof(MonoScript), false);
                EditorGUI.EndDisabledGroup();

                bigLabel.value.alignment = TextAnchor.MiddleLeft;
                GUILayout.Label(string.Concat("配置项：", window.currSelAsset.name), bigLabel.value);

                GUILayoutExtension.VerticalGroup(() => {
                    foreach (var field in Fields)
                    {
                        if (AttributeHelper.TryGetFieldAttribute(field,out ConfigValueAttribute attr))
                        {
                            GUILayoutExtension.DrawField(field, Target, GUIHelper.TextContent(attr.Name, attr.Tooltip));
                        }
                        else
                        {
                            GUILayoutExtension.DrawField(field, Target);
                        }
                    }
                });
            });
        }
    }
}