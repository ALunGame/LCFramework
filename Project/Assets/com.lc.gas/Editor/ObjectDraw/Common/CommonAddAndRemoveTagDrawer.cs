using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(CommonAddAndRemoveTag))]
    public class CommonAddAndRemoveTagDrawer : ObjectDrawer
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
            CommonAddAndRemoveTag addAndRemoveTag = (CommonAddAndRemoveTag)Target;
            
            GUILayoutExtension.VerticalGroup(() =>
            {
                EditorGUILayout.LabelField(_label,lableStyle);
                addAndRemoveTag.addTags = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer), addAndRemoveTag.addTags, "添加的标签");
                addAndRemoveTag.addTags = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer), addAndRemoveTag.addTags, "移除的标签");
            });
            
            return addAndRemoveTag;
        }
        
        public override float GetHeight()
        {
            return 0;
        }
    }
}