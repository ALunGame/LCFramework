using System;

namespace LCToolkit
{
    public interface IBindable
    {
        event Action<object> onValueChanged;

        /// <summary>
        /// 设置值不通知
        /// </summary>
        /// <param name="value"></param>
        void SetValueWithoutNotify(object value);

        /// <summary>
        /// 注册值变化事件
        /// </summary>
        /// <param name="onValueChanged"></param>
        void RegisterValueChangedEvent(Action<object> onValueChanged);

        /// <summary>
        /// 注销值变化事件
        /// </summary>
        /// <param name="onValueChanged"></param>
        void UnregisterValueChangedEvent(Action<object> onValueChanged);

        /// <summary>
        /// 清理事件
        /// </summary>
        void ClearChangedEvent();
    }
}