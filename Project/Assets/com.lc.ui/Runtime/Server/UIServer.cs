using LCToolkit;
using System;
using System.Collections.Generic;

namespace LCUI
{
    public class UIServer : IUIServer
    {
        //界面和类型的映射
        private Dictionary<UIPanelId,Type> panelTypeDict = new Dictionary<UIPanelId,Type>();

        //界面规则
        private List<UIRule> rules = new List<UIRule>();

        //激活的界面
        private Dictionary<UIPanelId, InternalUIPanel> activePanelDict = new Dictionary<UIPanelId, InternalUIPanel>();

        //界面缓存
        private Dictionary<UIPanelId, InternalUIPanel> cachePanelDict = new Dictionary<UIPanelId, InternalUIPanel>();

        /// <summary>
        /// 所有的激活界面
        /// </summary>
        public IReadOnlyDictionary<UIPanelId, InternalUIPanel> ActivePanelDict { get => activePanelDict; }

        public void Init()
        {
            rules.Add(new UIStackRule(this));
            rules.Add(new UIHideOtherRule(this));
            foreach (var item in ReflectionHelper.GetChildTypes<InternalUIPanel>())
            {
                if (item.IsAbstract)
                    continue;
                if (AttributeHelper.TryGetTypeAttribute<UIPanelIdAttribute>(item,out var attr))
                {
                    if (panelTypeDict.ContainsKey(attr.PanelId))
                    {
                        UILocate.Log.LogError("声明的UIId重复>>>", item, attr.PanelId);
                        continue;
                    }
                    panelTypeDict.Add(attr.PanelId, item);
                }
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

        public T GetPanelModel<T>(UIPanelId panelId) where T : UIModel
        {
            InternalUIPanel panel = GetPanel(panelId);

            if (!activePanelDict.ContainsKey(panelId) && !cachePanelDict.ContainsKey(panelId))
            {
                cachePanelDict.Add(panelId, panel);
            }

            return (T)panel.Model;
        }

        public void Show(UIPanelId panelId)
        {
            InternalUIPanel panel = GetPanel(panelId);
            foreach (var item in rules)
            {
                item.ShowPanel(panelId, panel);
            }
            ExecuteShowPanel(panelId, panel);
        }

        public void Hide(UIPanelId panelId)
        {
            InternalUIPanel panel = ExecuteHidePanel(panelId);
            foreach (var item in rules)
            {
                item.HidePanel(panelId, panel);
            }
        }

        public void HideAllActivePanel()
        {
            foreach (UIPanelId paneId in new List<UIPanelId>(activePanelDict.Keys))
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
        private InternalUIPanel GetPanel(UIPanelId panelId)
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

            Type panelType = panelTypeDict[panelId];
            InternalUIPanel panel = (InternalUIPanel)ReflectionHelper.CreateInstance(panelType);
            return panel;
        }

        /// <summary>
        /// 创建界面节点
        /// </summary>
        private void CreatePanelTrans(InternalUIPanel panel)
        {
            if (panel.transform != null)
                return;
            UIPanelCreater.CreateUIPanelTrans(panel);
        }

        #endregion

        #region 界面显示

        private void ExecuteShowPanel(UIPanelId panelId,InternalUIPanel panel)
        {
            //放入Active
            if (!activePanelDict.ContainsKey(panelId))
                activePanelDict.Add(panelId, panel);

            //移除Cache
            if (cachePanelDict.ContainsKey(panelId))
                cachePanelDict.Remove(panelId);

            //尝试创建
            CreatePanelTrans(panel);

            //调用显示
            panel.Show();
        }

        #endregion

        #region 界面隐藏

        private InternalUIPanel ExecuteHidePanel(UIPanelId panelId)
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

        public void PushInCache(UIPanelId panelId, InternalUIPanel panel)
        {
            if (!activePanelDict.ContainsKey(panelId))
                activePanelDict.Remove(panelId);
            cachePanelDict.Add(panelId, panel);
        }

        public InternalUIPanel PopInCache(UIPanelId panelId)
        {
            if (!cachePanelDict.ContainsKey(panelId))
                return null;
            InternalUIPanel panel = cachePanelDict[panelId];
            cachePanelDict.Remove(panelId);
            return panel;
        }

        #endregion
    } 
}
