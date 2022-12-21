namespace LCUI
{
    public interface IUIServer
    {
        void Init();

        void Clear();

        T GetPanelModel<T>(UIPanelId panelId) where T : UIModel;

        void Show(UIPanelId panelId);

        void Hide(UIPanelId panelId);

        void HideAllActivePanel();

        void DestroyAllPanel();
    }
}
