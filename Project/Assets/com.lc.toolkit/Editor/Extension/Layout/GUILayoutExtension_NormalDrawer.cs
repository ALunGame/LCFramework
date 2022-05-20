using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace LCToolkit
{
    public static partial class GUILayoutExtension
    {
        /// <summary> 不是<see cref="private"/>、或者标记了<see cref="SerializeField"/>特性，并且没有标记<see cref="NonSerializedAttribute"/>特性，并且没有标记<see cref="HideInInspector"/>特性。 </summary>
        /// <returns> 满足以上条件返回<see cref="true"/> </returns>
        public static bool CanDraw(FieldInfo fieldInfo)
        {
            return ((!fieldInfo.IsPrivate && !fieldInfo.IsFamily) || AttributeHelper.TryGetFieldAttribute(fieldInfo, out SerializeField serAtt))
                    && !AttributeHelper.TryGetTypeAttribute(fieldInfo.DeclaringType, out NonSerializedAttribute nonAtt)
                    && !AttributeHelper.TryGetFieldAttribute(fieldInfo, out HideInInspector hideAtt);
        }

        /// <summary>
        /// 绘制折叠元素
        /// </summary>
        /// <param name="key">用于存取上下文数据</param>
        /// <param name="guiContent">元素显示配置</param>
        /// <returns></returns>
        public static bool DrawFoldout(int key, GUIContent guiContent)
        {
            string text = string.Concat(c_EditorPrefsFoldoutKey, key, ".", guiContent.text);
            var @bool = GUIHelper.GetContextData(text, false);
            @bool.value = EditorGUILayout.Foldout(@bool.value, guiContent, true);
            return @bool.value;
        }

        /// <summary>
        /// 绘制字段
        /// </summary>
        /// <param name="fieldInfo">字段</param>
        /// <param name="context">字段所属环境</param>
        public static void DrawField(FieldInfo fieldInfo, object context)
        {
            GUIContent label = null;
            if (AttributeHelper.TryGetFieldAttribute(fieldInfo, out TooltipAttribute tooltipAtt))
                label = GUIHelper.TextContent(ObjectNames.NicifyVariableName(fieldInfo.Name), tooltipAtt.tooltip);
            else
                label = GUIHelper.TextContent(ObjectNames.NicifyVariableName(fieldInfo.Name));
            DrawField(fieldInfo, context, label);
        }

        /// <summary>
        /// 绘制字段
        /// </summary>
        /// <param name="fieldInfo">字段</param>
        /// <param name="context">字段所属环境</param>
        public static void DrawField(FieldInfo fieldInfo, object context, string label)
        {
            DrawField(fieldInfo, context, GUIHelper.TextContent(label));
        }

        /// <summary>
        /// 绘制字段
        /// </summary>
        /// <param name="fieldInfo">字段</param>
        /// <param name="context">字段所属环境</param>
        /// <param name="label"></param>
        public static void DrawField(FieldInfo fieldInfo, object context, GUIContent label)
        {
            object value = fieldInfo.GetValue(context);
            object newValue = DrawField(fieldInfo.FieldType, value, label);
            if (value == null || newValue == null)
            {
                return;
            }
            if (!newValue.Equals(value))
            {
                fieldInfo.SetValue(context, newValue);
            }
        }

        public static object DrawField(Type type, object value, GUIContent label = null)
        {
            // 判断是否是数组
            if (typeof(IList).IsAssignableFrom(type))
                return DrawArrayField(type, value, label);
            else
                return DrawSingleField(type, value, label);
        }

        public static object DrawField(Type type, object value, string label,string tooltip)
        {
            GUIContent lable = GUIHelper.TextContent(label, tooltip);
            return DrawField(type, value, lable);
        }

        /// <summary>
        /// 绘制单个元素
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="label">名</param>
        /// <returns></returns>
        static object DrawSingleField(Type objType, object value, GUIContent label = null)
        {
            if (GUIExtension.IsBasicType(objType))
            {
                if (label == null)
                {
                    label = new GUIContent(objType.Name);
                }
                float tmpHeight = GUIExtension.GetHeight(objType, label);
                return GUIExtension.DrawField(EditorGUILayout.GetControlRect(true, tmpHeight), value, label);
            }

            Type type = objType;
            //如果类型是类 或者 是值类型并且不是基元类型
            if (type.IsClass || (type.IsValueType && !type.IsPrimitive))
            {
                if (label == null)
                {
                    label = new GUIContent(objType.Name);
                }

                //委托类型
                if (typeof(Delegate).IsAssignableFrom(type))
                    return null;
                //基础类型并且为空
                if (typeof(object).IsAssignableFrom(type) && value == null)
                    return null;
                int hashCode = value.GetHashCode();

                //值为空
                if (value == null)
                {
                    //泛型
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        type = Nullable.GetUnderlyingType(type);
                    value = Activator.CreateInstance(type, true);
                }

                //遍历类型字段
                GUILayout.BeginVertical();
                if (DrawFoldout(hashCode, label))
                {
                    EditorGUI.indentLevel++;
                    foreach (var field in ReflectionHelper.GetFieldInfos(type))
                    {
                        if (!CanDraw(field)) continue;

                        GUIContent fileLable  = new GUIContent(field.Name);
                        if (AttributeHelper.TryGetFieldAttribute(field,out HeaderAttribute attr))
                        {
                            fileLable = new GUIContent(attr.header);
                        }
                        if (GUIExtension.IsBasicType(type))
                        {
                            float tmpHeight = GUIExtension.GetHeight(type, fileLable);
                            return GUIExtension.DrawField(EditorGUILayout.GetControlRect(true, tmpHeight), field.GetValue(value), fileLable);
                        }
                        else
                        {
                            DrawField(field, value, fileLable);
                        }
                    }
                    EditorGUI.indentLevel--;
                }
                GUILayout.EndVertical();
                return value;
            }

            float height = GUIExtension.GetHeight(type, label);
            return GUIExtension.DrawField(EditorGUILayout.GetControlRect(true, height), value, label);
        }

        static object DrawArrayField(Type objType, object value, GUIContent label)
        {
            Type fieldType = objType;

            Type elementType;
            if (fieldType.IsArray)
                elementType = fieldType.GetElementType();
            else
            {
                Type type2 = fieldType;
                while (!type2.IsGenericType)
                {
                    type2 = type2.BaseType;
                }
                elementType = type2.GetGenericArguments()[0];
            }

            IList list;
            if (value == null)
            {
                if (fieldType.IsGenericType || fieldType.IsArray)
                {
                    value = list = (Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
                    {
                        elementType
                    }), true) as IList);
                }
                else
                {
                    value = list = (Activator.CreateInstance(fieldType, true) as IList);
                }
                if (fieldType.IsArray)
                {
                    Array array = Array.CreateInstance(elementType, list.Count);
                    list.CopyTo(array, 0);
                    value = list = array;
                }
                GUI.changed = true;
            }
            else
            {
                list = (IList)value;
            }
            if (DrawFoldout(value.GetHashCode(), label))
            {
                EditorGUI.indentLevel++;
                bool flag = value.GetHashCode() == editingFieldHash;
                int count = (!flag) ? list.Count : savedArraySize;
                int newCount = EditorGUILayout.IntField(GUIHelper.TextContent("Size"), count);
                if (flag && editingArray && (GUIUtility.keyboardControl != currentKeyboardControl || (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return)))
                {
                    if (newCount != list.Count)
                    {
                        int currentCount = list.Count;
                        if (newCount > currentCount)
                        {
                            if (fieldType.IsArray)
                            {
                                Array array2 = Array.CreateInstance(elementType, newCount);
                                int num3 = -1;
                                for (int i = 0; i < newCount; i++)
                                {
                                    if (i < list.Count)
                                    {
                                        num3 = i;
                                    }
                                    if (num3 == -1)
                                    {
                                        break;
                                    }

                                }
                            }
                            else
                            {
                                Type type = list.Count > 0 ? list[list.Count - 1].GetType() : elementType;
                                if (!typeof(UnityObject).IsAssignableFrom(type))
                                {
                                    for (int i = currentCount; i < newCount; i++)
                                        list.Add(ReflectionHelper.CreateInstance(type));
                                }
                                else
                                {
                                    for (int i = currentCount; i < newCount; i++)
                                        list.Add(null);
                                }
                            }
                        }
                        else
                        {
                            if (!fieldType.IsArray)
                            {
                                while (list.Count > newCount)
                                {
                                    list.RemoveAt(list.Count - 1);
                                }
                            }
                        }
                        Event.current.Use();
                    }
                    editingArray = false;
                    savedArraySize = -1;
                    editingFieldHash = -1;
                    GUI.changed = true;
                }
                else if (newCount != count)
                {
                    if (!editingArray)
                    {
                        currentKeyboardControl = GUIUtility.keyboardControl;
                        editingArray = true;
                        editingFieldHash = value.GetHashCode();
                    }
                    savedArraySize = newCount;
                }

                for (int k = 0; k < list.Count; k++)
                {
                    object obj = list[k];
                    label.text = $"Element {obj.GetType().Name}" + k;
                    list[k] = DrawField(obj.GetType(), obj);
                }

                //for (int k = 0; k < list.Count; k++)
                //{
                //    label.text = "Element " + k;
                //    object obj = list[k];
                //    list[k] = DrawField(obj.GetType(), obj, "Element " + k);
                //}

                EditorGUI.indentLevel--;
            }
            return list;
        }

        const string c_EditorPrefsFoldoutKey = "CZToolKit.Core.Editors.Foldout.";

        static int currentKeyboardControl = -1;

        static bool editingArray = false;

        static int savedArraySize = -1;

        static int editingFieldHash;
    }
}