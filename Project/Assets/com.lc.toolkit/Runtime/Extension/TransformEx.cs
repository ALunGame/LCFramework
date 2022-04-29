using System.Collections;
using UnityEngine;

namespace LCToolkit
{
    public static class TransformEx
    {
        /// <summary>
        /// 重置位置,旋转,缩放,
        /// </summary>
        /// <param name="trans"></param>
        public static void Reset(this Transform trans)
        {
            trans.transform.localPosition = Vector3.zero;
            trans.transform.localRotation = Quaternion.identity;
            trans.transform.localScale = Vector3.one;
        }
    }
}