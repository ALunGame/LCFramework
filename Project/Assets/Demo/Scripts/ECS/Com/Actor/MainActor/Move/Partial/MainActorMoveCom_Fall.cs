using UnityEngine;

namespace Demo.Com.MainActor
{
    public partial class MainActorMoveCom
    {
        /// <summary>
        /// 落下状态
        /// </summary>
        private void Fall()
        {
            if (IsGround())
            {
                ChangeState(MainActorMoveState.Normal);
                return;
            }
            //土狼时间--->玩家在下落时有几帧可以跳跃
            if(CoyotetimeFram > 0 && input.JumpKeyDown)
            {
                CoyotetimeFram = 0;
                velocity.y = 0;
                Jump();
                return;
            }
            if (input.JumpKeyDown)
            {
                //蹬墙跳
                if (BoxCheckCanClimb() && !CheckIsClimb())
                {
                    velocity.y = 0;
                    isIntroJump = false;
                    Jump(new Vector2(4 * -GetDirInt, 0), new Vector2(24, 0));
                    return;
                }
                //二段跳
                else
                {
                    if (SecondJump())
                        return;
                }
            }
            if (isCanFall())
            {
                velocity.y -= 150f * Time.deltaTime;
                velocity.y = Mathf.Clamp(velocity.y, -25, velocity.y);
                if (isCanClimb() && CheckIsClimb())
                {
                    moveState = MainActorMoveState.Climb;
                }
            }
        }
        
        private bool isCanFall()
        {
            return moveState != MainActorMoveState.Dash && moveState != MainActorMoveState.Jump && moveState != MainActorMoveState.Climb;
        }
    }
}