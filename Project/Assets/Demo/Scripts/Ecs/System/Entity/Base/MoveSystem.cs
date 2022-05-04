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
            return new List<Type>() { typeof(PropertyCom), typeof(MoveCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            PropertyCom propertyCom = GetCom<PropertyCom>(comList[0]);
            MoveCom moveCom = GetCom<MoveCom>(comList[1]);

            if (moveCom.HasNoReqMove)
                return;

            //移动
            moveCom.Rig.velocity = new Vector2(moveCom.ReqMoveSpeed, moveCom.Rig.velocity.y);
            if (moveCom.ReqJumpSpeed != 0)
                moveCom.Rig.velocity = new Vector2(moveCom.Rig.velocity.x, moveCom.ReqJumpSpeed);

            HandleGravitySpeed(moveCom, propertyCom);
            Debug.LogFormat("MoveSystem>>>>>velocity：{0}", moveCom.Rig.velocity);
            if (moveCom.ReqJumpSpeed != 0)
            {
                Debug.LogFormat("MoveSystem>>>>>velocity：{0}", moveCom.Rig.velocity);
            }
        }

        private void HandleGravitySpeed(MoveCom moveCom, PropertyCom propertyCom)
        {
            if (moveCom.Rig.velocity.y < 0)
            {
                moveCom.Rig.velocity += Definition.Gravity * 1.5f * propertyCom.Mass * Time.deltaTime;
            }
            else if (moveCom.Rig.velocity.y > 0)
            {
                moveCom.Rig.velocity += Definition.Gravity * 1 * propertyCom.Mass * Time.deltaTime;
            }
        }
    }
}