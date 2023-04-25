using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(ModifierAttr))]
    public class ModifierAttrDrawer : ObjectDrawer
    {
        private GUIStyle lableStyle;
        
        public override void OnInit()
        {
            lableStyle = new GUIStyle(EditorStyles.label);
            lableStyle.alignment = TextAnchor.MiddleLeft;
            lableStyle.fontStyle = FontStyle.Bold;
            lableStyle.fontSize = 15;
        }

        public override object OnGUI(Rect _position, GUIContent _label)
        {
            ModifierAttr modifierAttr = (ModifierAttr)Target;
            
            GUILayoutExtension.VerticalGroup(() =>
            {
                EditorGUILayout.LabelField(_label,lableStyle);
                modifierAttr.attrName =
                    (AttributeName) GUILayoutExtension.DrawField(typeof(AttributeName), modifierAttr.attrName, "属性名");
                modifierAttr.attributeValueType = (AttributeValueType)EditorGUILayout.EnumPopup("属性值类型:", modifierAttr.attributeValueType);
                modifierAttr.targetType = (TargetType)EditorGUILayout.EnumPopup("目标类型:", modifierAttr.targetType);
            },EditorStylesExtension.NullStyle);
            
            return modifierAttr;
        }
        
        public override float GetHeight()
        {
            return 0;
        }
    }
}