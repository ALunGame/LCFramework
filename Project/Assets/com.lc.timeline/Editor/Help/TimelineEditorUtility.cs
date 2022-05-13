using LCToolkit;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCTimeline
{
    /// <summary>
    /// 自定义窗口显示视图
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomTimelineWindowAttribute : Attribute
    {
        public Type targetGraphType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetGraphType">视图类</param>
        public CustomTimelineWindowAttribute(Type targetGraphType)
        {
            this.targetGraphType = targetGraphType;
        }
    }

    public static class TimelineEditorUtility
    {
        #region GraphWindowTypeCache
        static Dictionary<Type, Type> WindowTypeCache;

        public static Type GetGraphWindowType(Type graphType)
        {
            if (WindowTypeCache == null)
            {
                WindowTypeCache = new Dictionary<Type, Type>();
                foreach (var type in TypeCache.GetTypesDerivedFrom<TimelineWindow>())
                {
                    if (type.IsAbstract) continue;

                    foreach (var att in AttributeHelper.GetTypeAttributes(type, true))
                    {
                        if (att is CustomTimelineWindowAttribute sAtt)
                            WindowTypeCache[sAtt.targetGraphType] = type;
                    }
                }
            }
            if (WindowTypeCache.TryGetValue(graphType, out Type windowType))
                return windowType;
            if (graphType.BaseType != null)
                return GetGraphWindowType(graphType.BaseType);
            else
                return typeof(TimelineWindow);
        }

        #endregion
    }
}