using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LCUI
{
    /// <summary>
    /// UI定位器
    /// </summary>
    public static class LCUILocate
    {
        private static LCUICenter uiServer;

        public static void Init(Dictionary<UIPanelId, UIPanelInfo> panelConfDict, Dictionary<UILayer, RectTransform> layerGoDict)
        {
            uiServer = new LCUICenter();
            uiServer.Init(panelConfDict, layerGoDict);
        }

        public static void Clear()
        {
            uiServer.Clear();
        }

        public static void ShowUI(UIPanelId panelId, UIShowMode showMode, params object[] parm)
        {
            uiServer.ShowUI(panelId, showMode, parm);
        }

        public static void HideUI(UIPanelId panelId)
        {
            uiServer.HideUI(panelId);
        }

        public static T GetUIPanel<T>(UIPanelId panelId) where T : LCUIPanel
        {
            return uiServer.GetUIPanel<T>(panelId);
        }
    }
}
