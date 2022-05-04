using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.System
{
    public class Collider2DSystem : BaseSystem
    {
        private float CollisionRadius = 0.05f;

        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(Collider2DCom), typeof(MoveCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            Collider2DCom collider2DCom = GetCom<Collider2DCom>(comList[0]);
            MoveCom moveCom = GetCom<MoveCom>(comList[1]);
            HandleCollider(collider2DCom, moveCom);
        }

        //碰撞处理
        private void HandleCollider(Collider2DCom collider2DCom,MoveCom moveCom)
        {
            Vector2 pos = moveCom.Rig.position;
            collider2DCom.Collider.Up       = CheckCollider(pos + collider2DCom.UpCheckPoint);
            collider2DCom.Collider.Down     = CheckCollider(pos + collider2DCom.DownCheckPoint);
            collider2DCom.Collider.Left     = CheckCollider(pos + collider2DCom.LeftCheckPoint);
            collider2DCom.Collider.Right    = CheckCollider(pos + collider2DCom.RightCheckPoint);
        }

        private bool CheckCollider(Vector2 point)
        {
            Collider2D[] results = new Collider2D[4];
            int colliderCnt  = Physics2D.OverlapCircleNonAlloc(point, CollisionRadius, results, LayerMask.GetMask("Ground"));
            return colliderCnt > 0;
        }
    }
}