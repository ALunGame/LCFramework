#if UNITY_EDITOR
using System;
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// 演员配置资源
    /// </summary>
    [Serializable]
    public class ActorAsset
    {
        //配置Id
        public int actorId;
        //是否主角
        public bool isMainActor;
        //演员名
        public string actorName;
        //演员预制体
        public GameObject prefab;
    }
}
#endif