using Demo.Com;
using LCMap;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public static partial class ActorExtend
    {


        #region Set

        public static void SetDir(this Actor pActor, DirType pDir)
        {
            int dirValue = pDir == DirType.Right ? 0 : 1;
            Vector3 roate = new Vector3(0, dirValue*180, 0);
            pActor.SetRoate(roate);
        }

        #endregion

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
        
        public static int GetDirValue(this Actor pActor)
        {
            return pActor.GetDir() == DirType.Right ? 1 : -1;
        }

        public static DirType GetDir(this Actor pActor)
        {
            DirType dirType = pActor.DisplayCom.DisplayGo.transform.localEulerAngles.y == 0 ? DirType.Right : DirType.Left;
            return dirType;
        }

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
