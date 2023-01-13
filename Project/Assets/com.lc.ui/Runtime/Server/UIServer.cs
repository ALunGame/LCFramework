using LCToolkit;
using System;
using System.Collections.Generic;
using Demo;

namespace LCUI
{
    public class UIServer : IUIServer
    {
        //界面和类型的映射
        private Dictionary<UIPanelDef,Type> panelTypeDict = new Dictionary<UIPanelDef,Type>();

        //界面规则
        private List<UIRule> rules = new List<UIRule>();

        //激活的界面
        private Dictionary<UIPanelDef, InternalUIPanel> activePanelDict = new Dictionary<UIPanelDef, InternalUIPanel>();

        //界面缓存
        private Dictionary<UIPanelDef, InternalUIPanel> cachePanelDict = new Dictionary<UIPanelDef, InternalUIPanel>();

        /// <summary>
        /// 所有的激活界面
        /// </summary>
        public IReadOnlyDictionary<UIPanelDef, InternalUIPanel> ActivePanelDict { get => activePanelDict; }

        public void Init()
        {
            rules.Add(new UIStackRule(this));
            rules.Add(new UIHideOtherRule(this));

            foreach (UIPanelCnf uiPanelCnf in LCConfig.Config.UIPanelCnf.Values)
            {
                Type uiPanlType = ReflectionHelper.GetType(uiPanelCnf.script);
                panelTypeDict.Add(uiPanelCnf.id, uiPanlType);
            }
        }

        public void Clear()
        {
            panelTypeDict.Clear();
            foreach (var item in rules)
                item.Clear();
            rules.Clear();
            activePanelDict.Clear();
            cachePanelDict.Clear();
        }

        public T GetPanelModel<T>(UIPanelDef panelId) where T : UIModel
        {
            InternalUIPanel panel = GetPanel(panelId);

            if (!activePanelDict.ContainsKey(panelId) && !cachePanelDict.ContainsKey(panelId))
            {
                cachePanelDict.Add(panelId, panel);
            }

            return (T)panel.Model;
        }

        public void Show(UIPanelDef panelId)
        {
            InternalUIPanel panel = GetPanel(panelId);
            foreach (var item in rules)
            {
                item.ShowPanel(panelId, panel);
            }
            ExecuteShowPanel(panelId, panel);
        }

        public void Hide(UIPanelDef panelId)
        {
            InternalUIPanel panel = ExecuteHidePanel(panelId);
            foreach (var item in rules)
            {
                item.HidePanel(panelId, panel);
            }
        }

        public void HideAllActivePanel()
        {
            foreach (UIPanelDef paneId in new List<UIPanelDef>(activePanelDict.Keys))
            {
                ExecuteHidePanel(paneId);
            }
        }

        public void DestroyAllPanel()
        {
            foreach (var item in activePanelDict.Values)
            {
                item.Hide();
                item.Destroy();
            }
            foreach (var item in cachePanelDict.Values)
            {
                item.Destroy();
            }
        }

        #region 创建界面

        /// <summary>
        /// 获得界面
        /// </summary>
        /// <param name="panelId">界面Id</param>
        /// <returns></returns>
        private InternalUIPanel GetPanel(UIPanelDef panelId)
        {
            if (activePanelDict.ContainsKey(panelId))
                return activePanelDict[panelId];

            if (cachePanelDict.ContainsKey(panelId))
                return cachePanelDict[panelId];

            if (!panelTypeDict.ContainsKey(panelId))
            {
                UILocate.Log.LogError("界面没有绑定>>>", panelId);
                return null;
            }

            UIPanelCnf panelCnf = UILocate.UI.GetPanelCnf(panelId);
            Type panelType = panelTypeDict[panelId];
            InternalUIPanel panel = (InternalUIPanel)ReflectionHelper.CreateInstance(panelType);
            return panel;
        }

        /// <summary>
        /// 创建界面节点
        /// </summary>
        private void CreatePanelTrans(UIPanelDef panelId,InternalUIPanel panel)
        {
            if (panel.transform != null)
                return;
            UIPanelCreater.CreateUIPanelTrans(panelId,panel);
        }

        #endregion

        #region 界面显示

        private void ExecuteShowPanel(UIPanelDef panelId,InternalUIPanel panel)
        {
            //放入Active
            if (!activePanelDict.ContainsKey(panelId))
                activePanelDict.Add(panelId, panel);

            //移除Cache
            if (cachePanelDict.ContainsKey(panelId))
                cachePanelDict.Remove(panelId);

            //尝试创建
            CreatePanelTrans(panelId,panel);

            //调用显示
            panel.Show();
        }

        #endregion

        #region 界面隐藏

        private InternalUIPanel ExecuteHidePanel(UIPanelDef panelId)
        {
            if (!activePanelDict.ContainsKey(panelId))
                return null; 
            InternalUIPanel panel = activePanelDict[panelId];
            activePanelDict.Remove(panelId);

            //移除Cache
            if (!cachePanelDict.ContainsKey(panelId))
                cachePanelDict.Add(panelId, panel);

            //调用显示
            panel.Hide();

            return panel;
        }

        #endregion

        #region 回收池

        public void PushInCache(UIPanelDef panelId, InternalUIPanel panel)
        {
            if (!activePanelDict.ContainsKey(panelId))
                activePanelDict.Remove(panelId);
            cachePanelDict.Add(panelId, panel);
        }

        public InternalUIPanel PopInCache(UIPanelDef panelId)
        {
            if (!cachePanelDict.ContainsKey(panelId))
                return null;
            InternalUIPanel panel = cachePanelDict[panelId];
            cachePanelDict.Remove(panelId);
            return panel;
        }

        #endregion

        #region 配置

        public UIPanelCnf GetPanelCnf(global::UIPanelDef pPanelId)
        {
            return LCConfig.Config.UIPanelCnf[pPanelId];
        }
        

        #endregion
    } 
}
