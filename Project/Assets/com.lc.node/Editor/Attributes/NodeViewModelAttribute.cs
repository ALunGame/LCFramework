using System;

namespace LCNode
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeViewModelAttribute : Attribute
    {
        public Type targetType;

        public NodeViewModelAttribute(Type targetType)
        {
            this.targetType = targetType;
        }
    }
}