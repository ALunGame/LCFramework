using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.System
{
    public class MoveSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(MoveCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            MoveCom moveCom = GetCom<MoveCom>(comList[0]);
            float xMoveDis = moveCom.Velocity.x * Time.deltaTime;
            float yMoveDis = moveCom.Velocity.y * Time.deltaTime;
            moveCom.Trans.position += new Vector3(xMoveDis, yMoveDis, 0);
        }
    }
}