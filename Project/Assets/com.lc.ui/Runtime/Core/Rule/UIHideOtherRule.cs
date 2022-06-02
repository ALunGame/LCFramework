﻿using System.Collections.Generic;

namespace LCUI
{
    public class UIHideOtherRule : UIRule
    {
        private List<UIShowRule> rules = new List<UIShowRule>() { UIShowRule.HideOther_NoNeedBack, UIShowRule.HideOther };

        public UIHideOtherRule(UIServer server) : base(server)
        {
        }

        public override List<UIShowRule> CheckRules => rules;

        public override void OnShowPanel(UIPanelId panelId, InternalUIPanel panel)
        {
            UILocate.UI.HideAllActivePanel();
        }
    }
}
