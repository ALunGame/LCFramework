using System;
using System.Collections;
using UnityEngine;

namespace LCUI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class UIPanelAttribute : Attribute
    {
        /// <summary>
        /// 默认画布类型
        /// </summary>
        public UICanvasType CanvasType { get; private set; }

        /// <summary>
        /// 默认层级
        /// </summary>
        public UILayer DefaultLayer { get; private set; }

        /// <summary>
        /// 默认显示规则
        /// </summary>
        public UIShowRule DefaultShowRule { get; private set; }

        /// <summary>
        /// 默认预制体名
        /// </summary>
        public string UIPrefabName { get; private set; }

        public UIPanelAttribute(string prefabName, UILayer layer)
        {
            UIPrefabName = prefabName;
            DefaultLayer = layer;
            DefaultShowRule = UIShowRule.Overlay_NoNeedBack;
            CanvasType = UICanvasType.Static;
        }

        public UIPanelAttribute(string prefabName, UILayer layer, UIShowRule showRule)
        {
            UIPrefabName = prefabName;
            DefaultLayer = layer;
            DefaultShowRule = showRule;
            CanvasType = UICanvasType.Static;
        }

        public UIPanelAttribute(string prefabName, UILayer layer, UIShowRule showRule, UICanvasType canvasType)
        {
            UIPrefabName = prefabName;
            DefaultLayer = layer;
            DefaultShowRule = showRule;
            CanvasType = canvasType;
        }
    }
}