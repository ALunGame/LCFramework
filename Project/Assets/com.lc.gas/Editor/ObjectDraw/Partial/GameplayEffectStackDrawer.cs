using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(GameplayEffectStack))]
    public class GameplayEffectStackDrawer : ObjectDrawer
    {
        public override object OnGUI(Rect _position, GUIContent _label)
        {
            GameplayEffectStack stack = (GameplayEffectStack) Target;
            stack.limitCnt = EditorGUILayout.IntField("最大堆叠数量", stack.limitCnt);
            stack.type = (TargetType)EditorGUILayout.EnumPopup("堆叠目标", stack.type);
            stack.addRefreshDuration = EditorGUILayout.Toggle("添加时刷新持续时间", stack.addRefreshDuration);
            stack.expirationPolicy = (StackExpirationPolicy)EditorGUILayout.EnumPopup("到期时策略", stack.expirationPolicy);
            return stack;
        }

        public override float GetHeight()
        {
            return 0;
        }
    }
}