using Scenes.MoveTest;
using UnityEngine;

namespace Scenes.Move
{
    public partial class PlayerMove
    {
        /// <summary>
        /// 落下状态
        /// </summary>
        public void Fall()
        {
            if (IsGround())
            {
                ChangeState(PlayState.Normal);
                return;
            }
            //土狼时间--->玩家在下落时有几帧可以跳跃
            if(CoyotetimeFram > 0 && input.JumpKeyDown)
            {
                CoyotetimeFram = 0;
                Velocity.y = 0;
                Jump();
                return;
            }
            // //落下时如果在处在可以爬墙的位置，按下跳跃键即使不爬墙仍能进行小型蹬墙跳
            // if (input.JumpKeyDown && BoxCheckCanClimb() && !CheckIsClimb())
            // {
            //     Velocity.y = 0;
            //     Velocity.x = 0;
            //     Jump(new Vector2(4 * -GetClimpDirInt, 0), new Vector2(24 , 0));
            //     return;
            // }
            if (input.JumpKeyDown)
            {
                //蹬墙跳
                if (BoxCheckCanClimb() && !CheckIsClimb())
                {
                    Velocity.y = 0;
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
                Velocity.y -= 150f * Time.deltaTime;
                Velocity.y = Mathf.Clamp(Velocity.y, -25, Velocity.y);
                if (isCanClimb() && CheckIsClimb())
                {
                    playState = PlayState.Climb;
                }
            }
        }
        
        private bool isCanFall()
        {
            return playState != PlayState.Dash && playState != PlayState.Jump && playState != PlayState.Climb;
        }
    }
}