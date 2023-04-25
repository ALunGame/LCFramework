using System.Collections.Generic;
using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(GameplayEffectOverflow))]
    public class GameplayEffectOverflowDrawer : ObjectDrawer
    {
        public override object OnGUI(Rect _position, GUIContent _label)
        {
            GameplayEffectOverflow overflow = (GameplayEffectOverflow) Target;
            overflow.addEffectNames =
                (List<string>)GUILayoutExtension.DrawField(typeof(List<string>), overflow.addEffectNames, "溢出时添加的GE名");
            
            overflow.denyOverflowActive = EditorGUILayout.Toggle("溢出的Apply不会刷新Duration", overflow.denyOverflowActive);
            if (overflow.denyOverflowActive)
            {
                overflow.clearStackOnOverflow = EditorGUILayout.Toggle("溢出时清空栈", overflow.clearStackOnOverflow);
            }
            else
            {
                overflow.clearStackOnOverflow = false;
            }
            
            return overflow;
        }

        public override float GetHeight()
        {
            return 0;
        }
    }
}