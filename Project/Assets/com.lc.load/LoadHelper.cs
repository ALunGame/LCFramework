using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LCLoad
{
    public static class LoadHelper
    {
        /// <summary>
        /// 预制体
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static GameObject LoadPrefab(string assetName)
        {
            return LoadLocate.Load.LoadSync<GameObject>(assetName);
        }

        /// <summary>
        /// 加载贴图
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static Texture2D LoadTexture2D(string assetName)
        {
            return LoadLocate.Load.LoadSync<Texture2D>(assetName);
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static Sprite LoadSprite(string assetName)
        {
            return LoadLocate.Load.LoadSync<Sprite>(assetName);
        }

        /// <summary>
        /// 二进制
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static byte[] LoadBytes(string assetName)
        {
            return LoadLocate.Load.LoadSync<TextAsset>(assetName).bytes;
        }

        /// <summary>
        /// 字符串
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string LoadString(string assetName)
        {
            return LoadLocate.Load.LoadSync<TextAsset>(assetName).text;
        }

        /// <summary>
        /// 场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="loadMode"></param>
        /// <param name="activateOnLoad"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static AsyncOperationHandle<SceneInstance> LoadScene(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
        {
            return LoadLocate.Load.LoadScene(sceneName, loadMode, activateOnLoad, priority);
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="autoReleaseHandle"></param>
        /// <returns></returns>
        public static AsyncOperationHandle<SceneInstance> UnloadScene(AsyncOperationHandle handle, bool autoReleaseHandle = true)
        {
            return LoadLocate.Load.UnloadScene(handle, autoReleaseHandle);
        }
    }
}
