using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCUI
{
    [Serializable]
    public class UILayerInfo
    {
        public UILayer Layer;
        public RectTransform LayerGo;
    }

    public class LCUIRoot:MonoBehaviour
    {
        [SerializeField]
        public List<UIPanelInfo> PanelConfDict = new List<UIPanelInfo>();
        [SerializeField]
        public List<UILayerInfo> LayerGoDict = new List<UILayerInfo>();

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            Test();
        }

        private void Init()
        {
            Dictionary<UIPanelId, UIPanelInfo> panelConfDict = new Dictionary<UIPanelId, UIPanelInfo>();
            Dictionary<UILayer, RectTransform> layerGoDict = new Dictionary<UILayer, RectTransform>();
            for (int i = 0; i < PanelConfDict.Count; i++)
            {
                if (panelConfDict.ContainsKey(PanelConfDict[i].PanelId))
                {
                    PanelConfDict.RemoveAt(i);
                }
                else
                {
                    panelConfDict.Add(PanelConfDict[i].PanelId, PanelConfDict[i]);
                }
            }

            for (int i = 0; i < LayerGoDict.Count; i++)
            {
                if (layerGoDict.ContainsKey(LayerGoDict[i].Layer))
                {
                    LayerGoDict.RemoveAt(i);
                }
                else
                {
                    layerGoDict.Add(LayerGoDict[i].Layer, LayerGoDict[i].LayerGo);
                }
            }

            LCUILocate.Init(panelConfDict, layerGoDict);
        }


        private void Test()
        {
            LCUILocate.ShowUI(UIPanelId.Fight, UIShowMode.DoNothing);
            LCUILocate.ShowUI(UIPanelId.FightUpWorld, UIShowMode.DoNothing);
        }
    }
}
