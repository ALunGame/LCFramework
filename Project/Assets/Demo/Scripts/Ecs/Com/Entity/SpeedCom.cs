using LCECS.Core.ECS;
using UnityEngine;

namespace Demo.Com
{
    /// <summary>
    /// 速度组件
    /// </summary>
    [Com(ViewName = "速度组件", GroupName = "Entity")]
    public class SpeedCom : BaseCom
    {
        [ComValue]
        public float ReqMoveSpeed = 0;
        [ComValue]
        public float ReqJumpSpeed = 0;
        [ComValue]
        public bool ReqDash       = false;
        
        public float MaxMoveSpeed      = 0;
        public float MaxJumpSpeed      = 0;
        public float DashSpeed         = 0;
        public float ClimbSpeed        = 0;

        [ComValue]
        public Vector2 CurVelocity  = Vector2.zero;

        protected override void OnInit(GameObject go)
        {
        }
    }
}
