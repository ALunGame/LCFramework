using LCToolkit;
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
            window.titleContent = new GUIContent($"ÅäÖÃ:{asset.name}");
            window.currSelAsset = asset;
            window.ChangeSelAsset(asset);
            return window;
        }

        #endregion

        public ConfigAsset currSelAsset;
        private List<IConfig> configs;
        private Dictionary<FieldInfo, ConfigKeyAttribute> keyFields = new Dictionary<FieldInfo, ConfigKeyAttribute>();
        private Dictionary<FieldInfo, ConfigValueAttribute> valueFields = new Dictionary<FieldInfo, ConfigValueAttribute>();

        public void ChangeSelAsset(ConfigAsset asset)
        {
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
        private List<Rect> BtnRect = new List<Rect>();
        private IConfig CurrSelConfig = null;
        public Color UnSelectColor = new Color32(109, 140, 171, 255);
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

            //¹¤¾ßÀ¸
            GUILayoutExtension.HorizontalGroup(() =>
            {
                string selMap = currSelAsset == null ? "Null" : currSelAsset.name;
                GUILayout.Label($"µ±Ç°ÅäÖÃ:{selMap}", bigLabel.value);

                if (currSelAsset != null)
                {
                    if (GUILayout.Button("±£´æÅäÖÃ", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight)))
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
                            Color rectColor = UnSelectColor;
                            if (CurrSelConfig != null && CurrSelConfig.Equals(config))
                                rectColor = SelectColor;
                            
                            Rect rect = EditorGUILayout.GetControlRect(false, 35);
                            EditorGUI.DrawRect(rect, rectColor);
                            OnSelConfigEvent(rect, config);

                            MiscHelper.Btn("Ñ¡Ôñ", 35, 35, () =>
                            {
                                CurrSelConfig = config;

                            });
                            foreach (var fieldInfo in keyFields.Keys)
                            {
                                object value = fieldInfo.GetValue(config);
                                float height = GUIExtension.GetHeight(fieldInfo.FieldType, value, GUIHelper.TextContent(""));
                                value = GUIExtension.DrawField(EditorGUILayout.GetControlRect(true, height), value, GUIHelper.TextContent(""));
                                fieldInfo.SetValue(config, value);
                            }
                        });
                    }
                });
            }

            //OnHandleEvent(Event.current);
        }

        public void OnHandleEvent(Event evt)
        {
            if (evt == null)
                return;
            //Vector2 mousePos = evt.mousePosition;
            //switch (evt.type)
            //{
            //    case EventType.MouseDown:
            //        //×ó¼ü
            //        if (Event.current.button == 0)
            //        {
            //            CurrSelConfig = GetSelConfig(mousePos);
            //        }
            //        //ÓÒ¼ü
            //        if (Event.current.button == 1)
            //        {

            //        }
            //        break;
            //    default:
            //        break;
            //}
        }

        public void OnSelConfigEvent(Rect rect, IConfig config)
        {
            if (Event.current == null)
                return;
            Vector2 mousePos = Event.current.mousePosition;
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    if (rect.Contains(mousePos))
                    {
                        //×ó¼ü
                        if (Event.current.button == 0)
                        {
                            CurrSelConfig = config;
                        }
                        //ÓÒ¼ü
                        if (Event.current.button == 1)
                        {

                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private IConfig GetSelConfig(Vector2 mousePos)
        {
            for (int i = 0; i < BtnRect.Count; i++)
            {
                if (BtnRect[i].Contains(mousePos))
                {
                    return configs[i];
                }
            }
            return null;
        }

        private void DrawTabView(FieldInfo fieldInfo,string displayName,string tipName)
        {
            GUILayoutExtension.HorizontalGroup(() =>
            {
                for (int i = 0; i < configs.Count; i++)
                {
                    IConfig config = configs[i];
                    object value = fieldInfo.GetValue(config);
                    float height = GUIExtension.GetHeight(fieldInfo.FieldType, value, GUIHelper.TextContent(""));
                    value = GUIExtension.DrawField(EditorGUILayout.GetControlRect(true, height), value, GUIHelper.TextContent(""));
                    fieldInfo.SetValue(config, value);
                }
            });
            

            ////Ò»ÁÐ
            //GUILayoutExtension.VerticalGroup(() =>
            //{
            //    for (int i = 0; i < configs.Count; i++)
            //    {
            //        IConfig config = configs[i];
            //        object value = fieldInfo.GetValue(config);
            //        float height = GUIExtension.GetHeight(fieldInfo.FieldType, value, GUIHelper.TextContent(""));
            //        value = GUIExtension.DrawField(EditorGUILayout.GetControlRect(true, height), value, GUIHelper.TextContent(""));
            //        fieldInfo.SetValue(config, value);
            //    }
            //});
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
