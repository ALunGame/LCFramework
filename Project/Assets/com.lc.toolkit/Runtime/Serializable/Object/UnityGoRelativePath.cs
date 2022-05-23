using LCJson;
using UnityEngine;


namespace LCToolkit
{
    /// <summary>
    /// 资源相对坐标
    /// </summary>
    public class UnityGoRelativePath
    {
        //根节点
        [JsonIgnore]
        public GameObject RootGo;

        [JsonIgnore]
        public GameObject Go;

        //相对路径
        public string RelativePath;

        public UnityGoRelativePath(GameObject rootGo)
        {
            RootGo = rootGo;
        }

#if UNITY_EDITOR

        public GameObject GetObj()
        {
            if (string.IsNullOrEmpty(RelativePath) || RootGo == null)
                return null;

            Transform tmpObj = RootGo.transform.Find(RelativePath);
            return tmpObj.gameObject;
        }

#endif
    }
}