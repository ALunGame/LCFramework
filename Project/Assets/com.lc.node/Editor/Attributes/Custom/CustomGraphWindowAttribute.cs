using System;

namespace LCNode
{
    /// <summary>
    /// �Զ��崰����ʾ��ͼ
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
