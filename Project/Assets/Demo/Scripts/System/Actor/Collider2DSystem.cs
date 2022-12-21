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

        protected override List<Type> RegContainListenComs()
        {
            return new List<Type>() { typeof(Collider2DCom)};
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            Collider2DCom collider2DCom = GetCom<Collider2DCom>(comList[0]);
            // HandleBoxCollider(collider2DCom);
            // HandleCornerCollider(collider2DCom);
        }

        //碰撞处理
        private void HandleBoxCollider(Collider2DCom collider2DCom)
        {
            Vector2 pos = collider2DCom.GetColliderOffset();
            collider2DCom.Collider.Up       = CheckCollider(collider2DCom.UpCheckInfo.centerPos+pos,collider2DCom.UpCheckInfo.size);
            collider2DCom.Collider.Down     = CheckCollider(collider2DCom.DownCheckInfo.centerPos+pos,collider2DCom.DownCheckInfo.size);
            collider2DCom.Collider.Left     = CheckCollider(collider2DCom.LeftCheckInfo.centerPos+pos,collider2DCom.LeftCheckInfo.size);
            collider2DCom.Collider.Right    = CheckCollider(collider2DCom.RightCheckInfo.centerPos+pos,collider2DCom.RightCheckInfo.size);
        }

        //拐角碰撞检测
        private void HandleCornerCollider(Collider2DCom collider2DCom)
        {
            Vector2 pos = collider2DCom.GetColliderOffset();
            collider2DCom.Collider.UpRightCorner    = CheckCollider(collider2DCom.UpRightCornerCheckInfo.centerPos+pos,collider2DCom.UpRightCornerCheckInfo.size);
            collider2DCom.Collider.UpLeftCorner     = CheckCollider(collider2DCom.UpLeftCornerCheckInfo.centerPos+pos,collider2DCom.UpLeftCornerCheckInfo.size);
            collider2DCom.Collider.DownRightCorner  = CheckCollider(collider2DCom.DownRightCornerCheckInfo.centerPos+pos,collider2DCom.DownRightCornerCheckInfo.size);
            collider2DCom.Collider.DownLeftCorner   = CheckCollider(collider2DCom.DownLeftCornerCheckInfo.centerPos+pos,collider2DCom.DownLeftCornerCheckInfo.size);
        }

        private bool CheckCollider(Vector2 pCenter,Vector2 pSize)
        {
            Collider2D[] results = new Collider2D[4];
            int colliderCnt = Physics2D.OverlapBoxNonAlloc(pCenter, pSize, 0, results, LayerMask.GetMask("Map"));
            return colliderCnt > 0;
        }
    }
}