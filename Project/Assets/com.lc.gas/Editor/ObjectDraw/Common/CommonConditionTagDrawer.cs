using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(CommonConditionTag))]
    public class CommonConditionTagDrawer : ObjectDrawer
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
            CommonConditionTag conditionTag = (CommonConditionTag)Target;
            
            GUILayoutExtension.VerticalGroup(() =>
            {
                EditorGUILayout.LabelField(_label,lableStyle);
                conditionTag.requireTags = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer), conditionTag.requireTags, "需要的标签");
                conditionTag.ignoreTags = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer), conditionTag.ignoreTags, "禁止的标签");
            });
            

            return conditionTag;
        }
        
        public override float GetHeight()
        {
            return 0;
        }
    }
}