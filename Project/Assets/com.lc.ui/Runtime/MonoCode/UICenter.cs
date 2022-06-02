using UnityEngine;

namespace LCUI
{
    /// <summary>
    /// UI中心
    /// </summary>
    public class UICenter : MonoBehaviour
    {
        [Header("静态画布")]
        [SerializeField]
        private UICanvas staticCanvas;

        [Header("动态画布")]
        [SerializeField]
        private UICanvas dynamicCanvas;

        public UICanvas StaticCanvas { get => staticCanvas;}
        public UICanvas DynamicCanvas { get => dynamicCanvas;}


        private void Awake()
        {
            DontDestroyOnLoad(this);
            UILocate.SetUICenter(this);
            UILocate.Init();
        }


        private void OnDestroy()
        {
            UILocate.Clear();
        }

        /// <summary>
        /// 获得UI层级的父节点
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="canvasType"></param>
        /// <returns></returns>
        public RectTransform GetUILayerTrans(UILayer layer,UICanvasType canvasType = UICanvasType.Static)
        {
            UICanvas canvas = canvasType == UICanvasType.Static ? StaticCanvas : dynamicCanvas;
            switch (layer)
            {
                case UILayer.Base:
                    return canvas.BaseTrans;
                case UILayer.First:
                    return canvas.FirstTrans;
                case UILayer.Second:
                    return canvas.SecondTrans;
                case UILayer.Three:
                    return canvas.ThreeTrans;
                case UILayer.Top:
                    return canvas.TopTrans;
                default:
                    break;
            }
            return canvas.BaseTrans;
        }
    }
}