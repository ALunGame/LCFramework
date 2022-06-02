using System;

namespace LCUI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class UIPanelIdAttribute : Attribute
    {
        /// <summary>
        /// 界面Id
        /// </summary>
        public UIPanelId PanelId { get; private set; }

        public UIPanelIdAttribute(UIPanelId panelId)
        {
            PanelId = panelId;
        }
    }
}