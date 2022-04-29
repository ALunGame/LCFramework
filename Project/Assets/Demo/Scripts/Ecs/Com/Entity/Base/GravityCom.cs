using LCECS.Core;
using System.Collections;
using UnityEngine;

namespace Demo.Com
{
    /// <summary>
    /// 重力方向
    /// </summary>
    public enum GravityDir
    {
        Down,
        Up,
    }

    public class GravityCom : BaseCom
    {
        public GravityDir Dir = GravityDir.Down;
    }
}