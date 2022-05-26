﻿#if UNITY_EDITOR
using System;

namespace LCToolkit
{
    /// <summary>
    /// 声明字段自定义绘制
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FieldDrawerAttribute : Attribute { }
} 
#endif