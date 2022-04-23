using System;
using System.Collections.Generic;

namespace LCToolkit.Pool
{
    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool<T>
    {
        private int count;

        /// <summary>
        /// 对象数量
        /// </summary>
        public int Count { get => count; }

        /// <summary>
        /// 使用池
        /// </summary>
        private Dictionary<T,string> usePool = new Dictionary<T, string>();

        /// <summary>
        /// 缓存池
        /// </summary>
        private Dictionary<string, T> cachePool = new Dictionary<string, T>();

        public Func<string, T> onCreateObj;
        public Action<T> onRecycleObj;
        public Action<T> onReleaseObj;

        public ObjectPool(Func<string, T> createFunc, Action<T> onRecycleObj, Action<T> onReleaseObj)
        {
            this.onCreateObj = createFunc;
            this.onRecycleObj = onRecycleObj;
            this.onReleaseObj = onReleaseObj;
        }

        /// <summary>
        /// 获得对象
        /// </summary>
        /// <returns></returns>
        public T GetObj(string name)
        {
            if (cachePool.ContainsKey(name))
            {
                return cachePool[name];
            }
            T obj = onCreateObj(name);
            usePool.Add(obj, name);
            return obj;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <returns></returns>
        public void RecycleObj(T obj)
        {
            if (!usePool.ContainsKey(obj))
            {
                return;
            }
            string name = usePool[obj];
            usePool.Remove(obj);
            onRecycleObj(obj);
            cachePool.Add(name, obj);
        }

        /// <summary>
        /// 回收所有
        /// </summary>
        public void RecycleAll()
        {
            foreach (var item in usePool)
            {
                RecycleObj(item.Key);
            }
            usePool.Clear();
        }

        /// <summary>
        /// 释放所有
        /// </summary>
        public void Release()
        {
            foreach (var item in usePool)
            {
                onReleaseObj(item.Key);
            }
            usePool.Clear();

            foreach (var item in cachePool)
            {
                onReleaseObj(item.Value);
            }
            cachePool.Clear();
        }
    }
}
