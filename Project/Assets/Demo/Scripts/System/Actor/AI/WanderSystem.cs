using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.System
{
    public class WanderSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(WanderCom), typeof(TransformCom), typeof(Collider2DCom), typeof(PropertyCom),  };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            WanderCom wanderCom = GetCom<WanderCom>(comList[0]);
            TransformCom transCom = GetCom<TransformCom>(comList[1]);
            Collider2DCom collider2DCom = GetCom<Collider2DCom>(comList[2]);
            PropertyCom propertyCom = GetCom<PropertyCom>(comList[3]);

            wanderCom.WanderDir = CalcWanderMoveDir(wanderCom,collider2DCom);

            //更新方向
            transCom.ReqDir = wanderCom.WanderDir;

            //位移
            Vector3 delta = new Vector3(wanderCom.WanderDir == DirType.Right ? 1 : -1, 0, 0);
            delta = delta * propertyCom.MoveSpeed.Curr * Time.deltaTime;
            transCom.ReqMove = delta;
        }

        public DirType CalcWanderMoveDir(WanderCom wanderCom, Collider2DCom collider2DCom)
        {
            if (wanderCom.WanderDir == DirType.None)
            {
                return (DirType)UnityEngine.Random.Range((int)DirType.Left, (int)DirType.Right);
            }
            //不在地面
            if (!collider2DCom.Collider.Down)
            {
                return wanderCom.WanderDir == DirType.Left ? DirType.Right : DirType.Left;
            }
            if (collider2DCom.Collider.Left)
            {
                return DirType.Right;
            }
            if (collider2DCom.Collider.Right)
            {
                return DirType.Left;
            }
            return wanderCom.WanderDir;
        }
    }
}
