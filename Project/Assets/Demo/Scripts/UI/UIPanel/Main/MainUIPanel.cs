using LCUI;

namespace Demo.UI
{
    public class MainUIPanelModel : UIModel
    {

    }
    
    public class MainUIPanel : UIPanel<MainUIPanelModel>
    {
        private UIPartialPanelGlue<MainUI_EventPanel> eventPanel = new UIPartialPanelGlue<MainUI_EventPanel>("");
    }
}