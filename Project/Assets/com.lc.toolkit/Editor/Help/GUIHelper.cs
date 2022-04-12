using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace LCToolkit
{
    /// <summary>
    /// GUI辅助工具
    /// </summary>
    public static partial class GUIHelper
    {
        #region GUIContent

        public class GUIContentPool
        {
            Dictionary<string, GUIContent> GUIContentsCache = new Dictionary<string, GUIContent>();

            public GUIContent TextContent(string _name)
            {
                GUIContent content;
                if (!GUIContentsCache.TryGetValue(_name, out content))
                    content = new GUIContent(_name);
                content.tooltip = string.Empty;
                content.image = null;
                return content;
            }

            public GUIContent TextContent(string _name, Texture2D _image)
            {
                GUIContent content = TextContent(_name);
                content.image = _image;
                return content;
            }

            public GUIContent TextContent(string _name, string _tooltip)
            {
                GUIContent content = TextContent(_name);
                content.tooltip = _tooltip;
                return content;
            }

            public GUIContent TextContent(string _name, string _tooltip, Texture2D _image)
            {
                GUIContent content = TextContent(_name);
                content.tooltip = _tooltip;
                content.image = _image;
                return content;
            }
        }

        static GUIContentPool ContentPool = new GUIContentPool();

        public static GUIContent TextContent(string _name)
        {
            return ContentPool.TextContent(_name);
        }

        public static GUIContent TextContent(string _name, Texture2D _image)
        {
            return ContentPool.TextContent(_name, _image);
        }

        public static GUIContent TextContent(string _name, string _tooltip)
        {
            return ContentPool.TextContent(_name, _tooltip);
        }

        public static GUIContent TextContent(string _name, string _tooltip, Texture2D _image)
        {
            return ContentPool.TextContent(_name, _tooltip, _image);
        }

        #endregion

        #region ContextData
        public interface IContextData { }

        public sealed class ContextData<T> : IContextData { public T value; }

        public class ContextDataCache
        {
            Dictionary<string, IContextData> ContextDatas = new Dictionary<string, IContextData>();

            public bool TryGetContextData<T>(string _key, out ContextData<T> _contextData)
            {
                if (ContextDatas.TryGetValue(_key, out IContextData _data))
                {
                    if (_data is ContextData<T> _t_data)
                    {
                        _contextData = _t_data;
                        return true;
                    }
                }
                _contextData = new ContextData<T>();
                ContextDatas[_key] = _contextData;
                return false;
            }

            public ContextData<T> GetContextData<T>(string _key, T _default = default)
            {
                if (ContextDatas.TryGetValue(_key, out IContextData _data))
                {
                    if (_data is ContextData<T> _t_data)
                    {
                        return _t_data;
                    }
                }
                var contextData = new ContextData<T>();
                ContextDatas[_key] = contextData;
                return contextData;
            }
        }

        static ContextDataCache ContextDatas = new ContextDataCache();

        public static bool TryGetContextData<T>(string _key, out ContextData<T> _contextData)
        {
            return ContextDatas.TryGetContextData(_key, out _contextData);
        }

        public static ContextData<T> GetContextData<T>(string _key, T _default = default)
        {
            return ContextDatas.GetContextData(_key, _default);
        }
        #endregion

        #region 按钮

        /// <summary>
        /// 绘制一个按钮
        /// </summary>
        /// <param name="btnName">按钮名</param>
        /// <param name="width">按钮宽</param>
        /// <param name="height">按钮高</param>
        /// <param name="callBack">按钮回调</param>
        public static void Btn(string btnName, float width, float height, Action callBack)
        {
            if (GUILayout.Button(new GUIContent(btnName), GUI.skin.button, GUILayout.Width(width), GUILayout.Height(height)))
            {
                callBack?.Invoke();
            }
        }

        /// <summary>
        /// 绘制一个按钮
        /// </summary>
        /// <param name="btnName">按钮名</param>
        /// <param name="width">按钮宽</param>
        /// <param name="height">按钮高</param>
        /// <param name="callBack">按钮回调</param>
        public static void Btn(object btnName, float width, float height, Action callBack)
        {
            if (GUILayout.Button(new GUIContent(btnName.ToString()), GUI.skin.button, GUILayout.Width(width), GUILayout.Height(height)))
            {
                callBack?.Invoke();
            }
        }

        #endregion

        #region 对话框

        /// <summary>
        /// 编辑器对话框
        /// </summary>
        /// <param name="title">对话框标题</param>
        /// <param name="message">对话框内容</param>
        /// <param name="okFunc">确定回调</param>
        /// <param name="cancleFunc">取消回调</param>
        /// <param name="ok">确认按钮名</param>
        /// <param name="cancle">取消按钮名</param>
        public static void Dialog(string title, string message, Action okFunc = null, Action cancleFunc = null, string ok = "确定", string cancle = "取消")
        {
            bool isTrue = EditorUtility.DisplayDialog(title, message, ok, cancle);
            if (isTrue)
            {
                if (okFunc != null)
                {
                    okFunc();
                }
            }
            else
            {
                if (cancleFunc != null)
                {
                    cancleFunc();
                }
            }
        }

        #endregion

        #region 下拉框

        /// <summary>
        /// 编辑器下拉框
        /// </summary>
        /// <param name="selItem">下拉框名</param>
        /// <param name="itemList">列表</param>
        /// <param name="selCallBack">选择回调</param>
        /// <param name="width">下拉框宽</param>
        public static void Dropdown(string selItem, List<string> itemList, Action<string> selCallBack, float width = 100)
        {
            if (EditorGUILayout.DropdownButton(new GUIContent(selItem), FocusType.Keyboard, GUILayout.Width(width)))
            {
                GenericMenu menu = new GenericMenu();
                foreach (string item in itemList)
                {
                    AddMenuItem(menu, item, selItem, selCallBack);
                }
                menu.ShowAsContext();
            }
        }

        private static void AddMenuItem(GenericMenu menu, string item, string selItem, Action<string> selCallBack)
        {
            menu.AddItem(new GUIContent(item), selItem.Equals(item), (x) =>
            {

                selItem = x.ToString();
                if (selCallBack != null)
                {
                    selCallBack(x.ToString());
                }

            }, item);
        }

        /// <summary>
        /// 编辑器下拉框
        /// </summary>
        /// <param name="selItem">下拉框名</param>
        /// <param name="itemList">列表</param>
        /// <param name="selCallBack">选择回调</param>
        /// <param name="width">下拉框宽</param>
        public static void Dropdown(string selItem, List<string> itemList, Action<int> selCallBack, float width = 100)
        {
            if (EditorGUILayout.DropdownButton(new GUIContent(selItem), FocusType.Keyboard, GUILayout.Width(width)))
            {
                GenericMenu menu = new GenericMenu();

                for (int i = 0; i < itemList.Count; i++)
                {
                    AddMenuItem(menu, itemList[i], selItem, i, selCallBack);
                }
                menu.ShowAsContext();
            }
        }

        private static void AddMenuItem(GenericMenu menu, string item, string selItem, int index, Action<int> selCallBack)
        {
            menu.AddItem(new GUIContent(item), selItem.Equals(item), (x) =>
            {

                selItem = x.ToString();
                if (selCallBack != null)
                {
                    selCallBack(index);
                }

            }, item);
        }

        #endregion

        #region 线和区域

        /// <summary>
        /// 画贝赛尔曲线
        /// </summary>
        /// <param name="startPosition">起始位置</param>
        /// <param name="endPosition">结束位置</param>
        /// <param name="width">线宽</param>
        /// <param name="color">颜色</param>
        /// <param name="texture">贴图</param>
        public static void DrawBezierLine(Vector3 startPosition, Vector3 endPosition, float width, Color color = default(Color), Texture2D texture = null)
        {
            Handles.DrawBezier(startPosition, endPosition, startPosition - Vector3.left * 50f, endPosition + Vector3.left * 50f, color, texture, width);
        }

        private static Type EditorGUI;
        private static object EditorGUIObj;
        /// <summary>
        /// 绘制边框线
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="size">大小</param>
        /// <param name="color">颜色</param>
        public static void DrawOutline(Rect rect, float size, Color color)
        {
            if (EditorGUI == null)
            {
                Assembly asm = Assembly.Load("UnityEditor");
                EditorGUI = asm.GetType("UnityEditor.EditorGUI");
                EditorGUIObj = Activator.CreateInstance(EditorGUI);
            }
            MethodInfo oMethod = EditorGUI.GetMethod("DrawOutline", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            oMethod.Invoke(EditorGUIObj, new Object[] { rect, size, color });
        }

        /// <summary>
        /// 画一条线
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <param name="color">颜色</param>
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(start, end);
        }

        #endregion
    }
}
