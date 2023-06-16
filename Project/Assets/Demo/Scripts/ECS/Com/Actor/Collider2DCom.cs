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

        public bool UpRightCorner = false;
        public bool UpLeftCorner = false;
        public bool DownRightCorner = false;
        public bool DownLeftCorner = false;

        public bool IsNull { get { return !Up && !Down && !Left && !Right; } }

        public static ColliderData Null
        {
            get { return new ColliderData(); }
        }

        public override string ToString()
        {
            string box = $"Box-->Up:{Up} Down:{Down} Left:{Left} Right:{Right}";
            string corner = $" Corner-->UpRight:{UpRightCorner} UpLeft:{UpLeftCorner} DownRight:{DownRightCorner} DownLeft:{DownLeftCorner}";
            return box + corner;
        }
    }

    public class ColliderCheckInfo
    {
        public Vector2 centerPos;
        public Vector2 size;

        public ColliderCheckInfo()
        {
            
        }
        public ColliderCheckInfo(Vector2 pCenter,Vector2 pSize)
        {
            centerPos = pCenter;
            size = pSize;
        }
    }

    /// <summary>
    /// 碰撞组件
    /// </summary>
    [Serializable]
    public class Collider2DCom : BaseCom
    {
        [NonSerialized]
        public const float ColliderRadius = 0.2f;
        
        [NonSerialized]
        public const float CollisionOffset = 0.02f;

        [NonSerialized]
        public ColliderData Collider = ColliderData.Null;

        [NonSerialized]
        private BoxCollider2D collider2D;
        
        [NonSerialized]
        public Rigidbody2D rig2D;

        [NonSerialized]
        public Transform trans;

        [NonSerialized]
        public Shape colliderShape;

        
        [NonSerialized] public ColliderCheckInfo UpCheckInfo;
        [NonSerialized] public ColliderCheckInfo DownCheckInfo;
        [NonSerialized] public ColliderCheckInfo LeftCheckInfo;
        [NonSerialized] public ColliderCheckInfo RightCheckInfo;
        
        //上右拐角
        [NonSerialized] public ColliderCheckInfo UpRightCornerCheckInfo;
        //上左拐角
        [NonSerialized] public ColliderCheckInfo UpLeftCornerCheckInfo;
        //下右拐角
        [NonSerialized] public ColliderCheckInfo DownRightCornerCheckInfo;
        //下左拐角
        [NonSerialized] public ColliderCheckInfo DownLeftCornerCheckInfo;
        
        protected override void OnAwake(Entity pEntity)
        {
            BindGoCom bindGoCom = pEntity.GetCom<BindGoCom>();
            if (bindGoCom != null)
            {
                bindGoCom.RegGoChange(OnBindGoChange);
            }
            ActorDisplayCom displayCom = pEntity.GetCom<ActorDisplayCom>();
            if (displayCom != null)
            {
                displayCom.RegStateChange((stateName) =>
                {
                    OnDisplayGoChange(displayCom);
                });
            }
        }

        private void OnBindGoChange(GameObject pGo)
        {
            trans = pGo.transform;
            rig2D = pGo.GetComponent<Rigidbody2D>();
            UpdateCollider();
        }

        private void OnDisplayGoChange(ActorDisplayCom pDisplayCom)
        {
            collider2D = pDisplayCom.BodyCollider;
            UpdateCollider();
        }

        private void UpdateCollider()
        {
            if (collider2D == null || trans == null)
            {
                return;
            }
            Bounds bounds = collider2D.bounds;
            
            //盒子检测
            UpCheckInfo = new ColliderCheckInfo(new Vector2(bounds.extents.x, bounds.size.y - ColliderRadius / 2),
                new Vector2(bounds.size.x - ColliderRadius*2,ColliderRadius));
            DownCheckInfo = new ColliderCheckInfo(new Vector2(bounds.extents.x, ColliderRadius / 2),
                new Vector2(bounds.size.x - ColliderRadius*2,ColliderRadius));
            LeftCheckInfo = new ColliderCheckInfo(new Vector2(ColliderRadius / 2, collider2D.bounds.extents.y),
                new Vector2(ColliderRadius,bounds.size.y - ColliderRadius*2));
            RightCheckInfo = new ColliderCheckInfo(new Vector2(bounds.size.x - ColliderRadius / 2, collider2D.bounds.extents.y),
                new Vector2(ColliderRadius,bounds.size.y - ColliderRadius*2));
            
            //拐角检测
            UpRightCornerCheckInfo = new ColliderCheckInfo(
                new Vector2(bounds.size.x - ColliderRadius / 2,bounds.size.y - ColliderRadius / 2),
                new Vector2(ColliderRadius/2,ColliderRadius/2));
            UpLeftCornerCheckInfo = new ColliderCheckInfo(
                new Vector2(ColliderRadius / 2,bounds.size.y - ColliderRadius / 2),
                new Vector2(ColliderRadius/2,ColliderRadius/2));
            DownRightCornerCheckInfo = new ColliderCheckInfo(
                new Vector2(bounds.size.x - ColliderRadius / 2, ColliderRadius / 2),
                new Vector2(ColliderRadius/2,ColliderRadius/2));
            DownLeftCornerCheckInfo = new ColliderCheckInfo(
                new Vector2(ColliderRadius / 2, ColliderRadius / 2),
                new Vector2(ColliderRadius/2,ColliderRadius/2));

            colliderShape = new Shape();
            colliderShape.ShapeType = ShapeType.AABB;
            colliderShape.AABBMin = new Vector2(0, 0);
            colliderShape.AABBMax = new Vector2(collider2D.bounds.size.x, collider2D.bounds.size.y);
        }
        
        public Vector2 GetColliderOffset()
        {
            Bounds bounds = collider2D.bounds;
            return new Vector2(bounds.center.x-bounds.extents.x,bounds.center.y-bounds.extents.y);
        }

        private Vector2[] _cacheVects = new Vector2[4];
        public override void OnDrawGizmosSelected()
        {
            Vector2 pos = GetColliderOffset();
            
            GizmosHelper.DrawBounds(new Bounds(UpCheckInfo.centerPos+pos,UpCheckInfo.size),Color.blue);
            GizmosHelper.DrawBounds(new Bounds(DownCheckInfo.centerPos+pos,DownCheckInfo.size),Color.blue);
            GizmosHelper.DrawBounds(new Bounds(LeftCheckInfo.centerPos+pos,LeftCheckInfo.size),Color.blue);
            GizmosHelper.DrawBounds(new Bounds(RightCheckInfo.centerPos+pos,RightCheckInfo.size),Color.blue);

            GizmosHelper.DrawBounds(new Bounds(UpRightCornerCheckInfo.centerPos+pos,UpRightCornerCheckInfo.size),Color.red);
            GizmosHelper.DrawBounds(new Bounds(UpLeftCornerCheckInfo.centerPos+pos,UpLeftCornerCheckInfo.size),Color.red);
            GizmosHelper.DrawBounds(new Bounds(DownRightCornerCheckInfo.centerPos+pos,DownRightCornerCheckInfo.size),Color.red);
            GizmosHelper.DrawBounds(new Bounds(DownLeftCornerCheckInfo.centerPos+pos,DownLeftCornerCheckInfo.size),Color.red);
            
            // Gizmos.DrawSphere(DownCheckInfo.centerPos+pos,ColliderRadius);
            // Gizmos.DrawSphere(LeftCheckInfo.centerPos+pos,ColliderRadius);
            // Gizmos.DrawSphere(RightCheckInfo.centerPos+pos,ColliderRadius);
        }
    }
}