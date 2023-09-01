using System.Collections.Generic;
using UnityEngine;

namespace LCToolkit
{
    public class GoPoolServer : IGoPoolServer
    {
        private Transform poolRoot;
        private Transform PoolRoot
        {
            get
            {
                if (poolRoot == null)
                {
                    poolRoot = new GameObject("<-----GoPool----->").transform;
                    poolRoot.position = new Vector3(9999, 9999, 0);
                }
                return poolRoot;
            }
        }

        class GoPoolItem
        {
            public int uid;
            public string name;

            public GoPoolItem(int uid, string name)
            {
                this.uid = uid;
                this.name = name;
            }
        }

        private Dictionary<string, Transform> cacheRootDict = new Dictionary<string, Transform>();

        //正在使用的
        private Dictionary<int, string> activeGoDict = new Dictionary<int, string>();

        //缓存的
        private Dictionary<string, List<GameObject>> cacheGoDict = new Dictionary<string, List<GameObject>>();

        public GameObject Take(string name)
        {
            GameObject takeGo = null;

            //找缓存
            if (cacheGoDict.ContainsKey(name))
            {
                List<GameObject> golist = cacheGoDict[name];
                if (golist.Count > 0)
                {
                    takeGo = golist[0];
                    golist.RemoveAt(0);
                }
            }

            //创建新的
            if (takeGo == null)
            {
                GameObject go = IAFramework.GameContext.Asset.LoadPrefab(name);
                if (go == null)
                {
                    ToolkitLocate.Log.LogError("没有找到对应资源>>>>", name);
                    return null;
                }
                takeGo = GameObject.Instantiate(go);
                Transform cacheRoot = GetCacheRootTrans(name);
                takeGo.transform.SetParent(cacheRoot);
                takeGo.transform.Reset();
            }

            activeGoDict.Add(takeGo.GetInstanceID(), name);
            takeGo.SetActive(true);
            return takeGo;
        }

        private Transform GetCacheRootTrans(string name)
        {
            if (cacheRootDict.ContainsKey(name))
                return cacheRootDict[name];
            string findName = $"<-----{name}----->";
            Transform cacheRoot = PoolRoot.Find(findName);
            if (cacheRoot == null)
            {
                cacheRoot = new GameObject(findName).transform;
                cacheRoot.SetParent(poolRoot);
                cacheRoot.Reset();
                cacheRootDict.Add(name, cacheRoot);
            }
            return cacheRoot;
        }

        public void Recycle(GameObject go)
        {
            if (!activeGoDict.ContainsKey(go.GetInstanceID()))
            {
                ToolkitLocate.Log.LogError("不是池中物>>>>", go.name);
                return;
            }

            string name = activeGoDict[go.GetInstanceID()];
            activeGoDict.Remove(go.GetInstanceID());

            Transform cacheRoot = GetCacheRootTrans(name);
            go.transform.SetParent(cacheRoot);
            go.transform.Reset();

            if (!cacheGoDict.ContainsKey(name))
                cacheGoDict.Add(name, new List<GameObject>());
            cacheGoDict[name].Add(go);
            go.SetActive(false);
        }
    }
}