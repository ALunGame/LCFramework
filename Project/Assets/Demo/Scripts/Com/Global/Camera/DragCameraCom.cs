using Cinemachine;
using LCECS.Core;
using LCToolkit;
using System;
using UnityEngine;

namespace Demo.Com
{
    /// <summary>
    /// 拖动相机
    /// </summary>
    public class DragCameraCom : BaseCom
    {
        #region Static

        /// <summary>
        /// 拖拽速度
        /// </summary>
        public float DragSpeed = 1.2f;

        /// <summary>
        /// 弹簧回弹区域
        /// </summary>
        public float SpringOffset = 0.5f;
        /// <summary>
        /// 弹簧回弹时间
        /// </summary>
        public float SpringSmoothTime = 0.05f;
        /// <summary>
        /// 弹簧强度
        /// </summary>
        public float SpringIntensity = 0.75f;

        /// <summary>
        /// 惯性移动速率
        /// </summary>
        public float InertiaRate = 0.4f;
        /// <summary>
        /// 惯性移动阻尼总时间
        /// </summary>
        public float InertiaDampDuration = 0.4f;

        #endregion

        /// <summary>
        /// 虚拟相机
        /// </summary>
        [NonSerialized]
        public CinemachineVirtualCamera CMCamera;

        /// <summary>
        /// 相机区域
        /// </summary>
        [NonSerialized]
        public Shape CMArea = new Shape() { ShapeType = ShapeType.AABB};

        #region 拖拽

        /// <summary>
        /// 拖拽目标
        /// </summary>
        [NonSerialized]
        public GameObject DragTarget;

        /// <summary>
        /// 拖拽区域
        /// </summary>
        [NonSerialized]
        public Shape DragArea = new Shape() { ShapeType = ShapeType.AABB };

        #endregion

        #region 弹簧

        /// <summary>
        /// 弹簧区域
        /// </summary>
        [NonSerialized]
        public Shape SpringArea = new Shape() { ShapeType = ShapeType.AABB };

        /// <summary>
        /// 弹簧移动中
        /// </summary>
        [NonSerialized]
        public bool IsSpringMove;

        /// <summary>
        /// 处于弹簧阻尼阶段
        /// </summary>
        [NonSerialized]
        public bool IsSpringDamping;

        #endregion

        #region 惯性

        /// <summary>
        /// 惯性移动中
        /// </summary>
        [NonSerialized]
        public bool IsInertiaMove;

        /// <summary>
        /// 惯性移动速度
        /// </summary>
        [NonSerialized]
        public float InertiaSpeed;

        /// <summary>
        /// 惯性移动方向
        /// </summary>
        [NonSerialized]
        public Vector3 InertiaDir;

        /// <summary>
        /// 惯性阻尼
        /// </summary>
        [NonSerialized]
        public float InertiaDamp;

        #endregion

        #region 事件

        /// <summary>
        /// 输入事件
        /// </summary>
        public UnityEventInfo EventInfo;

        #endregion
    }
}
