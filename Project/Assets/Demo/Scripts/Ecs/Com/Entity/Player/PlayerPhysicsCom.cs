using LCECS.Core.ECS;
using UnityEngine;

namespace Demo.Com
{
    /// <summary>
    /// 玩家物理组件
    /// </summary>
    [Com(ViewName = "玩家物理组件", GroupName = "Player")]
    public class PlayerPhysicsCom : BaseCom
    {
        public Rigidbody2D Rig2D;
        public int GroundLayer = LayerMask.GetMask("Ground");

        //质量
        [ComValue(ViewEditor = true)]
        public float Mass = 1f;

        //默认质量
        [ComValue(ViewEditor = true)]
        public float DefaultMass = 1f;

        public float CollisionRadius     = 0.05f;
        [ComValue(ViewEditor = true)]
        public float CollisionOffset     = 0.05f;
        public Vector2 BottomCheckPoint  = Vector2.zero;
        public Vector2 RightCheckPoint   = Vector2.zero;
        public Vector2 LeftCheckPoint    = Vector2.zero;

        protected override void OnInit(GameObject go)
        {
            Rig2D = go.GetComponent<Rigidbody2D>();

            Collider2D collider2D = go.GetComponent<Collider2D>();
            BottomCheckPoint = new Vector2(collider2D.bounds.center.x,collider2D.bounds.center.y-collider2D.bounds.extents.y - CollisionOffset);
            RightCheckPoint  = new Vector2(collider2D.bounds.center.x+collider2D.bounds.extents.x+CollisionOffset,collider2D.bounds.center.y);
            LeftCheckPoint   = new Vector2(collider2D.bounds.center.x-collider2D.bounds.extents.x-CollisionOffset,collider2D.bounds.center.y);
        }
    }
}
