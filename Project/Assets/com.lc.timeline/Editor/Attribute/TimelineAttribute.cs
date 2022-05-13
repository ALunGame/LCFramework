using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LCTimeline
{
    /// <summary>
    /// 自定义Clip显示
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomClipViewAttribute : Attribute
    {
        public Type dataType;

        /// <summary>
        /// 时间轴界面
        /// </summary>
        /// <param name="_dataType">数据类</param>
        public CustomClipViewAttribute(Type _dataType)
        {
            dataType = _dataType;
        }
    }

    /// <summary>
    /// 片段菜单显示
    /// </summary>
    public class ClipMenuAttribute : Attribute
    {
        public string MenuName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuName">菜单名</param>
        public ClipMenuAttribute(string menuName)
        {
            MenuName = menuName;
        }
    }

    /// <summary>
    /// 自定义轨道显示
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomTrackViewAttribute : Attribute
    {
        public Type dataType;

        /// <summary>
        /// 时间轴界面
        /// </summary>
        /// <param name="_dataType">数据类</param>
        public CustomTrackViewAttribute(Type _dataType)
        {
            dataType = _dataType;
        }
    }

    /// <summary>
    /// 轨道颜色
    /// </summary>
    public class TrackColorAttribute : Attribute
    {
        public Color UnSelectColor = Color.white;

        public Color SelectColor = Color.green;

        public Color DisplayNameColor = Color.black;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuName">菜单名</param>
        public TrackColorAttribute(Color selectColor)
        {
            this.SelectColor = selectColor;
        }

        public TrackColorAttribute(Color selectColor, Color unSelectColor)
        {
            this.SelectColor = selectColor;
            this.UnSelectColor = unSelectColor;
        }

        public TrackColorAttribute(Color selectColor, Color unSelectColor, Color displayNameColor)
        {
            this.SelectColor = selectColor;
            this.UnSelectColor = unSelectColor;
            this.DisplayNameColor = displayNameColor;
        }
    }

    /// <summary>
    /// 轨道菜单显示
    /// </summary>
    public class TrackMenuAttribute : Attribute
    {
        public string MenuName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuName">菜单名</param>
        public TrackMenuAttribute(string menuName)
        {
            MenuName = menuName;
        }
    }


    /// <summary>
    /// 片段颜色
    /// </summary>
    public class ClipColorAttribute : Attribute
    {
        public Color UnSelectColor = Color.white;

        public Color SelectColor = Color.green;

        public Color DisplayNameColor = Color.black;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuName">菜单名</param>
        public ClipColorAttribute(Color selectColor)
        {
            this.SelectColor = selectColor;
        }

        public ClipColorAttribute(Color selectColor, Color unSelectColor)
        {
            this.SelectColor = selectColor;
            this.UnSelectColor = unSelectColor;
        }

        public ClipColorAttribute(Color selectColor, Color unSelectColor, Color displayNameColor)
        {
            this.SelectColor = selectColor;
            this.UnSelectColor = unSelectColor;
            this.DisplayNameColor = displayNameColor;
        }
    }
}
