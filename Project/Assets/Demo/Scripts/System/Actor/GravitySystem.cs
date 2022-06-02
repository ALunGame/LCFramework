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
            return new List<Type>() {typeof(GravityCom), typeof(TransformCom), typeof(Collider2DCom)};
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            GravityCom gravityCom = GetCom<GravityCom>(comList[0]);
            TransformCom transCom = GetCom<TransformCom>(comList[1]);
            float yDelta = gravityCom.Mass * _G * Time.deltaTime;
            transCom.ReqMove = new Vector3(transCom.ReqMove.x, transCom.ReqMove.y + yDelta);
        }
    } 
}
