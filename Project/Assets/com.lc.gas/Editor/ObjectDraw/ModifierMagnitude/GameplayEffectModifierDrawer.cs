using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(GameplayEffectModifier))]
    public class GameplayEffectModifierDrawer : ObjectDrawer
    {
        private GUIStyle lableStyle;
        private float splitWidth = 10;
        protected IReadOnlyList<FieldInfo> Fields { get; private set; }

        public override void OnInit()
        {
            lableStyle = new GUIStyle(EditorStyles.label);
            lableStyle.alignment = TextAnchor.MiddleLeft;
            lableStyle.fontStyle = FontStyle.Bold;
            lableStyle.fontSize = 15;
            Fields = ReflectionHelper.GetFieldInfos(Target.GetType()).Where(field => GUILayoutExtension.CanDraw(field)).ToList();
        }

        public override object OnGUI(Rect _position, GUIContent _label)
        {
            GameplayEffectModifier modifier = Target as GameplayEffectModifier;
            
            modifier.readAttr = (ModifierAttr)GUILayoutExtension.DrawField(typeof(ModifierAttr), modifier.readAttr,"读取的属性");
            
            EditorGUILayout.Space(splitWidth);
            EditorGUILayout.LabelField("运算符",lableStyle);
            modifier.op = (ModifierOp)EditorGUILayout.EnumPopup("运算符:", modifier.op);
            
            EditorGUILayout.Space(splitWidth);
            EditorGUILayout.LabelField("运算值",lableStyle);
            modifier.magnitude = (GameplayEffectModifierMagnitude)GUILayoutExtension.DrawField(typeof(GameplayEffectModifierMagnitude), modifier.magnitude,"运算值");
            
            EditorGUILayout.Space(splitWidth);
            modifier.setAttr = (ModifierAttr)GUILayoutExtension.DrawField(typeof(ModifierAttr), modifier.setAttr,"设置的属性");
            
            EditorGUILayout.Space(splitWidth);
            modifier.sourceTags = (CommonConditionTag)GUILayoutExtension.DrawField(typeof(CommonConditionTag), modifier.sourceTags,"来源标签条件");
            modifier.tagetTags = (CommonConditionTag)GUILayoutExtension.DrawField(typeof(CommonConditionTag), modifier.tagetTags,"目标标签条件");
            
            return Target;
        }
    }
}