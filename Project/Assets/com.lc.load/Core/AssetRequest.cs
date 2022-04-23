using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LCLoad
{
    public enum LoadState
    {
        Init,
        Loading,
        Loaded,
        Unload
    }

    /// <summary>
    /// һ����Դ��������
    /// </summary>
    public class AssetRequest
    {
        /// <summary>
        /// ����״̬
        /// </summary>
        protected LoadState loadState = LoadState.Init;

        /// <summary>
        /// ��Դ��
        /// </summary>
        protected string assetName = string.Empty;

        /// <summary>
        /// ���ؾ��
        /// </summary>
        protected AsyncOperationHandle handle;

        /// <summary>
        /// ���ü���
        /// </summary>
        protected int useCnt = 0;

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public virtual bool isDone
        {
            get { return loadState == LoadState.Loaded || loadState == LoadState.Unload; }
        }

        public T GetObj<T>() where T : UnityEngine.Object
        {
            return handle.Result as T;
        }

        public AssetRequest(string assetName)
        {
            this.loadState = LoadState.Init;
            this.assetName = assetName;
            this.useCnt    = 0;
        }
        
        /// <summary>
        /// ����
        /// </summary>
        internal virtual void Load<T>(Action<T> onComplete) where T : UnityEngine.Object
        {
            loadState = LoadState.Loaded;
        }

        /// <summary>
        /// ж��
        /// </summary>
        internal virtual void Unload()
        {
            Addressables.Release(handle);
            loadState = LoadState.Unload;
        }
    }

    public class BundleAssetRequest : AssetRequest
    {
        public BundleAssetRequest(string assetName) : base(assetName)
        {
        }

        internal override void Load<T>(Action<T> onComplete)
        {
            if (loadState == LoadState.Loaded)
            {
                onComplete?.Invoke(handle.Result as T);
                return;
            }
            loadState = LoadState.Loading;
            this.handle = Addressables.LoadAssetAsync<T>(assetName);
            this.handle.WaitForCompletion();
            loadState   = LoadState.Loaded;
            onComplete?.Invoke(handle.Result as T);
        }
    }

    public class BundleAssetRequestAsync : AssetRequest
    {
        /// <summary>
        /// ������ϻص�
        /// </summary>
        protected Action<AsyncOperationHandle> loadCompleteFunc;


        public BundleAssetRequestAsync(string assetName) : base(assetName)
        {
        }

        internal override void Load<T>(Action<T> onComplete)
        {
            if (loadState == LoadState.Loading)
            {
                if (handle.IsDone)
                {
                    if (loadCompleteFunc != null)
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
                        }
                    };
                }
            }
            else
            {
                this.loadState = LoadState.Loading;
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
                    }
                    this.loadState = LoadState.Loaded;
                };
            }
        }
    }
}

