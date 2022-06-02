using System.Collections.Generic;

namespace LCUI
{
    public abstract class UIRule
    {
        protected UIServer _Server;

        public UIRule(UIServer server)
        {
            _Server = server;
        }

        public abstract List<UIShowRule> CheckRules { get; }

        public void ShowPanel(UIPanelId panelId, InternalUIPanel panel)
        {
            if (!CheckRules.Contains(panel.DefaultShowRule))
            {
                return;
            }
            OnShowPanel(panelId, panel);
        }

        public virtual void OnShowPanel(UIPanelId panelId,InternalUIPanel panel)
        {

        }

        public void HidePanel(UIPanelId panelId, InternalUIPanel panel)
        {
            if (!CheckRules.Contains(panel.DefaultShowRule))
            {
                return;
            }
            OnHidePanel(panelId, panel);
        }

        public virtual void OnHidePanel(UIPanelId panelId, InternalUIPanel panel)
        {

        }

        public virtual void Clear()
        {

        }
    } 
}
