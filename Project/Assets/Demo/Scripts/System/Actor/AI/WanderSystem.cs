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
            return new List<Type>() { typeof(WanderCom), typeof(TransCom), typeof(Collider2DCom), typeof(BasePropertyCom),  };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            WanderCom wanderCom = GetCom<WanderCom>(comList[0]);
            TransCom transCom = GetCom<TransCom>(comList[1]);
            Collider2DCom collider2DCom = GetCom<Collider2DCom>(comList[2]);
            BasePropertyCom propertyCom = GetCom<BasePropertyCom>(comList[3]);

            wanderCom.WanderDir = CalcWanderMoveDir(transCom,wanderCom, collider2DCom);

            //更新方向
            transCom.Roate(wanderCom.WanderDir);

            //位移
            transCom.MoveDir(wanderCom.WanderDir, propertyCom.MoveSpeed.Curr);
        }

        public DirType CalcWanderMoveDir(TransCom transCom,WanderCom wanderCom, Collider2DCom collider2DCom)
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
                Vector3 currPos = transCom.Pos;
                if (Vector2.Distance(transCom.InitPos, currPos) > wanderCom.WanderRange)
                {
                    if (currPos.x - transCom.InitPos.x > 0)
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
