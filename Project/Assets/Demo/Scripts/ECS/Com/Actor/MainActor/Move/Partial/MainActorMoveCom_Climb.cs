using System;
using System.Collections;
using UnityEngine;

namespace Demo.Com.MainActor
{
    public partial class MainActorMoveCom
    {
        //爬墙耐力相关
        [NonSerialized] private float CurStamina;
        [NonSerialized] private float ClimbMaxStamina = 110;
        [NonSerialized] private float ClimbUpCost = 100 / 2.2f;
        [NonSerialized] private float ClimbStillCost = 100 / 10f;
        [NonSerialized] private float ClimbJumpCost = 110 / 4f;
        
        //爬墙速度
        public float ClimbSpeed;
        
        /// <summary>
        /// 攀爬主方法
        /// </summary>
        void Climb()
        {
            if(IsGround())
            {
                ChangeState(MainActorMoveState.Normal);
                return;
            }
            
            var CheckBox = BoxCheckCanClimb();
            if (!CheckBox)
            {
                if (!CheckIsClimb())
                {
                    ChangeState(MainActorMoveState.Fall);
                    return;
                }
            }
            velocity.x = 0;
            currDir = HorizontalBox == moveCollider.RightBox ? DirType.Right : DirType.Left;

            //爬墙时，检测是否接近墙的最上端，小于一定距离时自动跳到平台上
            if (isCanClimb())
            {
                int climbDirValue = currDir == DirType.Right ? 1 : -1;
                //头顶没有碰撞
                if (moveCollider.UpBox.Length == 0)
                {
                    if (input.MoveDir == climbDirValue)
                    {
                        float checkYDis = Center.y - HorizontalBox[0].point.y;
                        if (checkYDis >= Size.y/2 - MainActorMoveCollider.ColliderRadius)
                        {
                            monoHelper.BeginCoroutine(ClambAutoJump());
                            return;
                        }
                    }
                }

                //反方向，取消爬墙
                if (input.MoveDir != climbDirValue)
                {
                    velocity.y = -ClimbSpeed;
                }
                else
                {
                    //到达最顶部
                    if (rig2D.position.y - HorizontalBox[0].point.y <= 0.7f)
                    {
                        velocity.y = ClimbSpeed;
                    }
                }
            }
            
            //蹬墙跳
            if(input.JumpKeyDown)
            {
                Jump(new Vector2(8 * -GetDirInt, 0), new Vector2(24 , 0));
            }

        }
        
        /// <summary>
        /// 攀爬到墙壁最上沿时如果有可跳跃平台，则自动跳跃到平台上
        /// </summary>
        IEnumerator ClambAutoJump()
        {
            var posY = Mathf.Ceil(rig2D.position.y) + Size.y;
            isCanControl = false;
            velocity = Vector3.zero;
            while (posY - rig2D.position.y > 0)
            {
                velocity.y = JumpSpeed;
                velocity.x = GetDirInt * 15;
                yield return null;
            }
            velocity = Vector3.zero;
            ChangeState(MainActorMoveState.Fall);
            isCanControl = true;
        }
        
        
        /// <summary>
        /// 蔚蓝中，紧贴着墙壁并且按住朝向墙壁的方向键，会减缓下落速度，这里是检测是否按了朝向墙壁的按键
        /// </summary>
        /// <returns></returns>
        bool CheckIsClimb()
        {
            return (input.MoveDir < 0 && moveCollider.LeftBox.Length > 0) || (input.MoveDir > 0 && moveCollider.RightBox.Length > 0);
        }

        /// <summary>
        /// 是否可以攀爬
        /// </summary>
        /// <returns></returns>
        private bool isCanClimb()
        {
            return (moveState != MainActorMoveState.Dash && moveState != MainActorMoveState.Jump) && BoxCheckCanClimb() && isCanControl && !isIntroJump;
        }
        
        //正确情况的蹬墙跳应该是墙壁相对于玩家的反方向，爬墙的时候对玩家朝向进行了修正，所以玩家的反方向就是跳跃方向，
        //但是在fall状态下没有对玩家方向进行修改，所以只能通过墙的位置进行判断
        private int GetClimpDirInt
        {
            get
            {
                return HorizontalBox == moveCollider.RightBox ? 1 : -1;
            }
        }
        
        private bool BoxCheckCanClimb()
        {
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