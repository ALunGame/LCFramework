using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LCToolkit;
using System;

namespace LCUI
{
    /// <summary>
    /// 缓存对象
    /// </summary>
    [Serializable]
    public class UICacheItem
    {
        //激活池
        private List<GameObject> activePool = new List<GameObject>();

        //缓存池
        private List<GameObject> cachePool = new List<GameObject>();

        [Header("缓存对象")]
        [SerializeField]
        private GameObject cacheItem;

        [Header("缓存根节点")]
        [SerializeField]
        private Transform cacheRoot;

        private GameObject CreateCache()
        {
            if (cacheItem == null)
                return null;
            GameObject item = GameObject.Instantiate(cacheItem);
            item.transform.SetParent(cacheRoot);
            item.transform.Reset();
            return item;
        }

        public GameObject Take()
        {
            GameObject cacheItem;
            if (cachePool.Count > 0)
            {
                cacheItem = cachePool[0];
                cachePool.RemoveAt(0);
            }
            else
            {
                cacheItem = CreateCache();
            }
            cacheItem.SetActive(true);
            activePool.Add(cacheItem);
            return cacheItem;
        }

        public void RecycleAll()
        {
            for (int i = 0; i < activePool.Count; i++)
            {
                GameObject cacheItem = activePool[i];
                cacheItem.SetActive(false);
                cachePool.Add(cacheItem);
            }
            activePool.Clear();
        }
    }
}