using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.System
{
    public class GravitySystem : BaseSystem
    {
        public const float _G = -0.98f;

        protected override List<Type> RegListenComs()
        {
            return new List<Type>() {typeof(GravityCom), typeof(TransCom), typeof(Collider2DCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            GravityCom gravityCom = GetCom<GravityCom>(comList[0]);
            TransCom transCom = GetCom<TransCom>(comList[1]);
            
            Vector3 waitPos;
            transCom.HasWaitPos(out waitPos);

            float yDelta = gravityCom.Mass * _G * Time.deltaTime;
            transCom.WaitSetPos(new Vector3(waitPos.x, waitPos.y + yDelta));
        }
    } 
}
