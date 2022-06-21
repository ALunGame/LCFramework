using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.System
{
    /// <summary>
    /// 徘徊系统
    /// </summary>
    public class WanderSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(WanderCom), typeof(TransformCom), typeof(Collider2DCom), typeof(BasePropertyCom),  };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            WanderCom wanderCom = GetCom<WanderCom>(comList[0]);
            TransformCom transCom = GetCom<TransformCom>(comList[1]);
            Collider2DCom collider2DCom = GetCom<Collider2DCom>(comList[2]);
            BasePropertyCom propertyCom = GetCom<BasePropertyCom>(comList[3]);

            wanderCom.WanderDir = CalcWanderMoveDir(transCom,wanderCom, collider2DCom);

            //更新方向
            transCom.ReqDir = wanderCom.WanderDir;

            //位移
            Vector3 delta = new Vector3(wanderCom.WanderDir == DirType.Right ? 1 : -1, 0, 0);
            delta = delta * propertyCom.MoveSpeed.Curr * Time.deltaTime;
            transCom.ReqMove = delta;
        }

        public DirType CalcWanderMoveDir(TransformCom transCom,WanderCom wanderCom, Collider2DCom collider2DCom)
        {
            //碰撞判断
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

            //范围判断
            if (wanderCom.WanderRange > 0)
            {
                Vector3 currPos = transCom.GetPos();
                if (Vector2.Distance(transCom.CreatePos, currPos) > wanderCom.WanderRange)
                {
                    if (currPos.x - transCom.CreatePos.x > 0)
                    {
                        return DirType.Left;
                    }
                    else
                    {
                        return DirType.Right;
                    }
                }
            }

            return wanderCom.WanderDir;
        }
    }
}
