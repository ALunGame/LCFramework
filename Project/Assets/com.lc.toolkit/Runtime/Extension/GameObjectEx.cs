using System.Collections;
using UnityEngine;

namespace LCToolkit
{
    public static class GameObjectEx 
    {
        public static void SetActive(this GameObject pGo, bool pActive,string pPath = "")
        {
            if (pGo == null)
                return;
            if (!string.IsNullOrEmpty(pPath))
                pGo.SetActive(pActive);
            else
            {
                Transform findTrans = pGo.transform.Find(pPath);
                if (findTrans != null)
                    findTrans.gameObject.SetActive(pActive);
            }
        }
    }
}