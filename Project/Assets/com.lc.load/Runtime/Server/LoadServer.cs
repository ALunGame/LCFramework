using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace LCLoad
{
    public class LoadServer : ILoadServer
    {
        private Dictionary<string, AssetRequest> requestCache = new Dictionary<string, AssetRequest>();
        
         

        public T LoadSync<T>(string assetName) where T : Object
        {
            if (requestCache.ContainsKey(assetName))
            {
                AssetRequest request = requestCache[assetName];
                return request.GetObj<T>();
            }
            BundleAssetRequest newRequest = new BundleAssetRequest(assetName);
            newRequest.Load<T>(null);
            if (!requestCache.ContainsKey(assetName))
                requestCache.Add(assetName, newRequest);
            return newRequest.GetObj<T>();
        }

        public void LoadAsyn<T>(string assetName, Action<T> onComplete) where T : Object
        {
            if (requestCache.ContainsKey(assetName))
            {
                AssetRequest request = requestCache[assetName];
                onComplete?.Invoke(request.GetObj<T>());
                return;
            }
            BundleAssetRequestAsync newRequest = new BundleAssetRequestAsync(assetName);
            newRequest.Load<T>((T obj) => {
                if (!requestCache.ContainsKey(assetName))
                    requestCache.Add(assetName, newRequest);
                onComplete?.Invoke((T)obj);
            });
        }

        public AsyncOperationHandle<SceneInstance> LoadScene(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
        {
            return Addressables.LoadSceneAsync(sceneName, loadMode, activateOnLoad, priority);
        }

        public AsyncOperationHandle<SceneInstance> UnloadScene(AsyncOperationHandle handle, bool autoReleaseHandle = true)
        {
            return Addressables.UnloadSceneAsync(handle, autoReleaseHandle);
        }
    }
}