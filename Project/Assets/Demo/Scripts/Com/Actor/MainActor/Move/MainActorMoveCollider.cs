using LCToolkit;
using UnityEngine;

namespace Demo.Com.MainActor
{
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
    /// 主角移动物理检测
    /// </summary>
    public class MainActorMoveCollider
    {
        public const float ColliderRadius = 0.2f;
        
        private int checkLayerMask;           
        private BoxCollider2D collider2D;
        
        public ColliderCheckInfo UpCheckInfo    = new ColliderCheckInfo();
        public ColliderCheckInfo DownCheckInfo  = new ColliderCheckInfo();
        public ColliderCheckInfo LeftCheckInfo  = new ColliderCheckInfo();
        public ColliderCheckInfo RightCheckInfo = new ColliderCheckInfo();
        
        public RaycastHit2D DownBox { get; private set; }
        public RaycastHit2D[] UpBox { get; private set; }
        public RaycastHit2D[] RightBox{ get; private set; }
        public RaycastHit2D[] LeftBox { get; private set; }
        public RaycastHit2D[] HorizontalBox { get; private set; }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pCollider2D"></param>
        /// <param name="pCheckLayerName"></param>
        public void Init(BoxCollider2D pCollider2D,string pCheckLayerName)
        {
            collider2D = pCollider2D;
            checkLayerMask = LayerMask.GetMask(pCheckLayerName);
            
            Bounds bounds  = collider2D.bounds;
            //盒子检测
            UpCheckInfo = new ColliderCheckInfo(new Vector2(bounds.extents.x, bounds.size.y - ColliderRadius / 2),
                new Vector2(bounds.size.x - ColliderRadius*2,ColliderRadius));
            DownCheckInfo = new ColliderCheckInfo(new Vector2(bounds.extents.x, ColliderRadius / 2),
                new Vector2(bounds.size.x - ColliderRadius*2,ColliderRadius));
            LeftCheckInfo = new ColliderCheckInfo(new Vector2(ColliderRadius / 2, collider2D.bounds.extents.y),
                new Vector2(ColliderRadius,bounds.size.y - ColliderRadius*2));
            RightCheckInfo = new ColliderCheckInfo(new Vector2(bounds.size.x - ColliderRadius / 2, collider2D.bounds.extents.y),
                new Vector2(ColliderRadius,bounds.size.y - ColliderRadius*2));
        }

        /// <summary>
        /// 更新碰撞信息
        /// </summary>
        public void UpdateRaycastHitInfos()
        {
            Vector2 pos = GetColliderOffset();

            RightBox = Physics2D.BoxCastAll(RightCheckInfo.centerPos+pos, RightCheckInfo.size, 0, Vector3.right, 0.1f, checkLayerMask);
            LeftBox = Physics2D.BoxCastAll(LeftCheckInfo.centerPos+pos, LeftCheckInfo.size, 0, Vector3.left, 0.1f, checkLayerMask);
            UpBox = Physics2D.BoxCastAll(UpCheckInfo.centerPos+pos, UpCheckInfo.size, 0, Vector3.up, 0.05f, checkLayerMask);
            DownBox = Physics2D.BoxCast(DownCheckInfo.centerPos+pos, DownCheckInfo.size, 0, Vector3.down, 0.05f, checkLayerMask);
        }
        
        /// <summary>
        /// 检测是周围是否有墙壁，既是否可以爬墙。
        /// </summary>
        /// <returns></returns>
        public bool BoxCheckCanClimbDash()
        {
            Vector2 pos = GetColliderOffset();
            RightBox = Physics2D.BoxCastAll(RightCheckInfo.centerPos + pos, RightCheckInfo.size, 0, Vector3.right, 0.4f, checkLayerMask);
            LeftBox  = Physics2D.BoxCastAll(LeftCheckInfo.centerPos + pos, LeftCheckInfo.size, 0, Vector3.left, 0.4f, checkLayerMask);
            return RightBox.Length > 0 || LeftBox.Length > 0;
        }
        
        /// <summary>
        /// 获得碰撞盒偏移
        /// </summary>
        /// <returns></returns>
        public Vector2 GetColliderOffset()
        {
            Bounds bounds = collider2D.bounds;
            return new Vector2(bounds.center.x-bounds.extents.x,bounds.center.y-bounds.extents.y);
        }

        /// <summary>
        /// 绘制射线检测
        /// </summary>
        public void DrawRaycastGizmos()
        {
            Vector2 pos = GetColliderOffset();
            
            GizmosHelper.DrawBounds(new Bounds(UpCheckInfo.centerPos+pos,UpCheckInfo.size),Color.blue);
            GizmosHelper.DrawBounds(new Bounds(DownCheckInfo.centerPos+pos,DownCheckInfo.size),Color.blue);
            GizmosHelper.DrawBounds(new Bounds(LeftCheckInfo.centerPos+pos,LeftCheckInfo.size),Color.blue);
            GizmosHelper.DrawBounds(new Bounds(RightCheckInfo.centerPos+pos,RightCheckInfo.size),Color.blue);
        }
        
        /// <summary>
        /// 是否在地面上
        /// </summary>
        public bool IsGround { get { return DownBox.collider != null ? true : false; } }

        /// <summary>
        /// 碰撞检测中心
        /// </summary>
        public Vector2 Center
        {
            get
            {
                Bounds bounds = collider2D.bounds;
                return bounds.center;
            }
        }

        public Vector2 Size
        {
            get
            {
                Bounds bounds = collider2D.bounds;
                return bounds.extents;
            }
        }
    }
}