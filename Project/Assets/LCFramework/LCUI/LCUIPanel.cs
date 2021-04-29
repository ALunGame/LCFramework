using UnityEngine;

namespace LCUI
{
    public class LCUIPanel:MonoBehaviour
    {
        private UIPanelId panelId;
        private RectTransform canvas;
        private Camera uiCamera;
        public UIPanelId PanelId { get => panelId; set => panelId = value; }
        public RectTransform Canvas { get => canvas; set => canvas = value; }
        public Camera UiCamera { get => uiCamera; set => uiCamera = value; }

        private void Start()
        {
            
        }

        private void Awake()
        {
            
        }

        public virtual void OnAwake()
        {

        }

        public virtual void OnShow(params object[] parms)
        {

        }

        public virtual void OnHide()
        {

        }
    }
}
