using System;
using System.Collections;
using UnityEngine;

namespace Demo.Com.MainActor
{
    public partial class MainActorMoveCom
    {
        [NonSerialized] private Vector2 DashDir;
        
        [NonSerialized] private int dashCount = 0;
        
        private void Dash()
        {
            velocity = Vector2.zero;
            dashCount--;
            ChangeState(MainActorMoveState.Dash);
            monoHelper.EndAllCoroutine();
            monoHelper.BeginCoroutine(IntroDash());
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
                if(moveState == MainActorMoveState.Dash)
                {
                    velocity = DashDir * 30f;
                }
                i++;
                CheckFixedHorizontalMove();
                yield return new WaitForFixedUpdate();
            }
            isCanControl = true;
            if (moveState == MainActorMoveState.Dash)
            {
                if (DashDir.y > 0)
                {
                    velocity.y = 24;
                }
                if(IsGround())
                    ChangeState(MainActorMoveState.Normal);
                else
                    ChangeState(MainActorMoveState.Fall);
            }
        }
        
        private void CheckDashJump()
        {
            if (moveState == MainActorMoveState.Dash)
            {
                if (input.JumpKeyDown)
                {
                    if (DashDir == Vector2.up && BoxCheckCanClimbDash())
                    {
                        Jump(new Vector2(4 * -GetClimpDirInt, 24 - JumpSpeed + 6), new Vector2(24, 0));
                    }
                    else if (IsGround())
                    {
                        velocity.y = 0;
                        if(input.v < 0)
                        {
                            if(input.MoveDir != 0)
                            {
                                dashCount = 1;
                                velocity = new Vector3(30 * input.MoveDir, 0);
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
        
        private bool BoxCheckCanClimbDash()
        {
            moveCollider.BoxCheckCanClimbDash();
            if (moveCollider.RightBox.Length > 0)
            {
                HorizontalBox = moveCollider.RightBox;
            }
            else if (moveCollider.LeftBox.Length > 0)
            {
                HorizontalBox = moveCollider.LeftBox;
            }
            return moveCollider.RightBox.Length > 0 || moveCollider.LeftBox.Length > 0;
        }
    }
}