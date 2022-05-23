using System;

namespace LCToolkit
{
    public interface IBindable
    {
        event Action<object> onValueChanged;

        /// <summary>
        /// ����ֵ��֪ͨ
        /// </summary>
        /// <param name="value"></param>
        void SetValueWithoutNotify(object value);

        /// <summary>
        /// ע��ֵ�仯�¼�
        /// </summary>
        /// <param name="onValueChanged"></param>
        void RegisterValueChangedEvent(Action<object> onValueChanged);

        /// <summary>
        /// ע��ֵ�仯�¼�
        /// </summary>
        /// <param name="onValueChanged"></param>
        void UnregisterValueChangedEvent(Action<object> onValueChanged);

        /// <summary>
        /// �����¼�
        /// </summary>
        void ClearChangedEvent();
    }
}