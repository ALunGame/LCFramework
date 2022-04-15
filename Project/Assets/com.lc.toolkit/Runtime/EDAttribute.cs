#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace LCToolkit
{
    /// <summary>
    /// 只读属性
    /// </summary>
    public class EDReadOnlyAttribute : PropertyAttribute
    {

    }

    [CustomPropertyDrawer(typeof(EDReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}

#endif