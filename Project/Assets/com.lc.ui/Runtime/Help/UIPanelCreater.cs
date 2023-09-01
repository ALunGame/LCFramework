﻿
using System.Collections;
using Demo;
using UnityEngine;
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
        public static void CreateUIPanelTrans(UIPanelDef panelId,InternalUIPanel panel)
        {
            UIPanelCnf panelCnf = UILocate.UI.GetPanelCnf(panelId);

            RectTransform panelRootTrans = UILocate.UICenter.GetUILayerTrans(panelCnf.layer, panelCnf.canvas);
            if (panelRootTrans == null)
            {
                UILocate.Log.LogError("创建界面节点失败,没有找到层级节点>>>", panelCnf.layer, panelCnf.canvas);
                return;
            }
            if (string.IsNullOrEmpty(panelCnf.prefab))
            {
                UILocate.Log.LogError("创建界面节点失败,没有声明预制体>>>",panelCnf.prefab);
                return;
            }
            GameObject goAsset = IAFramework.GameContext.Asset.LoadPrefab(panelCnf.prefab);
            if (goAsset == null)
            {
                UILocate.Log.LogError("创建界面节点失败,没有找到预制体>>>", panelCnf.prefab);
                return;
            }
            GameObject panGo = GameObject.Instantiate(goAsset);
            panGo.transform.SetParent(panelRootTrans, false);
            panGo.transform.Reset();

            //设置节点
            panel.SetTransform(panGo.transform);
            panel.Awake();
        }

        /// <summary>
        /// 创建界面节点
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static void CreateUIPanelTrans(InternalUIPanel panel,Transform panelTrans)
        {
            if (panelTrans == null)
            {
                UILocate.Log.LogError("创建界面节点失败,没有节点>>>");
                return;
            }
            //设置节点
            panel.SetTransform(panelTrans.transform);
            panel.Awake();
        }
    }
}