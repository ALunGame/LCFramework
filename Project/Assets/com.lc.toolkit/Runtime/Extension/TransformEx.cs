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

        /// <summary>
        /// 重置位置,旋转
        /// </summary>
        /// <param name="trans"></param>
        public static void ResetNoScale(this Transform trans)
        {
            trans.transform.localPosition = Vector3.zero;
            trans.transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// 重置位置,旋转
        /// </summary>
        /// <param name="trans"></param>
        public static T GetOrAddCom<T>(this Transform trans) where T : Component
        {
            if (trans.GetComponent<T>() != null)
            {
                return trans.GetComponent<T>();
            }
            else
            {
                return trans.gameObject.AddComponent<T>();
            }
        }
    }
}