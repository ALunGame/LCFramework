using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LCLoad
{
    public static class LoadHelper
    {
        /// <summary>
        /// 创建预制体
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static GameObject CreateGo(string assetName)
        {
            GameObject goAsset = LoadPrefab(assetName);
            if (goAsset == null)
            {
                LCLoad.LoadLocate.Log.LogError("创建预制体失败，没有对应资源",assetName);
                return null;
            }

            return GameObject.Instantiate(goAsset);
        }
        
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
        /// 加载资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static T Load<T>(string assetName) where T : UnityEngine.Object
        {
            return LoadLocate.Load.LoadSync<T>(assetName);
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
            TextAsset textAsset = LoadLocate.Load.LoadSync<TextAsset>(assetName);
            if (textAsset == null)
                return "";
            return textAsset.text;
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
