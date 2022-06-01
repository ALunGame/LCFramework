using System;

namespace LCToolkit
{
    public interface IBindable
    {
        /// <summary>
        /// 清理事件
        /// </summary>
        void ClearChangedEvent();
    }
}