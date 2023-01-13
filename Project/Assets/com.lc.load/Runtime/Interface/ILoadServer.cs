using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LCLoad
{
    public interface ILoadServer
    {
        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="assetName"></param>
        T LoadSync<T>(string assetName) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loader"></param>
        void LoadAsyn<T>(string assetName, Action<T> onComplete) where T : UnityEngine.Object;

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="loadMode"></param>
        /// <param name="activateOnLoad"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        AsyncOperationHandle<SceneInstance> LoadScene(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100);

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="autoReleaseHandle"></param>
        /// <returns></returns>
        AsyncOperationHandle<SceneInstance> UnloadScene(AsyncOperationHandle handle, bool autoReleaseHandle = true);
    }
}
