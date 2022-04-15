using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.Initialization;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace LCLoad
{
    class BaseLoader
    {
        /// <summary>
        /// 资源名
        /// </summary>
        protected string assetName = string.Empty;

        /// <summary>
        /// 加载句柄
        /// </summary>
        private AsyncOperationHandle handle;

        /// <summary>
        /// 是否加载完成
        /// </summary>
        private bool isLoad = false;

        public BaseLoader(string assetName)
        {
            this.assetName = assetName;
            this.isLoad = false;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="onComplete">加载完成之后的回调</param>
        public virtual void Load<T>(Action<T> onComplete) where T : UnityEngine.Object
        {
            if (this.isLoad)
            {
                if (handle.IsDone)
                {
                    if (onComplete != null)
                    {
                        onComplete(handle.Result as T);
                    }
                }
                else
                {
                    handle.Completed += (result) =>
                    {
                        if (result.Status == AsyncOperationStatus.Succeeded)
                        {
                            var obj = result.Result as T;
                            if (onComplete != null)
                            {
                                onComplete(obj);
                            }
                        }
                        else
                        {
                            if (onComplete != null)
                            {
                                onComplete(null);
                            }
                            LoadLocate.Log.LogError("Load name = " + assetName + " tpye = " + typeof(T).ToString() + " failed!  ");
                        }
                    };
                }
            }
            else
            {
                this.isLoad = true;
                this.handle = Addressables.LoadAssetAsync<T>(assetName);
                handle.Completed += (result) =>
                {
                    if (result.Status == AsyncOperationStatus.Succeeded)
                    {
                        var obj = result.Result as T;
                        if (onComplete != null)
                        {
                            onComplete(obj);
                        }
                    }
                    else
                    {
                        if (onComplete != null)
                        {
                            onComplete(null);
                        }
                        LoadLocate.Log.LogError("Load name = " + assetName + " tpye = " + typeof(T).ToString() + " failed!  ");
                    }
                };
            }
        }

        /// <summary>
        /// 同步方法加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Load<T>() where T : UnityEngine.Object
        {
            this.isLoad = true;
            this.handle = Addressables.LoadAssetAsync<T>(assetName);
            T obj = this.handle.WaitForCompletion() as T;
            this.isLoad = false;
            return obj;
        }

        /// <summary>
        /// 同时加载多个资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public virtual List<T> Loads<T>() where T : UnityEngine.Object
        {
            this.isLoad = true;
            this.handle = Addressables.LoadAssetsAsync<T>(assetName, (obj) => { });
            List<T> objs = this.handle.WaitForCompletion() as List<T>;
            return objs;
        }

        public virtual void Release()
        {
            if (this.isLoad)
            {
                this.isLoad = false;
                Addressables.Release(handle);
            }
        }
    } 
}

