using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class MoveSystem : BaseSystem
    {
        protected override List<Type> RegContainListenComs()
        {
            return new List<Type>() { typeof(MoveCom), typeof(TransCom) };
        }

        protected override List<Type> RegNoContainComs()
        {
            return new List<Type>() { typeof(Collider2DCom)};
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            MoveCom moveCom = (MoveCom)comList[0];
            TransCom transCom = (TransCom)comList[1];
            
            
            float xPos = moveCom.CurrentMoveInfo.xDelta;
            float yPos = moveCom.CurrentMoveInfo.jumpSpeed * Time.deltaTime;

            if (xPos == 0 && yPos == 0)
            {
                return;
            }
            transCom.WaitSetPos(transCom.Pos + new Vector3(xPos,yPos));
            transCom.UpdateWaitTransData();
            if (xPos != 0)
            {
                transCom.Roate(xPos > 0 ? DirType.Right : DirType.Left);
            }
        }
    }
}
