using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues;

using UnityObject = UnityEngine.Object;
using LCToolkit.Help;
using System;

namespace LCToolkit.Extension
{
    public static partial class GUILayoutExtension
    {
        /// <summary>
        /// 创建一个显示隐藏组
        /// </summary>
        /// <param name="key">用于存取上下文数据</param>
        /// <param name="visible">是否可见</param>
        /// <param name="drawFunc">组内绘制函数</param>
        /// <param name="speed">动画速度</param>
        /// <returns></returns>
        public static void FadeGroup(string key, bool visible, Action drawFunc, float speed = 1)
        {
            if (!GUIHelper.TryGetContextData<AnimFloat>(key, out var contextData))
                contextData.value = new AnimFloat(visible ? 1 : 0);
            contextData.value.speed = speed * (visible ? 1 : 2);
            contextData.value.target = visible ? 1 : 0;
            float t = contextData.value.value;
            if (visible)
            {
                t--;
                t = -(t * t * t * t - 1);
            }
            else
            {
                t = t * t;
            }

            GUIExtension.SetAlpha(t, () =>
            {
                if (EditorGUILayout.BeginFadeGroup(t))
                {
                    drawFunc?.Invoke();
                }
                EditorGUILayout.EndFadeGroup();
            });
        }

        /// <summary>
        /// 创建一个垂直的布局
        /// </summary>
        /// <param name="drawFunc">组内绘制函数</param>
        /// <param name="options">布局属性</param>
        /// <returns></returns>
        public static void VerticalGroup(Action drawFunc,params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(EditorStylesExtension.RoundedBoxStyle, options);
            drawFunc?.Invoke();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 创建一个水平的布局
        /// </summary>
        /// <param name="drawFunc">组内绘制函数</param>
        /// <param name="options">布局属性</param>
        /// <returns></returns>
        public static void HorizontalGroup(Action drawFunc, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(EditorStylesExtension.RoundedBoxStyle, options);
            drawFunc?.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 创建一个开关布局
        /// </summary>
        /// <param name="label">开关名</param>
        /// <param name="foldout">是否展开</param>
        /// <param name="enable">是否开启</param>
        /// <param name="drawFunc">布局内绘制函数</param> 
        /// <returns></returns>
        public static (bool foldout, bool enable) ToggleGroup(string label, bool foldout, bool enable, Action drawFunc)
        {
            VerticalGroup(() => {
                Rect rect = GUILayoutUtility.GetRect(50, 25);
                rect = EditorGUI.IndentedRect(rect);
                Rect toggleRect = new Rect(rect.x + 20, rect.y, rect.height, rect.height);

                Event current = Event.current;
                if (current.type == EventType.MouseDown && current.button == 0)
                {
                    if (toggleRect.Contains(current.mousePosition))
                    {
                        enable = !enable;
                    }
                    else if (rect.Contains(current.mousePosition))
                    {
                        foldout = !foldout;
                    }
                }

                switch (current.type)
                {
                    case EventType.MouseDown:
                    case EventType.MouseUp:
                    case EventType.Repaint:
                        GUI.Box(rect, string.Empty, GUI.skin.button);

                        Rect t = rect;
                        t.xMin += 5;
                        t.xMax = t.xMin + t.height;
                        EditorGUI.Foldout(t, foldout, string.Empty);

                        toggleRect.width = rect.width - t.width;
                        EditorGUI.ToggleLeft(toggleRect, label, enable);
                        break;
                    default:
                        break;
                }

                EditorGUI.BeginDisabledGroup(!enable);
                EditorGUI.indentLevel++;

                if (enable)
                    drawFunc?.Invoke();

                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();
            });
            return (foldout, enable);
        }

        /// <summary>
        /// 创建一个折叠区域
        /// </summary>
        /// <param name="label">折叠名</param>
        /// <param name="foldout"><是否折叠/param>
        /// <returns></returns>
        public static void FoldoutGroup(string label, bool foldout, Action drawFunc)
        {
            VerticalGroup(() =>
            {
                Rect rect = GUILayoutUtility.GetRect(50, 25);
                rect = EditorGUI.IndentedRect(rect);

                Event current = Event.current;
                if (current.type == EventType.MouseDown && current.button == 0)
                {
                    if (rect.Contains(current.mousePosition))
                    {
                        foldout = !foldout;
                        current.Use();
                    }
                }

                switch (current.type)
                {
                    case EventType.MouseDown:
                    case EventType.MouseUp:
                    case EventType.Repaint:
                        GUI.Box(rect, string.Empty, GUI.skin.button);

                        Rect t = rect;
                        t.xMin += 5;
                        t.xMax -= 5;
                        EditorGUI.Foldout(t, foldout, label);
                        break;
                    default:
                        break;
                }

                EditorGUI.indentLevel++;

                if (foldout)
                    drawFunc?.Invoke();

                EditorGUI.indentLevel--;
            });
        }

        /// <summary>
        /// 绘制一个文件选择窗口
        /// </summary>
        /// <param name="label">提示名</param>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string FilePath(string label, string path)
        {
            EditorGUILayout.BeginHorizontal();
            path = EditorGUILayout.TextField(label, path);
            Rect rect = GUILayoutUtility.GetLastRect();
            UnityObject uObj = GUIExtension.DragDropAreaSingle(rect, DragAndDropVisualMode.Copy);
            if (uObj != null && AssetDatabase.IsMainAsset(uObj))
            {
                string p = AssetDatabase.GetAssetPath(uObj);
                if (!AssetDatabase.IsValidFolder(p))
                    path = p;
            }
            if (GUILayout.Button(EditorGUIUtility.FindTexture("FolderEmpty Icon"), EditorStylesExtension.OnlyIconButtonStyle, GUILayout.Width(18), GUILayout.Height(18)))
            {
                path = EditorUtility.OpenFilePanel($"{label}文件选择", Application.dataPath, "*");
                if (!string.IsNullOrEmpty(path))
                    path = path.Substring(path.IndexOf("Assets"));
            }
            EditorGUILayout.EndHorizontal();
            return path;
        }

        /// <summary>
        /// 绘制一个目录选择窗口
        /// </summary>
        /// <param name="label">提示名</param>
        /// <param name="folderPath">目录路径</param>
        /// <returns></returns>
        public static string FolderPath(string label, string folderPath)
        {
            EditorGUILayout.BeginHorizontal();
            folderPath = EditorGUILayout.TextField(label, folderPath);
            Rect rect = GUILayoutUtility.GetLastRect();
            UnityObject uObj = GUIExtension.DragDropAreaSingle(rect, DragAndDropVisualMode.Copy);
            if (uObj != null && AssetDatabase.IsMainAsset(uObj))
            {
                string p = AssetDatabase.GetAssetPath(uObj);
                if (AssetDatabase.IsValidFolder(p))
                    folderPath = p;
            }
            if (GUILayout.Button(EditorGUIUtility.FindTexture("FolderEmpty Icon"), EditorStylesExtension.OnlyIconButtonStyle, GUILayout.Width(18), GUILayout.Height(18)))
            {
                folderPath = EditorUtility.OpenFolderPanel($"{label}目录选择", Application.dataPath, string.Empty);
                if (!string.IsNullOrEmpty(folderPath))
                    folderPath = folderPath.Substring(folderPath.IndexOf("Assets"));
            }
            EditorGUILayout.EndHorizontal();
            return folderPath;
        }


    }
}
