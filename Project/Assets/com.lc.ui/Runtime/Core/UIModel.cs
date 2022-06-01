using LCToolkit;
using System.Collections.Generic;

namespace LCUI
{
    public class UIModel
    {
        private List<IBindable> bindFields = new List<IBindable>();

        /// <summary>
        /// 绑定字段
        /// </summary>
        public List<IBindable> BindFields { get => bindFields; }

        /// <summary>
        /// 添加绑定字段
        /// </summary>
        public void AddBindable(IBindable bindable)
        {
            bindFields.Add(bindable);
        }

        /// <summary>
        /// 清楚绑定事件
        /// </summary>
        public void ClearEvent()
        {
            for (int i = 0; i < bindFields.Count; i++)
            {
                bindFields[i].ClearChangedEvent();
            }
        }
    }
}
