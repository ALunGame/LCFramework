using System;

namespace LCToolkit
{
    public interface IBindable
    {
        /// <summary>
        /// ֵ�ı�
        /// </summary>
        void ValueChanged();

        /// <summary>
        /// �����¼�
        /// </summary>
        void ClearChangedEvent();
    }
}