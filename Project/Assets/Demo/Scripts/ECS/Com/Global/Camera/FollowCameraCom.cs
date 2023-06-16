using Cinemachine;
using LCECS.Core;
using LCMap;
using UnityEngine;
using System;
using LCToolkit;

namespace Demo.Com
{
    /// <summary>
    /// 跟随相机
    /// </summary>
    public class FollowCameraCom : BaseCom
    {
        /// <summary>
        /// 虚拟相机
        /// </summary>
        [NonSerialized]
        public CinemachineVirtualCamera CMCamera;
    }
}
