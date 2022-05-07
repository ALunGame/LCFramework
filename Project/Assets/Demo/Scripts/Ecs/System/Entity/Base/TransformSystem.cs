using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Demo.System
{
    public class TransformSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(TransformCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            TransformCom transCom = GetCom<TransformCom>(comList[0]);

            DirType backDir = transCom.ForwardDir == DirType.Right ? DirType.Left : DirType.Right;
            DirType dirType = DirType.Right;
            if (transCom.ReqDir == DirType.Right)
                dirType = transCom.ForwardDir;
            else
                dirType = backDir;

            if (dirType == transCom.CurrDir)
                return;

            transCom.CurrDir = dirType;
            Vector3 dir = new Vector3(0, transCom.CurrDir == DirType.Right ? 0 : 180, 0);
            transCom.DisplayRootTrans.localEulerAngles = dir;
        }
    }
}
