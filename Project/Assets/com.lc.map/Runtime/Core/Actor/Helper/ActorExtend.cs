using System.Collections;
using UnityEngine;

namespace LCMap
{
    public static partial class ActorExtend
    {
        #region Set

        public static void SetBindGo(this Actor pActor, GameObject pGo)
        {
            pActor.BindGo.SetBindGo(pGo);
        }

        public static void SetState(this Actor pActor, string pStateName)
        {
            pActor.DisplayCom.SetState(pStateName);
        }

        public static void SetPos(this Actor pActor, Vector3 pPos)
        {
            pActor.Trans.SetPos(pPos);
        }

        public static void SetRoate(this Actor pActor, Vector3 pRoate)
        {
            if (pActor.DisplayCom.DisplayGo != null)
            {
                pActor.DisplayCom.DisplayGo.transform.localEulerAngles = pRoate;
            }
            pActor.Trans.SetRoate(pRoate,true);
        }

        public static void SetScale(this Actor pActor, Vector3 pScale)
        {
            pActor.Trans.SetScale(pScale);
        }

        #endregion

        #region Get

        public static GameObject GetStateGo(this Actor pActor)
        {
            return pActor.DisplayCom.StateGo;
        }

        #endregion
    }
}