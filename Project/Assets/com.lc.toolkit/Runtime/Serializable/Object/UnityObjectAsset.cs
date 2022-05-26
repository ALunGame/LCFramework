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

        public AssetType assetType = AssetType.GameObj;
        public string ObjName = "";
        public string ObjPath = "";

        public UnityObjectAsset()
        {

        }

        public UnityObjectAsset(AssetType assetType)
        {
            this.assetType = assetType;
        }

#if UNITY_EDITOR

        public enum AssetType
        {
            GameObj,
            AnimClip,
        }

        public UnityObject GetObj()
        {
            if (string.IsNullOrEmpty(ObjPath))
                return null;
            return AssetDatabase.LoadAssetAtPath(ObjPath, GetObjType());
        }

        public Type GetObjType()
        {
            Type objType = typeof(GameObject);
            switch (assetType)
            {
                case AssetType.GameObj:
                    objType = typeof(GameObject);
                    break;
                case AssetType.AnimClip:
                    objType = typeof(AnimationClip);
                    break;
                default:
                    break;
            }
            return objType;
        }

#endif
    }
}