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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetGraphType">��ͼ��</param>
        public CustomGraphWindowAttribute(Type targetGraphType)
        {
            this.targetGraphType = targetGraphType;
        }
    }
}
