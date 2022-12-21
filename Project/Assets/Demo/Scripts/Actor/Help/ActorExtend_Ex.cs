using Demo.Com;
using LCMap;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public static partial class ActorExtend
    {
        public static int GetDirValue(this Actor pActor)
        {
            return pActor.Trans.GetDir() == DirType.Right ? 1 : -1;
        }

        public static DirType GetDir(this Actor pActor)
        {
            return pActor.Trans.GetDir();
        }

        #region Move

        // public static MoveInfo MoveToPos(this Actor pActor, Vector3 pTargetPos)
        // {
        //     return pActor.GetCom<MoveCom>().MoveToPos(pTargetPos);
        // }
        //
        // public static MoveInfo MoveToActor(this Actor pActor, Actor pTargetActor)
        // {
        //     return pActor.GetCom<MoveCom>().MoveToActor(pTargetActor);
        // }

        #endregion

        #region Get

        /// <summary>
        /// 获得最近的演员
        /// </summary>
        /// <param name="pActor"></param>
        /// <param name="pCheckActors"></param>
        /// <returns></returns>
        public static Actor GetNearestActor(this Actor pActor, List<Actor> pCheckActors)
        {
            if (pCheckActors == null || pCheckActors.Count <= 0)
            {
                return null;
            }

            float checkDis = float.MaxValue;
            Actor resActor = null;
            for (int i = 0; i < pCheckActors.Count; i++)
            {
                float tDis = Vector3.Distance(pActor.Pos, pCheckActors[i].Pos);
                if (tDis < checkDis)
                {
                    resActor = pCheckActors[i];
                    checkDis = tDis;
                }
            }

            return resActor;
        }

        #endregion
    }
}
