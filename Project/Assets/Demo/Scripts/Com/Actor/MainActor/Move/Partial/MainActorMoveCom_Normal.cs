using System;

namespace Demo.Com.MainActor
{
    public partial class MainActorMoveCom
    {
        /// <summary>
        /// 土狼时间
        /// </summary>
        [NonSerialized] private int CoyotetimeFram = 0;
        
        /// <summary>
        /// 移动速度
        /// </summary>
        public float MoveSpeed;
        
        private void Normal()
        {
            if (!IsGround())
            {
                CoyotetimeFram = 4;
                ChangeState(MainActorMoveState.Fall);
                return;
            }

            velocity.y = 0;
            if (input.JumpKeyDown)
            {
                Jump();
                return;
            }
        }
    }
}