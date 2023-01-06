using System.Collections;
using Demo.Com;
using Scenes.MoveTest;
using UnityEngine;

namespace Scenes.Move
{
    public partial class PlayerMove
    {
        [Header("爬墙速度")]
        public float ClimbSpeed;
        //爬墙耐力相关
        private float CurStamina;
        private float ClimbMaxStamina = 110;
        private float ClimbUpCost = 100 / 2.2f;
        private float ClimbStillCost = 100 / 10f;
        private float ClimbJumpCost = 110 / 4f;
        
        /// <summary>
        /// 攀爬主方法
        /// </summary>
        private void Climb()
        {
            if(IsGround())
            {
                ChangeState(PlayState.Normal);
                return;
            }
            
            var CheckBox = BoxCheckCanClimb();
            if (!CheckBox)
            {
                if (!CheckIsClimb())
                {
                    ChangeState(PlayState.Fall);
                    return;
                }
            }
            Velocity.x = 0;
            playDir = HorizontalBox == _moveCollider.RightBox ? DirType.Right : DirType.Left;

            //爬墙时，检测是否接近墙的最上端，小于一定距离时自动跳到平台上
            if (isCanClimb())
            {
                int climbDirValue = playDir == DirType.Right ? 1 : -1;
                //头顶没有碰撞
                if (_moveCollider.UpBox.Length == 0)
                {
                    if (input.MoveDir == climbDirValue)
                    {
                        float checkYDis = Center.y - HorizontalBox[0].point.y;
                        if (checkYDis >= Size.y/2 - PlayerMoveCollider.ColliderRadius)
                        {
                            StartCoroutine("ClambAutoJump");
                            return;
                        }
                    }
                }

                //反方向，取消爬墙
                if (input.MoveDir != climbDirValue)
                {
                    Velocity.y = -ClimbSpeed;
                }
                else
                {
                    //到达最顶部
                    if (transform.position.y - HorizontalBox[0].point.y <= 0.7f)
                    {
                        Velocity.y = ClimbSpeed;
                    }
                }
            }
            
            //蹬墙跳
            if(input.JumpKeyDown)
            {
                if(input.ClimbKey)
                {
                    if((input.h > 0 && GetDirInt < 0) || (input.h < 0 && GetDirInt > 0))
                    {
                        Jump(new Vector2(8 * -GetDirInt, 0), new Vector2(24 , 0));
                    }
                    else
                    {
                        Jump();
                    }
                }
                else
                {
                    Jump(new Vector2(8 * -GetDirInt, 0), new Vector2(24 , 0));
                }
            }

        }
        
        /// <summary>
        /// 攀爬到墙壁最上沿时如果有可跳跃平台，则自动跳跃到平台上
        /// </summary>
        private IEnumerator ClambAutoJump()
        {
            var posY = Mathf.Ceil(transform.position.y) + Size.y;
            isCanControl = false;
            Velocity = Vector3.zero;
            while (posY - transform.position.y > 0)
            {
                Velocity.y = JumpSpeed;
                Velocity.x = GetDirInt * 15;
                yield return null;
            }
            Velocity = Vector3.zero;
            ChangeState(PlayState.Fall);
            isCanControl = true;
        }
        
        
        /// <summary>
        /// 蔚蓝中，紧贴着墙壁并且按住朝向墙壁的方向键，会减缓下落速度，这里是检测是否按了朝向墙壁的按键
        /// </summary>
        /// <returns></returns>
        private bool CheckIsClimb()
        {
            return (input.MoveDir < 0 && _moveCollider.LeftBox.Length > 0) || (input.MoveDir > 0 && _moveCollider.RightBox.Length > 0);
        }

        /// <summary>
        /// 是否可以攀爬
        /// </summary>
        /// <returns></returns>
        private bool isCanClimb()
        {
            return (playState != PlayState.Dash && playState != PlayState.Jump) && BoxCheckCanClimb() && isCanControl && !isIntroJump;
        }
        
        //正确情况的蹬墙跳应该是墙壁相对于玩家的反方向，爬墙的时候对玩家朝向进行了修正，所以玩家的反方向就是跳跃方向，
        //但是在fall状态下没有对玩家方向进行修改，所以只能通过墙的位置进行判断
        private int GetClimpDirInt
        {
            get
            {
                return HorizontalBox == _moveCollider.RightBox ? 1 : -1;
            }
        }
    }
}