using LCECS.Core;
using UnityEngine;
using LCJson;
using System;

namespace Demo.Com
{
    public class ColliderData
    {
        public bool Up      = false;
        public bool Down    = false;
        public bool Left    = false;
        public bool Right   = false;

        public bool IsNull()
        {
            return !Up && !Down && !Left && !Right;
        }

        public static ColliderData Null
        {
            get { return new ColliderData(); }
        }

        public override string ToString()
        {
            return $"Up:{Up} Down:{Down} Left:{Left} Right:{Right}";
        }
    }


    /// <summary>
    /// 碰撞组件
    /// </summary>
    [Serializable]
    public class Collider2DCom : BaseCom
    {
        [NonSerialized]
        public const float CollisionOffset = 0.05f;

        [NonSerialized]
        public ColliderData Collider = ColliderData.Null;

        private Collider2D collider2D;

        public Vector2 UpCheckPoint
        {
            get { return new Vector2(collider2D.bounds.center.x, collider2D.bounds.center.y + collider2D.bounds.extents.y + CollisionOffset); }
        }

        public Vector2 DownCheckPoint
        {
            get { return new Vector2(collider2D.bounds.center.x, collider2D.bounds.center.y - collider2D.bounds.extents.y - CollisionOffset); }
        }

        public Vector2 RightCheckPoint
        {
            get { return new Vector2(collider2D.bounds.center.x + collider2D.bounds.extents.x + CollisionOffset, collider2D.bounds.center.y); }
        }

        public Vector2 LeftCheckPoint
        {
            get { return new Vector2(collider2D.bounds.center.x - collider2D.bounds.extents.x - CollisionOffset, collider2D.bounds.center.y); }
        }

        protected override void OnInit(GameObject go)
        {
            collider2D = go.transform.Find("Display/Default/ColliderBox").GetComponent<Collider2D>();
        }
    }
}