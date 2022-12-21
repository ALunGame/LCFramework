using System;

namespace LCToolkit
{
    public interface IBindable
    {
        /// <summary>
        /// 值改变
        /// </summary>
        void ValueChanged();

        /// <summary>
        /// 清理事件
        /// </summary>
        void ClearChangedEvent();
    }
}