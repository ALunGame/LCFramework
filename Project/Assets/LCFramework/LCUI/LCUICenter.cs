using LCHelp;
using System.Collections.Generic;
using UnityEngine;

namespace LCUI
{
    public class LCUICenter
    {
        //界面配置
        private Dictionary<UIPanelId, UIPanelInfo> PanelConfDict = new Dictionary<UIPanelId, UIPanelInfo>();
        //层级根节点
        private Dictionary<UILayer, RectTransform> LayerGoDict = new Dictionary<UILayer, RectTransform>();

        //缓存数量（超过销毁）
        private int CacheCnt = 5;
        //UI界面栈
        private Stack<UIPanelId> UIPanelStack = new Stack<UIPanelId>();
        //显示的UI界面
        private List<LCUIPanel> ActiveUIPanelList = new List<LCUIPanel>();
        //缓存池
        private List<LCUIPanel> CacheUIPanelList = new List<LCUIPanel>();

        #region Cache

        //将界面放入缓存
        private void PushUIPanelInCache(LCUIPanel panel)
        {
            if (CacheUIPanelList.Count>= CacheCnt)
            {
                ExecuteDestroyPanel(CacheUIPanelList[0]);
                CacheUIPanelList.RemoveAt(0);
            }
            CacheUIPanelList.Add(panel);
        }

        //从缓存中取出界面
        private LCUIPanel PopUIPanelInCache(UIPanelId panelId)
        {
            for (int i = 0; i < CacheUIPanelList.Count; i++)
            {
                if (CacheUIPanelList[i].PanelId==panelId)
                {
                    CacheUIPanelList.RemoveAt(i);
                    return CacheUIPanelList[i];
                }
            }
            return null;
        }

        #endregion

        #region Execute
        private void ExecuteShowPanel(LCUIPanel panel, params object[] param)
        {
            if (panel == null)
                return;
            panel.gameObject.SetActive(true);
            panel.OnShow(param);
        }

        private void ExecuteHidePanel(LCUIPanel panel)
        {
            if (panel == null)
                return;
            panel.gameObject.SetActive(false);
            panel.OnHide();
        }

        private void ExecuteDestroyPanel(LCUIPanel panel)
        {
            if (panel == null)
                return;
            UnityEngine.Object.Destroy(panel.gameObject);
        }

        #endregion

        #region Create

        private LCUIPanel CreateUIPanel(UIPanelId panelId)
        {
            if (!PanelConfDict.ContainsKey(panelId))
            {
                LCHelp.LCLog.LogError("没有对应的配置>>>>>", panelId);
                return null;
            }
            UIPanelInfo info = PanelConfDict[panelId];
            if (!LayerGoDict.ContainsKey(info.Layer))
            {
                LCHelp.LCLog.LogError("没有对应的层级节点>>>>>", info.Layer);
                return null;
            }
            GameObject uiGo  = GameObject.Instantiate(info.PanelRect.gameObject);
            uiGo.transform.SetParent(LayerGoDict[info.Layer],false);
            uiGo.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            uiGo.GetComponent<RectTransform>().localScale = Vector3.one;
            uiGo.name = string.Format("{0}({1})", uiGo.name, panelId);

            if (uiGo.GetComponent<LCUIPanel>()==null)
            {
                LCHelp.LCLog.LogError("界面没有 LCUIPanel 组件>>>>>", panelId);
                GameObject.Destroy(uiGo);
                return null;
            }

            LCUIPanel uiPanel = uiGo.GetComponent<LCUIPanel>();
            uiPanel.PanelId   = panelId;
            uiPanel.Canvas    = LayerGoDict[info.Layer].parent.GetComponent<RectTransform>();
            uiPanel.UiCamera  = LayerGoDict[info.Layer].parent.parent.GetComponent<Camera>();
            uiPanel.OnAwake();
            return uiPanel;
        }

        #endregion

        #region Check

        private bool CheckUIPanelIsActive(UIPanelId panelId)
        {
            for (int i = 0; i < ActiveUIPanelList.Count; i++)
            {
                if (ActiveUIPanelList[i].PanelId == panelId)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Get

        private LCUIPanel GetUIPanel(UIPanelId panelId)
        {
            for (int i = 0; i < ActiveUIPanelList.Count; i++)
            {
                if (ActiveUIPanelList[i].PanelId==panelId)
                {
                    return ActiveUIPanelList[i];
                }
            }

            LCUIPanel panel = PopUIPanelInCache(panelId);
            if (panel==null)
            {
                panel = CreateUIPanel(panelId);
            }
            return panel;
        }

        private LCUIPanel GetActivePanel(UIPanelId panelId)
        {
            for (int i = 0; i < ActiveUIPanelList.Count; i++)
            {
                if (ActiveUIPanelList[i].PanelId == panelId)
                {
                    return ActiveUIPanelList[i];
                }
            }
            return null;
        }

        #endregion

        #region Remove

        private void RemoveActivePanel(UIPanelId panelId)
        {
            for (int i = 0; i < ActiveUIPanelList.Count; i++)
            {
                if (ActiveUIPanelList[i].PanelId==panelId)
                {
                    ActiveUIPanelList.RemoveAt(i);
                }
            }
        }

        #endregion

        private void HideOtherUIPanel()
        {
            for (int i = 0; i < ActiveUIPanelList.Count; i++)
            {
                ExecuteHidePanel(ActiveUIPanelList[i]);
                ActiveUIPanelList.RemoveAt(i);
            }
        }

        #region Public

        public void Init(Dictionary<UIPanelId, UIPanelInfo> panelConfDict, Dictionary<UILayer, RectTransform> layerGoDict)
        {
            PanelConfDict = panelConfDict;
            LayerGoDict = layerGoDict;
        }

        public void Clear()
        {

        }

        public void ShowUI(UIPanelId panelId, UIShowMode showMode,params object[] parm)
        {
            LCUIPanel panel = GetUIPanel(panelId);
            if (panel==null)
                return;

            //隐藏所有界面
            if (showMode== UIShowMode.HideOther || showMode== UIShowMode.NoNeedBack_HideOther)
            {
                HideOtherUIPanel();
            }
            //隐藏上一个界面（啥都不做就不隐藏）
            else if (showMode != UIShowMode.DoNothing)
            {
                if (UIPanelStack.Count>0)
                {
                    UIPanelId lastPanelId = UIPanelStack.Peek();
                    LCUIPanel lastPanel   = GetActivePanel(lastPanelId);
                    ExecuteHidePanel(lastPanel);
                    PushUIPanelInCache(lastPanel);
                    RemoveActivePanel(lastPanel.PanelId);
                }
            }

            //显示界面
            ExecuteShowPanel(panel, parm);

            //不需要返回的就不用加在栈中
            if (showMode!= UIShowMode.NoNeedBack && showMode!= UIShowMode.NoNeedBack_HideOther)
            {
                UIPanelStack.Push(panel.PanelId);
            }

            //没有在打开列表
            if (!CheckUIPanelIsActive(panelId))
            {
                ActiveUIPanelList.Add(panel);
            }
        }

        public void HideUI(UIPanelId panelId)
        {
            if (!CheckUIPanelIsActive(panelId))
            {
                LCLog.Log("该界面没有显示中", panelId);
                return;
            }

            //先隐藏
            LCUIPanel hidePanel = GetActivePanel(panelId);
            ExecuteHidePanel(hidePanel);
            RemoveActivePanel(panelId);

            //判断是不是在栈顶
            if (UIPanelStack.Count<=0)
            {
                LCLog.LogR("UI界面栈中没有界面》》》》》》》");
                return;
            }

            //在栈顶就出栈
            if (UIPanelStack.Peek() == panelId)
            {
                UIPanelStack.Pop();
            }

            //下个界面显示
            if (UIPanelStack.Count <= 0)
            {
                PushUIPanelInCache(hidePanel);
                LCLog.LogR("UI界面栈中没有下个界面》》》》》》》");
                return;
            }
            UIPanelId nextPanelId = UIPanelStack.Peek();
            LCUIPanel nextPanel = GetUIPanel(nextPanelId);
            if (nextPanel==null)
            {
                LCLog.LogR("下个界面创建失败》》》》》》》", nextPanelId);
                return;
            }
            ExecuteShowPanel(nextPanel);
            //添加在显示界面中
            if (!CheckUIPanelIsActive(nextPanelId))
            {
                ActiveUIPanelList.Add(nextPanel);
            }

            //放入缓存
            PushUIPanelInCache(hidePanel);
        }

        //测试代码
        public T GetUIPanel<T>(UIPanelId panelId) where T:LCUIPanel
        {
            LCUIPanel panel = GetActivePanel(panelId);
            if (panel==null)
            {
                return default;
            }
            return (T)panel;
        }

        #endregion
    }
}
