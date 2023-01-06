using System.Collections;
using Scenes.MoveTest;
using UnityEngine;

namespace Scenes.Move
{
    public partial class PlayerMove
    {
        [Header("冲刺方向")]
        public Vector2 DashDir;
        
        private int dashCount = 0;
        
        private void Dash()
        {
            Velocity = Vector2.zero;
            dashCount--;
            ChangeState(PlayState.Dash);
            StopAllCoroutines();
            StartCoroutine("IntroDash");
        }
        
        private IEnumerator IntroDash()
        {
            //获取输入时的按键方向
            float verticalDir;
            if(IsGround() && input.v < 0)  //在地面上并且按住下时不应该有垂直方向
            {
                verticalDir = 0;
            }
            else
            {
                verticalDir = input.v;
            }
            //冲刺方向注意归一化
            DashDir = new Vector2(input.MoveDir, verticalDir).normalized;
            if(DashDir == Vector2.zero)
            {
                DashDir = Vector3.right * GetDirInt;
            }
            int i = 0;
            isCanControl = false;
            FixHorizon = false;
            while (i < 9)
            {
                if(playState == PlayState.Dash)
                {
                    Velocity = DashDir * 30f;
                }
                i++;
                CheckFixedHorizontalMove();
                yield return new WaitForFixedUpdate();
            }
            isCanControl = true;
            if (playState == PlayState.Dash)
            {
                if (DashDir.y > 0)
                {
                    Velocity.y = 24;
                }
                if(IsGround())
                    ChangeState(PlayState.Normal);
                else
                    ChangeState(PlayState.Fall);
            }
        }
        
        private void CheckDashJump()
        {
            if (playState == PlayState.Dash)
            {
                if (input.JumpKeyDown)
                {
                    if (DashDir == Vector2.up && BoxCheckCanClimbDash())
                    {
                        Jump(new Vector2(4 * -GetClimpDirInt, 24 - JumpSpeed + 6), new Vector2(24, 0));
                    }
                    else if (IsGround())
                    {
                        Velocity.y = 0;
                        if(input.v < 0)
                        {
                            if(input.MoveDir != 0)
                            {
                                dashCount = 1;
                                Velocity = new Vector3(30 * input.MoveDir, 0);
                                Jump(new Vector2(4 * input.MoveDir, -4), new Vector2(42, 0));
                            }
                            else
                            {
                                Jump(new Vector2(0, -4), new Vector2(0, 0));
                            }
                        }
                        else
                        {
                            Jump();
                        }
                    }
                }
            }
        }
    }
}