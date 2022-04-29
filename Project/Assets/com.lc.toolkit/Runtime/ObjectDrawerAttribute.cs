#if UNITY_EDITOR
using System;

namespace LCToolkit
{
    /// <summary>
    /// 声明字段自定义绘制
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FieldDrawerAttribute : Attribute { }

    /// <summary>
    /// Unity内部资源
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class UnityAssetTypeAttribute : Attribute
    {
        private Type objType;

        public Type ObjType { get { return objType; } }

        /// <summary>
        /// Unity内部资源
        /// </summary>
        public UnityAssetTypeAttribute(Type _type)
        {
            objType = _type;
        }
    }
} 
#endif