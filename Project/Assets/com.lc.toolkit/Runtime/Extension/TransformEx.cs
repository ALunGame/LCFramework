﻿using System.Collections;
using UnityEngine;

namespace LCToolkit
{
    public static class TransformEx
    {
        public static void SetActive(this Transform pTrans,string pPath, bool pActive)
        {
            if (pTrans == null)
                return;
            pTrans.gameObject.SetActive(pPath, pActive);
        }

        public static bool Find(this Transform pTrans, string pPath,out Transform pFindTrans)
        {
            pFindTrans = null;
            if (pTrans == null)
                return false;
            pFindTrans = pTrans.Find(pPath);
            if (pFindTrans == null)
                return false;
            return true;
        }

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