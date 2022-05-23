using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityObject = UnityEngine.Object;
using LCJson;

#if UNITY_EDITOR

using UnityEditor.SceneManagement;
using UnityEditor;

#endif

namespace LCToolkit
{
    /// <summary>
    /// 需要序列化的资源
    /// </summary>
    public class UnityObjectAsset
    {
        [JsonIgnore]
        public UnityObject Obj;

        public string ObjName;
        public string ObjPath;

#if UNITY_EDITOR

        public UnityObject GetObj(Type objType)
        {
            if (string.IsNullOrEmpty(ObjPath))
                return null;
            return AssetDatabase.LoadAssetAtPath(ObjPath, objType);
        }

#endif
    }
}