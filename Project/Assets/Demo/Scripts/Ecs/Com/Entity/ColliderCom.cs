using LCECS.Core.ECS;
using UnityEngine;

namespace Demo.Com
{
    public enum ColliderDir : short
    {
        None,
        Up,
        Down,
        Left,
        Right,
    }
    
    [Com(ViewName = "碰撞组件",GroupName = "Entity")]
    public class ColliderCom:BaseCom
    {
        //碰撞方向
        [ComValue]
        public ColliderDir CollideDir = ColliderDir.None;

        //辅助碰撞方向（当碰撞较多时）
        [ComValue]
        public ColliderDir SubCollideDir = ColliderDir.None;


        protected override void OnInit(GameObject go)
        {
            
        }
    }
}