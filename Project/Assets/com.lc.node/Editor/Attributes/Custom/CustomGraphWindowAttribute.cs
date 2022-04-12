using System;

namespace LCNode
{
    /// <summary>
    /// 自定义窗口显示视图
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomGraphWindowAttribute : Attribute
    {
        public Type targetGraphType;

        public CustomGraphWindowAttribute(Type targetGraphType)
        {
            this.targetGraphType = targetGraphType;
        }
    }
}
