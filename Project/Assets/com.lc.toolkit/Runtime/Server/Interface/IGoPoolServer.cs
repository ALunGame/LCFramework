using UnityEngine;

namespace LCToolkit
{
    /// <summary>
    /// Gameobjet对象池
    /// </summary>
    public interface IGoPoolServer
    {
        /// <summary>
        /// 获得一个回收Gameobject
        /// </summary>
        /// <param name="name">预制体名</param>
        /// <returns></returns>
        GameObject Take(string name);

        /// <summary>
        /// 回收一个Gameobject
        /// </summary>
        /// <param name="go"></param>
        void Recycle(GameObject go);
    }
}