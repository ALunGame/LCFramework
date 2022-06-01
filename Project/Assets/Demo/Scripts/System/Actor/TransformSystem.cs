using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.System
{
    public class TransformSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(TransformCom), typeof(Collider2DCom), typeof(AnimCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            TransformCom transCom = GetCom<TransformCom>(comList[0]);
            Collider2DCom collider2DCom = GetCom<Collider2DCom>(comList[1]);
            AnimCom animCom = GetCom<AnimCom>(comList[2]);

            HandleDir(transCom);
            HandleMove(transCom, collider2DCom, animCom);
            transCom.UpdatePos();
        }

        private void HandleDir(TransformCom transCom)
        {
            if (transCom.ReqDir == DirType.None)
                return;
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
            transCom.DisplayTrans.localEulerAngles = dir;

            transCom.ReqDir = DirType.None;
        }

        private void HandleMove(TransformCom transCom, Collider2DCom collider2DCom, AnimCom animCom)
        {

            if (transCom.ReqMove.x != 0)
            {
                if (collider2DCom.Collider.Left && transCom.ReqMove.x < 0)
                    transCom.ReqMove.x = 0;
                if (collider2DCom.Collider.Right && transCom.ReqMove.x > 0)
                    transCom.ReqMove.x = 0;
            }
            if (transCom.ReqMove.y != 0)
            {
                if (collider2DCom.Collider.Up && transCom.ReqMove.y > 0)
                    transCom.ReqMove.y = 0;
                if (collider2DCom.Collider.Down && transCom.ReqMove.y < 0)
                    transCom.ReqMove.y = 0;
            }

            if (transCom.ReqMove == Vector3.zero)
                return;

            transCom.Translate(transCom.ReqMove);
            animCom.SetReqAnim("run");

            transCom.ReqMove = Vector3.zero;
        }
    }
}
