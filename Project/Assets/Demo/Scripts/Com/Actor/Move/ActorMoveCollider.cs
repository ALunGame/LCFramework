using UnityEngine;

namespace Demo.Com.Move
{
    public class ActorMoveCollider
    {
        /// <summary>
        /// 碰撞
        /// </summary>
        public BoxCollider2D Collider2D { get; private set; }
        
        /// <summary>
        /// 碰撞区域
        /// </summary>
        public Rect ColliderRect { get; private set; }
        
        /// <summary>
        /// 检测的碰撞层
        /// </summary>
        public int GroundMask { get; private set;}
        
        /// <summary>
        /// 合法的
        /// </summary>
        public bool IsLegal { get; private set;}

        public ActorMoveCollider()
        {
            IsLegal = false;
        }
        
        public void SetRect(BoxCollider2D pCollider2D)
        {
            Collider2D = pCollider2D;
            ColliderRect = new Rect(pCollider2D.offset, pCollider2D.size);
            IsLegal = true;
        }
        
        
    }
}