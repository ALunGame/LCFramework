using LCToolkit;
using System;
using System.Collections.Generic;

namespace LCUI
{
    public class UIServer : IUIServer
    {
        //��������͵�ӳ��
        private Dictionary<UIPanelId,Type> panelTypeDict = new Dictionary<UIPanelId,Type>();

        //�������
        private List<UIRule> rules = new List<UIRule>();

        //����Ľ���
        private Dictionary<UIPanelId, InternalUIPanel> activePanelDict = new Dictionary<UIPanelId, InternalUIPanel>();

        //���滺��
        private Dictionary<UIPanelId, InternalUIPanel> cachePanelDict = new Dictionary<UIPanelId, InternalUIPanel>();

        /// <summary>
        /// ���еļ������
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
                        UILocate.Log.LogError("������UIId�ظ�>>>", item, attr.PanelId);
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

        #region ��������

        /// <summary>
        /// ��ý���
        /// </summary>
        /// <param name="panelId">����Id</param>
        /// <returns></returns>
        private InternalUIPanel GetPanel(UIPanelId panelId)
        {
            if (activePanelDict.ContainsKey(panelId))
                return activePanelDict[panelId];

            if (cachePanelDict.ContainsKey(panelId))
                return cachePanelDict[panelId];

            if (!panelTypeDict.ContainsKey(panelId))
            {
                UILocate.Log.LogError("����û�а�>>>", panelId);
                return null;
            }

            Type panelType = panelTypeDict[panelId];
            InternalUIPanel panel = (InternalUIPanel)ReflectionHelper.CreateInstance(panelType);
            return panel;
        }

        /// <summary>
        /// ��������ڵ�
        /// </summary>
        private void CreatePanelTrans(InternalUIPanel panel)
        {
            if (panel.transform != null)
                return;
            UIPanelCreater.CreateUIPanelTrans(panel);
        }

        #endregion

        #region ������ʾ

        private void ExecuteShowPanel(UIPanelId panelId,InternalUIPanel panel)
        {
            //����Active
            if (!activePanelDict.ContainsKey(panelId))
                activePanelDict.Add(panelId, panel);

            //�Ƴ�Cache
            if (cachePanelDict.ContainsKey(panelId))
                cachePanelDict.Remove(panelId);

            //���Դ���
            CreatePanelTrans(panel);

            //������ʾ
            panel.Show();
        }

        #endregion

        #region ��������

        private InternalUIPanel ExecuteHidePanel(UIPanelId panelId)
        {
            if (!activePanelDict.ContainsKey(panelId))
                return null; 
            InternalUIPanel panel = activePanelDict[panelId];
            activePanelDict.Remove(panelId);

            //�Ƴ�Cache
            if (!cachePanelDict.ContainsKey(panelId))
                cachePanelDict.Add(panelId, panel);

            //������ʾ
            panel.Hide();

            return panel;
        }

        #endregion

        #region ���ճ�

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
