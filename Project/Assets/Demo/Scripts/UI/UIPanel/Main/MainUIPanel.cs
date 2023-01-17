using LCUI;

namespace Demo.UI
{
    public class MainUIPanelModel : UIModel
    {

    }
    
    public class MainUIPanel : UIPanel<MainUIPanelModel>
    {
        private UIPartialPanelGlue<MainUI_EventPanel> eventPanel = new UIPartialPanelGlue<MainUI_EventPanel>("Right/MainUI_EventPanel");
        private UIPartialPanelGlue<MainUI_FightPanel> fightPanel = new UIPartialPanelGlue<MainUI_FightPanel>("Bottom/MainUI_FightPanel");
    }
}