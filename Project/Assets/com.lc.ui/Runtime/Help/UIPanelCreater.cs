using System.Collections;
using UnityEngine;
using LCLoad;
using LCToolkit;

namespace LCUI
{
    /// <summary>
    /// UIPanel创建
    /// </summary>
    public static class UIPanelCreater
    {
        /// <summary>
        /// 创建界面节点
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static RectTransform CreateUIPanelTrans(InternalUIPanel panel)
        {
            RectTransform panelRootTrans = UILocate.UICenter.GetUILayerTrans(panel.Layer, panel.CanvasType);
            if (panelRootTrans == null)
            {
                UILocate.Log.LogError("创建界面节点失败,没有找到层级节点>>>", panel.Layer, panel.CanvasType);
                return null;
            }
            if (string.IsNullOrEmpty(panel.UIPrefabName))
            {
                UILocate.Log.LogError("创建界面节点失败,没有声明预制体>>>",panel.UIPrefabName);
                return null;
            }
            GameObject goAsset = LoadHelper.LoadPrefab(panel.UIPrefabName);
            if (goAsset == null)
            {
                UILocate.Log.LogError("创建界面节点失败,没有找到预制体>>>", panel.UIPrefabName);
                return null;
            }
            GameObject panGo = GameObject.Instantiate(goAsset);
            panGo.transform.SetParent(panelRootTrans, false);
            panGo.transform.Reset();

            //设置节点
            panel.SetTransform(panGo.transform);

            return panGo.GetComponent<RectTransform>();
        }

        /// <summary>
        /// 创建界面节点
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static RectTransform CreateUIPanelTrans(InternalUIPanel panel,RectTransform panelTrans)
        {
            if (panelTrans == null)
            {
                UILocate.Log.LogError("创建界面节点失败,没有节点>>>");
                return null;
            }
            //设置节点
            panel.SetTransform(panelTrans.transform);
            return panelTrans;
        }
    }
}