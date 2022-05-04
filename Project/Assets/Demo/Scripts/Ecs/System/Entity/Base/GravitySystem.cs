using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.System
{
    public class GravitySystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(PropertyCom), typeof(GravityCom), typeof(MoveCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            //PropertyCom propertyCom = GetCom<PropertyCom>(comList[0]);
            //GravityCom gravityCom = GetCom<GravityCom>(comList[1]);
            //MoveCom moveCom = GetCom<MoveCom>(comList[2]);

            //int yDir = gravityCom.Dir == GravityDir.Down ? -1 : 1;
            ////float yValue = yDir * propertyCom.Mass.Curr * 0.98f * Time.deltaTime*3;
            //float yValue = yDir * propertyCom.Mass.Curr * 0.98f * Time.deltaTime;
            //moveCom.Rig.velocity = new Vector2(moveCom.Rig.velocity.x, moveCom.Rig.velocity.y + yValue);

            //Debug.LogError("重力改变" + moveCom.Rig.velocity);

        }
    } 
}
