using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(GameplayEffectTag))]
    public class GameplayEffectTagDrawer : ObjectDrawer
    {

        public override object OnGUI(Rect _position, GUIContent _label)
        {
            GameplayEffectTag effectTag = (GameplayEffectTag) Target;
            
            effectTag.tag = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer), effectTag.tag,"标签");
            effectTag.grantedTag = (CommonAddAndRemoveTag)GUILayoutExtension.DrawField(typeof(CommonAddAndRemoveTag), effectTag.grantedTag,"GE添加时对组件的Tag操作");
            effectTag.addConTag = (CommonConditionTag)GUILayoutExtension.DrawField(typeof(CommonConditionTag), effectTag.addConTag,"GE添加的条件标签");
            effectTag.removeTags = (CommonConditionTag)GUILayoutExtension.DrawField(typeof(CommonConditionTag), effectTag.removeTags,"GE被移除的条件标签");
            
            return effectTag;
        }

        public override float GetHeight()
        {
            return 0;
        }
    }
}