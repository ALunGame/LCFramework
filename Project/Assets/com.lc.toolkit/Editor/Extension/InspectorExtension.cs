using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using LCToolkit.Core;
using UnityObject = UnityEngine.Object;

namespace LCToolkit
{
    /// <summary>
    /// Inspector面板扩展
    /// </summary>
    public static partial class InspectorExtension
    {
        /// <summary>
        /// 在Inspector面板上绘制一个对象
        /// </summary>
        public static void DrawObjectInInspector(object _targetObject, object _owner = null)
        {
            if (_targetObject is UnityObject)
            {
                Selection.activeObject = _targetObject as UnityObject;
            }
            else
            {
                Selection.activeObject = ObjectInspector.Instance;
                ObjectInspector.Instance.Init(_targetObject, _owner);
            }
        }

        /// <summary>
        /// 在Inspector面板上绘制一个对象
        /// </summary>
        public static void DrawObjectInInspector(string _title, object _targetObject, object _owner = null)
        {
            DrawObjectInInspector(_targetObject, _owner);
            ObjectInspector.Instance.name = _title;
        }
    }
}