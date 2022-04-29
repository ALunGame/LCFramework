using LCECS.Core;
using UnityEngine;
using LCJson;
using System;

namespace Demo.Com
{
    /// <summary>
    /// 碰撞组件
    /// </summary>
    [Serializable]
    public class Collider2DCom : BaseCom
    {
        [NonSerialized]
        public const float CollisionOffset = 0.05f;

        public Vector2 UpCheckPoint     = Vector2.zero;
        public Vector2 BottomCheckPoint = Vector2.zero;
        public Vector2 RightCheckPoint  = Vector2.zero;
        public Vector2 LeftCheckPoint   = Vector2.zero;

        protected override void OnInit(GameObject go)
        {
            Collider2D collider2D = go.GetComponent<Collider2D>();
            UpCheckPoint = new Vector2(collider2D.bounds.center.x, collider2D.bounds.center.y + collider2D.bounds.extents.y + CollisionOffset);
            BottomCheckPoint = new Vector2(collider2D.bounds.center.x, collider2D.bounds.center.y - collider2D.bounds.extents.y - CollisionOffset);
            RightCheckPoint = new Vector2(collider2D.bounds.center.x + collider2D.bounds.extents.x + CollisionOffset, collider2D.bounds.center.y);
            LeftCheckPoint = new Vector2(collider2D.bounds.center.x - collider2D.bounds.extents.x - CollisionOffset, collider2D.bounds.center.y);
        }
    }
}