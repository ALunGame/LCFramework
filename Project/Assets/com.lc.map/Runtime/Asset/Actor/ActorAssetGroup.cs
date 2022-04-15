#if UNITY_EDITOR
using LCToolkit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

using Object = UnityEngine.Object;

namespace LCMap
{
    /// <summary>
    /// 演员配置组
    /// </summary>
    [CreateAssetMenu(fileName = "地图演员组", menuName = "配置组/地图/演员组", order = 4)]
    public class ActorAssetGroup : ScriptableObject
    {
        public SerializableDictionary<int, ActorAsset> actorDict = new SerializableDictionary<int, ActorAsset>();
    }
} 
#endif
