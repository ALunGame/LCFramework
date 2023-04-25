using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(GameplayEffectModifierMagnitude))]
    public class GameplayEffectModifierMagnitudeDrawer : ObjectDrawer
    {
        private static string typeName;
        private static string childTypeName = "";
        
        private List<string> magnitudeTypeNames = new List<string>()
        {
            "固定浮点值",
            "获得属性值",
            "自定义类值",
        };

        private List<string> customTypes = new List<string>();

        protected IReadOnlyList<FieldInfo> Fields { get; private set; }
        
        private string GetMagnitudeTypeName()
        {
            GameplayEffectModifierMagnitude magnitude = Target as GameplayEffectModifierMagnitude;
            if (typeName == magnitudeTypeNames[2])
            {
                return magnitudeTypeNames[2];
            }
            if (magnitude.magnitude == null)
            {
                return "";
            }
            if (magnitude.magnitude.GetType().Equals(typeof(GameplayEffectModifierMagnitudeFloat)))
            {
                return magnitudeTypeNames[0];
            }
            if (magnitude.magnitude.GetType().Equals(typeof(GameplayEffectModifierMagnitudeAttribute)))
            {
                return magnitudeTypeNames[1];
            }
            if (magnitude.magnitude.GetType().BaseType.Equals(typeof(ModifierMagnitudeCustom)))
            {
                return magnitudeTypeNames[2];
            }
            return magnitudeTypeNames[0];
        }

        private void SetTarget(string pTypeName)
        {
            if (GetMagnitudeTypeName() == pTypeName)
            {
                return;
            }
            GameplayEffectModifierMagnitude magnitude = Target as GameplayEffectModifierMagnitude;
            if (pTypeName == magnitudeTypeNames[0])
            {
                magnitude.magnitude = new GameplayEffectModifierMagnitudeFloat();
            }
            else if (pTypeName == magnitudeTypeNames[1])
            {
                magnitude.magnitude = new GameplayEffectModifierMagnitudeAttribute();
            }
            else if (pTypeName == magnitudeTypeNames[2])
            {
                magnitude.magnitude = null;

                //TODO:创建自定义类
            }

            if (magnitude.magnitude != null)
            {
                Fields = ReflectionHelper.GetFieldInfos(magnitude.magnitude.GetType()).Where(field => GUILayoutExtension.CanDraw(field)).ToList();
            }
            typeName = pTypeName;
        }

        public override void OnInit()
        {
            GameplayEffectModifierMagnitude magnitude = Target as GameplayEffectModifierMagnitude;
            if (magnitude.magnitude != null)
            {
                Fields = ReflectionHelper.GetFieldInfos(magnitude.magnitude.GetType()).Where(field => GUILayoutExtension.CanDraw(field)).ToList();
            }

            foreach (Type childType in ReflectionHelper.GetChildTypes<ModifierMagnitudeCustom>())
            {
                customTypes.Add(childType.FullName);
            }
            
        }

        public override object OnGUI(Rect _position, GUIContent _label)
        {
            typeName = GetMagnitudeTypeName();
            
            //类型
            GUILayoutExtension.HorizontalGroup(() =>
            {
                EditorGUILayout.LabelField("类型");
                MiscHelper.Dropdown(typeName,magnitudeTypeNames, (string selTypeName) =>
                {
                    SetTarget(selTypeName);
                });
            });
            
            typeName = GetMagnitudeTypeName();
            if (typeName == magnitudeTypeNames[0])
            {
                DrawFloatMagnitude();
            }
            else if (typeName == magnitudeTypeNames[1])
            {
                DrawAttributeMagnitude();
            }
            else if (typeName == magnitudeTypeNames[2])
            {
                DrawCustomMagnitude();
            }
            
            return Target;
        }

        private void DrawFloatMagnitude()
        {
            GameplayEffectModifierMagnitude magnitude = Target as GameplayEffectModifierMagnitude;
            foreach (var field in Fields)
            {
                GUILayoutExtension.DrawField(field, magnitude.magnitude);
            }
        }
        
        private void DrawAttributeMagnitude()
        {
            GameplayEffectModifierMagnitude magnitude = Target as GameplayEffectModifierMagnitude;
            foreach (var field in Fields)
            {
                GUILayoutExtension.DrawField(field, magnitude.magnitude);
            }
        }
        
        private void DrawCustomMagnitude()
        {
            GameplayEffectModifierMagnitude magnitude = Target as GameplayEffectModifierMagnitude;

            
            GUILayoutExtension.HorizontalGroup(() =>
            {
                EditorGUILayout.LabelField(GUIHelper.TextContent("自定义类",childTypeName));
                MiscHelper.Dropdown(childTypeName,customTypes, (string selTypeName) =>
                {
                    childTypeName = selTypeName;
                    magnitude.magnitude = (ModifierMagnitudeCustom)ReflectionHelper.CreateInstance(ReflectionHelper.GetType(childTypeName),null);
                });
            });
            
            if (magnitude.magnitude != null)
            {
                foreach (var field in Fields)
                {
                    GUILayoutExtension.DrawField(field, magnitude.magnitude);
                }
            }
        }
        
        public override float GetHeight()
        {
            return 0;
        }
    }
}