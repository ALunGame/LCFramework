using Demo.Com;
using DG.Tweening;
using LCECS.Core.ECS;
using System;
using System.Collections.Generic;
using LCECS;
using UnityEngine;

namespace Demo.System
{
    /// <summary>
    /// 玩家物理系统
    /// </summary>
    public class PlayerPhysicsSystem : BaseSystem
    {
        private Collider2D[] colliderResults = new Collider2D [4];
        
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(PlayerPhysicsCom), typeof(SpeedCom), typeof(PlayerCom),typeof(ColliderCom), typeof(StateCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            HandleState(comList);
            HandleCollider(comList);
            HandleMove(comList);
            HandleMass(comList);
            HandleGravityVelocity(comList);
            HandleShowVelocity(comList);
        }

        private void HandleState(List<BaseCom> comList)
        {
            PlayerPhysicsCom physicsCom = GetCom<PlayerPhysicsCom>(comList[0]);
            SpeedCom speedCom = GetCom<SpeedCom>(comList[1]);
            StateCom stateCom = GetCom<StateCom>(comList[4]);
            if (stateCom.CurState== EntityState.Stop || stateCom.CurState== EntityState.Dead)
            {
                physicsCom.Rig2D.velocity = Vector2.zero;
                speedCom.ReqDash = false;
                speedCom.ReqJumpSpeed = 0;
                speedCom.ReqMoveSpeed = 0;
            }
        }

        //处理碰撞方向
        private void HandleCollider(List<BaseCom> comList)
        {
            PlayerPhysicsCom physicsCom = GetCom<PlayerPhysicsCom>(comList[0]);
            ColliderCom colliderCom     = GetCom<ColliderCom>(comList[3]);

            //地面射线
            bool isGround    = Physics2D.OverlapCircleNonAlloc((Vector2)physicsCom.Rig2D.position + physicsCom.BottomCheckPoint, physicsCom.CollisionRadius,colliderResults,physicsCom.GroundLayer) > 0;

            //右射线
            bool isRightWall = Physics2D.OverlapCircleNonAlloc((Vector2)physicsCom.Rig2D.position + physicsCom.RightCheckPoint, physicsCom.CollisionRadius,colliderResults, physicsCom.GroundLayer)>0;

            //左射线
            bool isLeftWall  = Physics2D.OverlapCircleNonAlloc((Vector2)physicsCom.Rig2D.position + physicsCom.LeftCheckPoint, physicsCom.CollisionRadius,colliderResults,physicsCom.GroundLayer)>0;

            //赋值
            if (isGround)
            {
                colliderCom.CollideDir = ColliderDir.Down;
                //赋值子碰撞
                if (isRightWall)
                    colliderCom.SubCollideDir = ColliderDir.Right;
                else if (isLeftWall)
                    colliderCom.SubCollideDir = ColliderDir.Left;
                else
                    colliderCom.SubCollideDir = ColliderDir.None;
            }
            else
            {
                if (isRightWall)
                {
                    colliderCom.SubCollideDir = ColliderDir.Right;
                    colliderCom.CollideDir = ColliderDir.Right;
                }
                else if (isLeftWall)
                {
                    colliderCom.SubCollideDir = ColliderDir.Left;
                    colliderCom.CollideDir = ColliderDir.Left;
                }
                else
                {
                    colliderCom.SubCollideDir = ColliderDir.None;
                    colliderCom.CollideDir = ColliderDir.None;
                }
            }
        }

        //处理移动
        private void HandleMove(List<BaseCom> comList)
        {
            PlayerPhysicsCom physicsCom     = GetCom<PlayerPhysicsCom>(comList[0]);
            SpeedCom speedCom               = GetCom<SpeedCom>(comList[1]);
            PlayerCom playerCom             = GetCom<PlayerCom>(comList[2]);

            if (playerCom.DoDash)
            {
                Dash(physicsCom, speedCom, playerCom);
                playerCom.DoDash = false;
                return;
            }

            if (playerCom.IsDash)
            {
                return;
            }

            //速度赋值
            Movement(physicsCom,speedCom);
            if (speedCom.ReqJumpSpeed != 0)
            {
                Jump(physicsCom, speedCom);
            }
        }

        //处理质量
        private void HandleMass(List<BaseCom> comList)
        {
            PlayerPhysicsCom physicsCom = GetCom<PlayerPhysicsCom>(comList[0]);
            SpeedCom speedCom = GetCom<SpeedCom>(comList[1]);
            PlayerCom playerCom = GetCom<PlayerCom>(comList[2]);
            ColliderCom colliderCom     = GetCom<ColliderCom>(comList[3]);

            //冲刺时无重力
            if (playerCom.IsDash)
            {
                physicsCom.Mass = 0;
                return;
            }

            //没有体力了（直接默认质量）
            if (playerCom.CurrEnergy <= 0)
            {
                physicsCom.Mass = physicsCom.DefaultMass;
                return;
            }

            //墙上质量降低（摩擦）
            if (colliderCom.CollideDir == ColliderDir.Left || colliderCom.CollideDir == ColliderDir.Right)
            {
                physicsCom.Mass = physicsCom.DefaultMass * 0.6f;
            }

            physicsCom.Mass = physicsCom.DefaultMass;
        }

        //处理重力速度
        private void HandleGravityVelocity(List<BaseCom> comList)
        {
            PlayerPhysicsCom phyCom = GetCom<PlayerPhysicsCom>(comList[0]);
            SpeedCom speedCom       = GetCom<SpeedCom>(comList[1]);
            PlayerCom playerCom     = GetCom<PlayerCom>(comList[2]);

            //降落
            if (phyCom.Rig2D.velocity.y < 0)
            {
                phyCom.Rig2D.velocity += Definition.Gravity * playerCom.FallMultiplier * phyCom.Mass * Time.deltaTime;
            }
            //跳跃
            else if (phyCom.Rig2D.velocity.y > 0)
            {
                if (speedCom.ReqJumpSpeed == 0)
                {
                    phyCom.Rig2D.velocity += Definition.Gravity * playerCom.LowJumpMultiplier * phyCom.Mass * Time.deltaTime;
                }
            }
        }
        
        //处理展示的速度（和动画播放相关）
        private void HandleShowVelocity(List<BaseCom> comList)
        {
            PlayerPhysicsCom physicsCom     = GetCom<PlayerPhysicsCom>(comList[0]);
            SpeedCom speedCom               = GetCom<SpeedCom>(comList[1]);
            PlayerCom playerCom             = GetCom<PlayerCom>(comList[2]);
            ColliderCom colliderCom     = GetCom<ColliderCom>(comList[3]);
            
            //水平方向
            if (speedCom.ReqMoveSpeed!=0)
            {
                speedCom.CurVelocity.x = speedCom.ReqMoveSpeed;
            }
            else
            {
                speedCom.CurVelocity.x = physicsCom.Rig2D.velocity.x;
            }
            
            //竖直方向
            if (speedCom.ReqJumpSpeed!=0)
            {
                speedCom.CurVelocity.y = speedCom.ReqJumpSpeed;
            }
            else
            {
                if (colliderCom.CollideDir == ColliderDir.Down)
                {
                    speedCom.CurVelocity.y = 0;
                }
                else
                {
                    speedCom.CurVelocity.y = physicsCom.Rig2D.velocity.y;
                }
            }
        }

        //执行移动
        private void Movement(PlayerPhysicsCom phyCom,SpeedCom speedCom)
        {
            phyCom.Rig2D.velocity = new Vector2(speedCom.ReqMoveSpeed, phyCom.Rig2D.velocity.y);
        }

        //执行跳跃
        private void Jump(PlayerPhysicsCom phyCom, SpeedCom speedCom)
        {
            phyCom.Rig2D.velocity = new Vector2(phyCom.Rig2D.velocity.x, speedCom.ReqJumpSpeed);
        }

        //执行冲刺
        private void Dash(PlayerPhysicsCom phyCom, SpeedCom speedCom, PlayerCom playerCom)
        {
            playerCom.IsDash        = true;

            //速度归零
            phyCom.Rig2D.velocity   = Vector2.zero;

            //计算冲刺速度
            Vector2 dir             = Vector2.zero;
            dir.x                   = playerCom.SpriteRender.flipX ? -1 : 1;
            dir.y                   = speedCom.ReqJumpSpeed == 0 ? 0 : 1;
            phyCom.Rig2D.velocity  += dir * speedCom.DashSpeed;

            //拖拽还原状态
            DOVirtual.Float(8, 0, playerCom.DashDragTime, (x) =>
            {
                phyCom.Rig2D.drag = x;
                if (x == 0)
                {
                    playerCom.IsDash = false;
                    phyCom.Mass = phyCom.DefaultMass;
                }
            });
        }

        
    }

}
