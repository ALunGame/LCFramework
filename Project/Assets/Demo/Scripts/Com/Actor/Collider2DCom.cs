using LCECS.Core;
using UnityEngine;
using LCJson;
using System;
using LCMap;
using LCToolkit;

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
        public const float CollisionOffset = 0.02f;

        [NonSerialized]
        public ColliderData Collider = ColliderData.Null;

        [NonSerialized]
        private BoxCollider2D collider2D;

        [NonSerialized]
        public Vector2 UpCheckPoint;

        [NonSerialized]
        public Vector2 DownCheckPoint;

        [NonSerialized]
        public Vector2 RightCheckPoint;

        [NonSerialized]
        public Vector2 LeftCheckPoint;

        [NonSerialized]
        public Transform trans;

        protected override void OnInit(GameObject go)
        {
            ActorObj actorObj = go.GetComponent<ActorObj>();
            trans = go.transform;
            actorObj.OnDisplayGoChange += OnDisplayGoChange;
            OnDisplayGoChange(actorObj);
        }

        private Vector2 GetColliderOffset(BoxCollider2D collider2D)
        {
            return new Vector2(collider2D.bounds.center.x, collider2D.bounds.center.y) - trans.position.ToVector2();
        }

        private void OnDisplayGoChange(ActorObj actorGo)
        {
            collider2D     = actorGo.GetBodyCollider();
            Vector2 offset = GetColliderOffset(collider2D);

            UpCheckPoint    = new Vector2(offset.x, offset.y + collider2D.bounds.extents.y + CollisionOffset);
            DownCheckPoint  = new Vector2(offset.x, offset.y - collider2D.bounds.extents.y - CollisionOffset);
            RightCheckPoint = new Vector2(offset.x + collider2D.bounds.extents.x + CollisionOffset, offset.y);
            LeftCheckPoint  = new Vector2(offset.x - collider2D.bounds.extents.x - CollisionOffset, offset.y);
        }

        public override void OnDrawGizmosSelected()
        {
            Vector2 pos = trans.position;

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(pos + UpCheckPoint, 0.05f);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pos + DownCheckPoint, 0.05f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(pos + LeftCheckPoint, 0.05f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pos + RightCheckPoint, 0.05f);
        }
    }
}