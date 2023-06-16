using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LCECS;

namespace Demo
{
    public static partial class TransComExtend
    {
        public static DirType GetDir(this TransCom pTransCom)
        {
            return pTransCom.Roate.y == 0 ? DirType.Right : DirType.Left;
        }

        public static void MoveDir(this TransCom pTransCom, DirType pDir, float pSpeed)
        {
            Vector3 dir = new Vector3(pDir == DirType.Right ? 1 : -1, 0, 0);
            pTransCom.MoveDir(dir, pSpeed);
        }

        public static void Roate(this TransCom pTransCom, DirType pDir)
        {
            Vector3 roate = new Vector3(0, pDir == DirType.Right ? 0 : 180, 0);
            pTransCom.Roate(roate);
        }

        public static void SetDir(this TransCom pTransCom, DirType pDir)
        {
            Vector3 roate = new Vector3(0, pDir == DirType.Right ? 0 : 180, 0);
            pTransCom.SetRoate(roate);
        }
    }
}
