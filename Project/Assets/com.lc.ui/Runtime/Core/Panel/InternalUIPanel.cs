using System.Collections.Generic;
using UnityEngine;
using LCToolkit;
using LCLoad;

namespace LCUI
{
    /// <summary>
    /// UI面板
    /// </summary>
    public class InternalUIPanel
    {
        /// <summary>
        /// 界面节点
        /// </summary>
        public Transform transform { get; private set; }

        #region Cache

        private List<UICacheItem> cacheItems = new List<UICacheItem>();

        /// <summary>
        /// 添加缓存对象，将会在界面关闭时自动回收
        /// </summary>
        /// <param name="cacheItem"></param>
        public void AddCacheItem(UICacheItem cacheItem)
        {
            if (!cacheItems.Contains(cacheItem))
                return;
            cacheItems.Add(cacheItem);
        }

        public void RecycleCaches()
        {
            for (int i = 0; i < cacheItems.Count; i++)
            {
                cacheItems[i].RecycleAll();
            }
        }

        #endregion

        #region Create

        public Transform CreatePanelTrans()
        {
            if (AttributeHelper.TryGetTypeAttribute<UIPanelAttribute>(this.GetType(),out var attr))
            {
                GameObject goAsset = LoadHelper.LoadPrefab(attr.UIPrefabName);
                if (goAsset == null)
                {
                    LCUI.UILocate.Log.LogError("创建界面出错，没有对应预制体", this.GetType(), attr.UIPrefabName);
                    return null;
                }
                GameObject uiPanel = GameObject.Instantiate(goAsset);
                transform = uiPanel.transform;
                return transform;
            }
            else
            {
                LCUI.UILocate.Log.LogError("创建界面出错，必须声明 UIPanelAttribute 属性才能创建", this.GetType());
                return null;
            }
        }

        public Transform CreatePanelTrans(Transform panelTrans)
        {
            transform = panelTrans;
            return transform;
        }

        #endregion


        public void Clear()
        {
            RecycleCaches();
        }
    }
}