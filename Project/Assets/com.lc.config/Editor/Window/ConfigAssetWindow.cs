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
            window.titleContent = new GUIContent($"����:{asset.name}");
            window.currSelAsset = asset;
            window.ChangeSelAsset(asset);
            return window;
        }

        #endregion

        public ConfigAsset currSelAsset;
        private List<IConfig> configs = new List<IConfig>();
        private Dictionary<FieldInfo, ConfigValueAttribute> fields = new Dictionary<FieldInfo, ConfigValueAttribute>();
        private Dictionary<FieldInfo, Rect> fieldRects = new Dictionary<FieldInfo, Rect>();

        private List<IConfig> SelConfigs = new List<IConfig>();
        public CommandDispatcher CommandDispacter { get; private set; }

        public void ChangeSelAsset(ConfigAsset asset)
        {
            configs.Clear();
            fields.Clear();
            fieldRects.Clear();
            SelConfigs.Clear();
            CommandDispacter = new CommandDispatcher();

            currSelAsset = asset;
            List<IConfig> tmpConfigs = currSelAsset.Load();
            if (tmpConfigs == null)
            {
                configs = new List<IConfig>();
                configs.Add(currSelAsset.CreateCnfItem());
            }
            else
            {
                configs = tmpConfigs;
            }

            GetFields();
        }

        private void GetFields()
        {
            Type cnfType = currSelAsset.GetCnfType();
            List<FieldInfo> tmpKeyFields = new List<FieldInfo>();
            List<FieldInfo> tmpValueFields = new List<FieldInfo>();
            foreach (var item in ReflectionHelper.GetFieldInfos(cnfType))
            {
                if (AttributeHelper.TryGetFieldAttribute(item, out ConfigKeyAttribute keyAttr))
                {
                    tmpKeyFields.Add(item);
                }
                else
                {
                    tmpValueFields.Add(item);
                }
            }
            tmpKeyFields.Sort((x, y) =>
            {
                AttributeHelper.TryGetFieldAttribute(x, out ConfigKeyAttribute xKeyAttr);
                AttributeHelper.TryGetFieldAttribute(y, out ConfigKeyAttribute yKeyAttr);

                if (xKeyAttr.keyIndex == yKeyAttr.keyIndex)
                    return 1;
                else if (xKeyAttr.keyIndex > yKeyAttr.keyIndex)
                    return 1;
                else if (xKeyAttr.keyIndex < yKeyAttr.keyIndex)
                    return -1;
                else
                    return 1;
            });

            for (int i = 0; i < tmpKeyFields.Count; i++)
            {
                AttributeHelper.TryGetFieldAttribute(tmpKeyFields[i], out ConfigKeyAttribute keyAttr);
                fields.Add(tmpKeyFields[i], keyAttr);
                fieldRects.Add(tmpKeyFields[i], GetFieldRect(tmpKeyFields[i]));
            }
            for (int i = 0; i < tmpValueFields.Count; i++)
            {
                if (AttributeHelper.TryGetFieldAttribute(tmpValueFields[i], out ConfigValueAttribute valueAttr))
                {
                    fields.Add(tmpValueFields[i], valueAttr);
                    fieldRects.Add(tmpValueFields[i], GetFieldRect(tmpValueFields[i]));
                }
                else
                {
                    fields.Add(tmpValueFields[i], new ConfigValueAttribute(tmpValueFields[i].Name));
                    fieldRects.Add(tmpValueFields[i], GetFieldRect(tmpValueFields[i]));
                }
            }
        }

        private Rect GetFieldRect(FieldInfo field)
        {
            float height = 0;
            if (typeof(IList).IsAssignableFrom(field.FieldType))
            {
                height = GUIExtension.GetHeight(typeof(string), GUIHelper.TextContent(""));
            }
            else
            {
                height = GUIExtension.GetHeight(field.FieldType, GUIHelper.TextContent(""));
            }
            return EditorGUILayout.GetControlRect(true, height);
        }

        private float BtnWidth = 100;
        private float BtnHeight = 50;
        private Vector2 ScrollPos = Vector2.zero;
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

            //������
            GUILayoutExtension.HorizontalGroup(() =>
            {
                string selMap = currSelAsset == null ? "Null" : currSelAsset.name;
                GUILayout.Label($"��ǰ����:{selMap}", bigLabel.value);

                if (currSelAsset != null)
                {
                    if (GUILayout.Button("��������", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight)))
                    {
                        SaveAsset(currSelAsset);
                    }
                }

            }, GUILayout.Height(50));

            if (currSelAsset!=null)
            {
                GUILayoutExtension.ScrollView(ref ScrollPos, () =>
                {
                    GUILayoutExtension.HorizontalGroup(() =>
                    {
                        EditorGUILayout.Space(35);
                        //�ֶ�
                        foreach (var fieldInfo in fields.Keys)
                        {
                            ConfigValueAttribute attr = fields[fieldInfo];
                            Rect rect = GetFieldRect(fieldInfo);
                            EditorGUI.LabelField(rect,GUIHelper.TextContent(attr.Name, attr.Tooltip), bigLabel.value);
                        }
                    });

                    for (int i = 0; i < configs.Count; i++)
                    {
                        IConfig config = configs[i];
                        GUILayoutExtension.HorizontalGroup(() =>
                        {
                            GUI.color = Color.white;
                            if (IsInSel(config))
                                GUI.color = SelectColor;
                            MiscHelper.Btn("ѡ��", 35, 35, () =>
                            {
                                OnClickSelBtn(config);
                            });
                            foreach (var fieldInfo in fields.Keys)
                            {
                                object value = fieldInfo.GetValue(config);
                                if (typeof(IList).IsAssignableFrom(fieldInfo.FieldType))
                                {
                                    if (GUI.Button(GetFieldRect(fieldInfo), $"{fieldInfo.Name}�б�"))
                                    {
                                        InspectorExtension.DrawObjectInInspector($"{fieldInfo.Name}�б�", value);
                                    }
                                }
                                else
                                {
                                    object newValue = GUIExtension.DrawField(GetFieldRect(fieldInfo), value, GUIHelper.TextContent(""));
                                    if (newValue == null || !newValue.Equals(value))
                                    {
                                        CommandDispacter.Do(new ChangeValueCommand(config, fieldInfo, newValue));
                                    }
                                }

                            }
                            GUI.color = Color.white;
                        });
                    }
                });
            }
            OnHandleEvent(Event.current);
        }

        private void OnDestroy()
        {
            Selection.activeObject = null;
        }

        public void OnHandleEvent(Event evt)
        {
            if (evt == null)
                return;
            //����Ctrl+S
            if (Event.current.Equals(Event.KeyboardEvent("^S")))
            {
                SaveAsset(currSelAsset);
            }
            //����Ctrl+Z
            if (Event.current.Equals(Event.KeyboardEvent("^Z")))
            {
                CommandDispacter.Undo();
            }
            //����Ctrl+Y
            if (Event.current.Equals(Event.KeyboardEvent("^Y")))
            {
                CommandDispacter.Redo();
            }
            //����Ctrl+D ����
            if (Event.current.Equals(Event.KeyboardEvent("^D")))
            {
                CopyConfigs();
            }
            //ɾ��Del
            if (Event.current.Equals(Event.KeyboardEvent("Delete")))
            {
                DelConfigs();
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
            InspectorExtension.DrawObjectInInspector(config,this);
        }

        private void CopyConfigs()
        {
            CommandDispacter.BeginGroup();
            for (int i = 0; i < SelConfigs.Count; i++)
            {
                IConfig tmpComfig = SelConfigs[i];
                IConfig newConfig = tmpComfig.Clone();
                CommandDispacter.Do(new AddConfigCommand(configs, newConfig));
            }
            CommandDispacter.EndGroup();
        }

        private void DelConfigs()
        {
            CommandDispacter.BeginGroup();
            for (int i = 0; i < SelConfigs.Count; i++)
            {
                IConfig tmpComfig = SelConfigs[i];
                CommandDispacter.Do(new RemoveConfigCommand(configs, tmpComfig));
            }
            SelConfigs.Clear();
            CommandDispacter.EndGroup();
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
