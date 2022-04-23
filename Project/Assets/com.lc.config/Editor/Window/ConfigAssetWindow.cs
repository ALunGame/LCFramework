using LCToolkit;
using LCToolkit.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LCConfig
{
    public class ConfigAssetWindow : BaseEditorWindow
    {
        #region Static
        private static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();

        public static ConfigAssetWindow Open(ConfigAsset asset)
        {
            if (asset == null) return null;
            var window = GetWindow<ConfigAssetWindow>();
            window.titleContent = new GUIContent($"配置:{asset.name}");
            window.currSelAsset = asset;
            window.ChangeSelAsset(asset);
            return window;
        }

        #endregion

        public ConfigAsset currSelAsset;
        private List<IConfig> configs;
        private Dictionary<FieldInfo, ConfigKeyAttribute> keyFields = new Dictionary<FieldInfo, ConfigKeyAttribute>();
        private Dictionary<FieldInfo, ConfigValueAttribute> valueFields = new Dictionary<FieldInfo, ConfigValueAttribute>();

        private List<IConfig> SelConfigs = new List<IConfig>();
        public CommandDispatcher CommandDispacter { get; private set; }

        public void ChangeSelAsset(ConfigAsset asset)
        {
            CommandDispacter = new CommandDispatcher();

            currSelAsset = asset;
            List<IConfig> tmpConfigs = currSelAsset.Load();
            if (tmpConfigs == null)
            {
                configs = new List<IConfig>();
                configs.Add(currSelAsset.CreateCnfItem());
                configs.Add(currSelAsset.CreateCnfItem());
                configs.Add(currSelAsset.CreateCnfItem());
                configs.Add(currSelAsset.CreateCnfItem());
            }
            else
            {
                configs = tmpConfigs;
            }

            Type cnfType = currSelAsset.GetCnfType();
            foreach (var item in ReflectionHelper.GetFieldInfos(cnfType))
            {
                if (AttributeHelper.TryGetFieldAttribute(item, out ConfigKeyAttribute keyAttr))
                {
                    keyFields.Add(item,keyAttr);
                }
                else
                {
                    if (AttributeHelper.TryGetFieldAttribute(item, out ConfigValueAttribute attr))
                    {
                        valueFields.Add(item, attr);
                    }
                    else
                    {
                        valueFields.Add(item, new ConfigValueAttribute(item.Name,""));
                    }
                }
            }
        }

        private float BtnWidth = 100;
        private float BtnHeight = 50;
        public Color SelectColor = new Color32(158, 203, 247, 255);

        private void OnGUI()
        {
            if (!ContextDataCache.TryGetContextData<GUIStyle>("BigLabel", out var bigLabel))
            {
                bigLabel.value = new GUIStyle(GUI.skin.label);
                bigLabel.value.fontSize = 18;
                bigLabel.value.fontStyle = FontStyle.Bold;
                bigLabel.value.alignment = TextAnchor.MiddleLeft;
                bigLabel.value.stretchWidth = true;
            }

            //工具栏
            GUILayoutExtension.HorizontalGroup(() =>
            {
                string selMap = currSelAsset == null ? "Null" : currSelAsset.name;
                GUILayout.Label($"当前配置:{selMap}", bigLabel.value);

                if (currSelAsset != null)
                {
                    if (GUILayout.Button("保存配置", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight)))
                    {
                        SaveAsset(currSelAsset);
                    }
                }

            }, GUILayout.Height(50));

            if (currSelAsset!=null)
            {
                GUILayoutExtension.VerticalGroup(() =>
                {
                    for (int i = 0; i < configs.Count; i++)
                    {
                        IConfig config = configs[i];
                        GUILayoutExtension.HorizontalGroup(() =>
                        {
                            GUI.color = Color.white;
                            if (IsInSel(config))
                                GUI.color = SelectColor;
                            MiscHelper.Btn("选择", 35, 35, () =>
                            {
                                OnClickSelBtn(config);
                            });
                            foreach (var fieldInfo in keyFields.Keys)
                            {
                                object value = fieldInfo.GetValue(config);
                                float height = GUIExtension.GetHeight(fieldInfo.FieldType, value, GUIHelper.TextContent(""));
                                value = GUIExtension.DrawField(EditorGUILayout.GetControlRect(true, height), value, GUIHelper.TextContent(""));
                                fieldInfo.SetValue(config, value);
                            }
                            GUI.color = Color.white;
                        });
                    }
                });
            }

            OnHandleEvent(Event.current);
        }

        public void OnHandleEvent(Event evt)
        {
            if (evt == null)
                return;
            //保存Ctrl+S
            if (Event.current.Equals(Event.KeyboardEvent("^S")))
            {
                SaveAsset(currSelAsset);
            }
            //撤销Ctrl+Z
            if (Event.current.Equals(Event.KeyboardEvent("^Z")))
            {
                CommandDispacter.Undo();
            }
            //回退Ctrl+Y
            if (Event.current.Equals(Event.KeyboardEvent("^Y")))
            {
                CommandDispacter.Redo();
            }
            //回退Ctrl+D 复制
            if (Event.current.Equals(Event.KeyboardEvent("^D")))
            {
                screenshot.enabled = true;
            }
            //删除Del
            if (Event.current.Equals(Event.KeyboardEvent("Delete")))
            {
                screenshot.enabled = true;
            }
        }

        private bool IsInSel(IConfig config)
        {
            for (int i = 0; i < SelConfigs.Count; i++)
            {
                if (SelConfigs[i].Equals(config))
                {
                    return true;
                }
            }
            return false;
        }

        private void OnClickSelBtn(IConfig config)
        {
            for (int i = 0; i < SelConfigs.Count; i++)
            {
                if (SelConfigs[i].Equals(config))
                {
                    SelConfigs.RemoveAt(i);
                    return;
                }
            }
            SelConfigs.Add(config);
        }

        private void SaveAsset(ConfigAsset asset)
        {
            asset.Save(configs);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    } 
}
