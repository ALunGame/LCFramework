using System;
using System.Collections.Generic;
using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomInspectorDrawer(typeof(GameplayEffect))]
    public class GameplayEffectDrawer : ObjectInspectorDrawer
    {
        private bool OpenTag = false;
        private bool OpenRate = false;
        private bool OpenPeriod = false;
        private bool OpenDuration = false;
        private bool OpenModifiers = false;
        private bool OpenStack = false;
        private bool OpenOverflow = false;
        private bool OpenExpiration = false;
        private bool OpenDisplay = false;
        private List<bool> modifierFoldOutDict = new List<bool>();

        private List<string> typeNames = new List<string>()
        {
            "立即改变",
            "永久改变",
            "持续改变",
        };

        public override void OnEnable()
        {
            modifierFoldOutDict.Clear();
        }

        public override void OnInspectorGUI()
        {
            GEAsset asset = Owner as GEAsset;
            GameplayEffect effect = (GameplayEffect)Target;
            effect.name = asset.FileName();
            
            EditorGUILayout.LabelField("效果名", effect.name);
            
            //标签
            FoldoutGroup("标签", OpenTag, () =>
            {
                OpenTag = true;
                effect.tag = (GameplayEffectTag)GUILayoutExtension.DrawField(typeof(GameplayEffectTag), effect.tag);
            }, () =>
            {
                OpenTag = false;
            });
            
            //类型
            GUILayoutExtension.HorizontalGroup(() =>
            {
                EditorGUILayout.LabelField("类型");
                MiscHelper.Dropdown(typeNames[(int)effect.type],typeNames, (int selIndex) =>
                {
                    effect.type = (GameplayEffectType)selIndex;
                });
            });
            
            //应用的概率
            FoldoutGroup("应用概率", OpenRate, () =>
            {
                OpenRate = true;
                if (effect.rate == null)
                {
                    MiscHelper.Btn("创建",100,30, () =>
                    {
                        effect.rate = new GameplayEffectActiveRate();
                    });
                }
                else
                {
                    MiscHelper.Btn("删除",100,30, () =>
                    {
                        effect.rate = null;
                    });
                }
            
                if (effect.rate != null)
                {
                    effect.rate = (GameplayEffectActiveRate)GUILayoutExtension.DrawField(typeof(GameplayEffectTag), effect.rate, "应用概率");
                }
            }, () =>
            {
                OpenRate = false;
            });
            
            //应用的周期
            if (effect.type != GameplayEffectType.Instand)
            {
                if (effect.period == null)
                    effect.period = new GameplayEffectPeriod();
                FoldoutGroup("应用周期", OpenPeriod, () =>
                {
                    OpenPeriod = true;
                    if (effect.period != null)
                    {
                        effect.period = (GameplayEffectPeriod)GUILayoutExtension.DrawField(typeof(GameplayEffectPeriod), effect.period, "应用周期");
                    }
                }, () =>
                {
                    OpenPeriod = false;
                });
            }
            else
            {
                effect.period = null;
            }
            
            //持续时间
            if (effect.type == GameplayEffectType.HasDuration)
            {
                if (effect.duration == null)
                {
                    effect.duration = new GameplayEffectModifierMagnitude();
                }
                FoldoutGroup("应用持续时间", OpenDuration, () =>
                {
                    OpenDuration = true;
                    effect.duration = (GameplayEffectModifierMagnitude)GUILayoutExtension.DrawField(typeof(GameplayEffectModifierMagnitude), effect.duration, "应用持续时间");
                }, () =>
                {
                    OpenDuration = false;
                });
            }
            else
            {
                effect.duration = null;
            }
            
            FoldoutGroup("属性改变流程", OpenModifiers, () =>
            {
                OpenModifiers = true;
                DrawModifiers(effect);
            }, () =>
            {
                OpenModifiers = false;
            });

            if (effect.type != GameplayEffectType.Instand)
            {
                FoldoutGroup("堆叠", OpenStack, () =>
                {
                    OpenStack = true;
                    if (effect.stack == null)
                    {
                        MiscHelper.Btn("创建",100,30, () =>
                        {
                            effect.stack = new GameplayEffectStack();
                        });
                    }
                    else
                    {
                        MiscHelper.Btn("删除",100,30, () =>
                        {
                            effect.stack = null;
                        });
                    }

                    if (effect.stack != null)
                    {
                        effect.stack = (GameplayEffectStack)GUILayoutExtension.DrawField(typeof(GameplayEffectStack), effect.stack);
                    }
                }, () =>
                {
                    OpenStack = false;
                });
            
                FoldoutGroup("堆叠溢出策略", OpenOverflow, () =>
                {
                    OpenOverflow = true;
                    if (effect.overflow == null)
                    {
                        MiscHelper.Btn("创建",100,30, () =>
                        {
                            effect.overflow = new GameplayEffectOverflow();
                        });
                    }
                    else
                    {
                        MiscHelper.Btn("删除",100,30, () =>
                        {
                            effect.overflow = null;
                        });
                    }

                    if (effect.overflow != null)
                    {
                        effect.overflow = (GameplayEffectOverflow)GUILayoutExtension.DrawField(typeof(GameplayEffectOverflow), effect.overflow);
                    }
                }, () =>
                {
                    OpenOverflow = false;
                });
                
                FoldoutGroup("被打断或正常结束策略", OpenExpiration, () =>
                {
                    OpenExpiration = true;
                    if (effect.expiration == null)
                    {
                        MiscHelper.Btn("创建",100,30, () =>
                        {
                            effect.expiration = new GameplayEffectExpiration();
                        });
                    }
                    else
                    {
                        MiscHelper.Btn("删除",100,30, () =>
                        {
                            effect.expiration = null;
                        });
                    }

                    if (effect.expiration != null)
                    {
                        effect.expiration = (GameplayEffectExpiration)GUILayoutExtension.DrawField(typeof(GameplayEffectExpiration), effect.expiration);
                    }
                }, () =>
                {
                    OpenExpiration = false;
                });
            }
            else
            {
                effect.stack = null;
                effect.overflow = null;
                effect.expiration = null;
            }
            
            FoldoutGroup("应用的表现", OpenDisplay, () =>
            {
                OpenDisplay = true;
                if (effect.display == null)
                {
                    MiscHelper.Btn("创建",100,30, () =>
                    {
                        effect.display = new GameplayEffectDisplay();
                    });
                }
                else
                {
                    MiscHelper.Btn("删除",100,30, () =>
                    {
                        effect.display = null;
                    });
                }

                if (effect.display != null)
                {
                    effect.display = (GameplayEffectDisplay)GUILayoutExtension.DrawField(typeof(GameplayEffectDisplay), effect.display);
                }
            }, () =>
            {
                OpenDisplay = false;
            });

            OnHandleEvent(Event.current);
        }
        
        public void OnHandleEvent(Event evt)
        {
            if (evt == null)
                return;
            //保存Ctrl+S
            if (Event.current.Equals(Event.KeyboardEvent("^S")))
            {
                SaveAsset();
                Event.current.Use();
            }
        }

        private void DrawModifiers(GameplayEffect pEffect)
        {
            if (pEffect.modifiers == null)
            {
                pEffect.modifiers = new List<GameplayEffectModifier>();
                modifierFoldOutDict = new List<bool>();
            }
            int count = pEffect.modifiers.Count;
            int newCount = EditorGUILayout.IntField(GUIHelper.TextContent("Size"), count);
            if (count != newCount)
            {
                //Add
                if (newCount > count)
                {
                    for (int i = 0; i < newCount - count; i++)
                    {
                        pEffect.modifiers.Add(new GameplayEffectModifier());
                        modifierFoldOutDict.Add(false);
                    }
                }
                else
                {
                    int delCnt = count - newCount;
                    delCnt = delCnt > count ? count : delCnt;
                    for (int i = 0; i < delCnt; i++)
                    {
                        pEffect.modifiers.RemoveAt(i);
                        modifierFoldOutDict.RemoveAt(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < pEffect.modifiers.Count; i++)
                {
                    FoldoutGroup($"Modifier:{i}", modifierFoldOutDict[i], () =>
                    {
                        modifierFoldOutDict[i] = true;
                        pEffect.modifiers[i] = (GameplayEffectModifier)GUILayoutExtension.DrawField(typeof(GameplayEffectModifier), pEffect.modifiers[i], i.ToString());
                    }, () =>
                    {
                        modifierFoldOutDict[i] = false;
                    });
                }
            }
        }
        
        private void FoldoutGroup(string label, bool foldout,Action drawFunc,Action hideDrawFunc)
        {
            GUILayoutExtension.VerticalGroup(() =>
            {
                Rect rect = GUILayoutUtility.GetRect(50, 25);
                rect = EditorGUI.IndentedRect(rect);

                Event current = Event.current;
                if (current.type == EventType.MouseDown && current.button == 0)
                {
                    if (rect.Contains(current.mousePosition))
                    {
                        foldout = !foldout;
                    }
                }
                
                GUI.Box(rect, string.Empty, GUI.skin.button);
                        
                Rect t = rect;
                t.xMin += 5;
                t.xMax -= 5;
                EditorGUI.Foldout(t, foldout, label);
                
                if (foldout)
                    drawFunc?.Invoke();
                else
                    hideDrawFunc?.Invoke();
            });
        }
        
        private void SaveAsset()
        {
            GameplayEffect effect = (GameplayEffect)Target;

            GEAsset asset = Owner as GEAsset;
            asset.Export(effect);
            
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            UpdateTarget(asset.GetAsset());
        }
    }
}