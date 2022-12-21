using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using LCToolkit;
using UnityEngine;

namespace Demo
{
    public class MoveWithColliderSystem : BaseSystem
    {
        //最小降落速度
        public const float _MinFallSpeed = -25;
        public const float _G = -9.8f;
        
        private const float CollisionRadius = 0.05f;
        private float m_MovementSmoothing = .05f;
        private Vector2 velocity = Vector3.zero;

        protected override List<Type> RegContainListenComs()
        {
            return new List<Type>() { typeof(MoveCom), typeof(TransCom), typeof(Collider2DCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            MoveCom moveCom = GetCom<MoveCom>(comList[0]);
            TransCom transCom = GetCom<TransCom>(comList[1]);
            Collider2DCom collider2DCom = GetCom<Collider2DCom>(comList[2]);

            Rigidbody2D rig2d = collider2DCom.rig2D;
            //碰撞更新
            UpdateColliderInfo(collider2DCom);

            MoveState moveState = CalcMoveState(collider2DCom, moveCom.CurrentMoveInfo);
            moveCom.SetCurrMoveState(moveState);
            HandleGravity(moveCom.CurrentMoveInfo, moveCom, collider2DCom);
            ExecuteMoveState(moveState, collider2DCom, moveCom);
        }

        #region 碰撞更新

        private void UpdateColliderInfo(Collider2DCom collider2DCom)
        {
            Physics2D.SyncTransforms();
            HandleBoxCollider(collider2DCom);
            HandleCornerCollider(collider2DCom);
        }

        private void HandleBoxCollider(Collider2DCom collider2DCom)
        {
            Vector2 pos = collider2DCom.GetColliderOffset();
            collider2DCom.Collider.Up       = CheckCollider(collider2DCom.UpCheckInfo.centerPos+pos,collider2DCom.UpCheckInfo.size);
            collider2DCom.Collider.Down     = CheckCollider(collider2DCom.DownCheckInfo.centerPos+pos,collider2DCom.DownCheckInfo.size);
            collider2DCom.Collider.Left     = CheckCollider(collider2DCom.LeftCheckInfo.centerPos+pos,collider2DCom.LeftCheckInfo.size);
            collider2DCom.Collider.Right    = CheckCollider(collider2DCom.RightCheckInfo.centerPos+pos,collider2DCom.RightCheckInfo.size);
        }

        //拐角碰撞检测
        private void HandleCornerCollider(Collider2DCom collider2DCom)
        {
            Vector2 pos = collider2DCom.GetColliderOffset();
            collider2DCom.Collider.UpRightCorner    = CheckCollider(collider2DCom.UpRightCornerCheckInfo.centerPos+pos,collider2DCom.UpRightCornerCheckInfo.size);
            collider2DCom.Collider.UpLeftCorner     = CheckCollider(collider2DCom.UpLeftCornerCheckInfo.centerPos+pos,collider2DCom.UpLeftCornerCheckInfo.size);
            collider2DCom.Collider.DownRightCorner  = CheckCollider(collider2DCom.DownRightCornerCheckInfo.centerPos+pos,collider2DCom.DownRightCornerCheckInfo.size);
            collider2DCom.Collider.DownLeftCorner   = CheckCollider(collider2DCom.DownLeftCornerCheckInfo.centerPos+pos,collider2DCom.DownLeftCornerCheckInfo.size);
        }

        private bool CheckCollider(Vector2 pCenter,Vector2 pSize)
        {
            Collider2D[] results = new Collider2D[4];
            int colliderCnt = Physics2D.OverlapBoxNonAlloc(pCenter, pSize, 0, results, LayerMask.GetMask("Map"));
            //int colliderCnt = Physics2D.OverlapCircleNonAlloc(pCenter, 0.2f, results, LayerMask.GetMask("Map"));
            return colliderCnt > 0;
        }

        #endregion
        
        
        private MoveState CalcMoveState(Collider2DCom pCollider2DCom, MoveInfo pMoveInfo)
        {
            float moveSpeed = pMoveInfo.moveSpeed;
            bool needJump = pMoveInfo.needJump;
            //没有任何移动
            if (moveSpeed == 0 && !needJump)
            {
                if (pCollider2DCom.Collider.Down)
                {
                    return MoveState.Stay_Ground;
                }
                else
                {
                    return MoveState.Fall_Sky;
                }
            }
            else
            {
                //空中
                if (pCollider2DCom.Collider.IsNull)
                {
                    if (!needJump)
                        return MoveState.Move_Sky;
                    else
                    {
                        if (moveSpeed == 0)
                            return MoveState.Jump_Sky;
                        else
                            return MoveState.MoveJump_Sky;
                    }
                }
                //地面
                if (pCollider2DCom.Collider.Down)
                {
                    if (needJump)
                    {
                        if (moveSpeed == 0)
                        {
                            return MoveState.Jump_Ground;
                        }
                        else
                        {
                            return MoveState.MoveJump_Ground;     
                        }
                    }
                    else
                    {
                        return MoveState.Move_Ground;    
                    }
                }
                //左侧墙
                if (pCollider2DCom.Collider.Left)
                {
                    if (moveSpeed > 0)
                    {
                        //抓墙
                        if (needJump)
                        {
                            return MoveState.GrabJump_Wall;  
                        }
                        else
                        {
                            return MoveState.Grab_Wall;     
                        }
                    }

                    if (moveSpeed < 0)
                    {
                        //爬墙
                        if (pCollider2DCom.Collider.UpLeftCorner)
                        {
                            if (needJump)
                            {
                                return MoveState.MoveJump_Wall;  
                            }
                            else
                            {
                                return MoveState.Move_Wall;     
                            }
                        }
                        //挂在墙上
                        else
                        {
                            if (needJump)
                            {
                                return MoveState.HangJump_Wall;  
                            }
                            else
                            {
                                return MoveState.Hang_Wall;     
                            }
                        }
                    }
                }
                //右侧墙
                if (pCollider2DCom.Collider.Right)
                {
                    if (moveSpeed < 0)
                    {
                        //抓墙
                        if (needJump)
                        {
                            return MoveState.GrabJump_Wall;  
                        }
                        else
                        {
                            return MoveState.Grab_Wall;     
                        }
                    }

                    if (moveSpeed > 0)
                    {
                        //爬墙
                        if (pCollider2DCom.Collider.UpLeftCorner)
                        {
                            if (needJump)
                            {
                                return MoveState.MoveJump_Wall;  
                            }
                            else
                            {
                                return MoveState.Move_Wall;     
                            }
                        }
                        //挂在墙上
                        else
                        {
                            if (needJump)
                            {
                                return MoveState.HangJump_Wall;  
                            }
                            else
                            {
                                return MoveState.Hang_Wall;     
                            }
                        }
                    }
                }
                //头顶有碰撞
                if (pCollider2DCom.Collider.Up)
                {
                    return MoveState.Fall_Sky;
                }
                return MoveState.Stay_Ground;
            }
        }
        
        private void HandleGravity(MoveInfo pMoveInfo, MoveCom pMoveCom,Collider2DCom pCollider2DCom)
        {
            MoveState moveState = pMoveCom.CurrMoveState;
            //在地面不处理重力
            if (moveState <= MoveState.MoveJump_Ground)
            {
                //没有跳跃，跳跃速度也不为0
                if (!pMoveInfo.needJump && pCollider2DCom.rig2D.velocity.y < 0)
                {
                    pCollider2DCom.rig2D.velocity = new Vector2(pCollider2DCom.rig2D.velocity.x, 0);
                }

                pCollider2DCom.rig2D.mass = MoveCom.DefaultMass;
                return;
            }
            //抓墙和挂墙无重力
            if (moveState == MoveState.Grab_Wall || moveState == MoveState.Hang_Wall)
            {
                pCollider2DCom.rig2D.mass = 0;
                pCollider2DCom.rig2D.velocity = new Vector2(pCollider2DCom.rig2D.velocity.x, 0);
                return;
            }
            //天空中
            if (moveState>=MoveState.Fall_Sky && moveState <= MoveState.MoveJump_Sky)
            {
                float tMass = pMoveCom.Mass;
                //降落快速
                if (pCollider2DCom.rig2D.velocity.y <= 0)
                {
                    pCollider2DCom.rig2D.mass =  MoveCom.DefaultMass*1.5f;
                }
                else
                {
                    pCollider2DCom.rig2D.mass =  MoveCom.DefaultMass;
                }
                return;
            }
            pCollider2DCom.rig2D.mass =  MoveCom.DefaultMass;
        }

        private void LimitMoveInfo(MoveState pMoveState, Collider2DCom pCollider2DCom,  MoveCom pMoveCom)
        {
            MoveInfo moveInfo = pMoveCom.CurrentMoveInfo;
            
            //地面操作
            if (pMoveState>=MoveState.Stay_Ground && pMoveState<=MoveState.MoveJump_Ground)
            {
                //跳跃不能穿过头顶障碍
                if (pCollider2DCom.Collider.Up)
                    moveInfo.needJump = false;
                if (pCollider2DCom.Collider.Left && moveInfo.moveSpeed < 0)
                    moveInfo.moveSpeed = 0;
                if (pCollider2DCom.Collider.Right && moveInfo.moveSpeed > 0)
                    moveInfo.moveSpeed = 0;
            }

            //墙壁操作
            if (pMoveState>=MoveState.Move_Wall && pMoveState<=MoveState.HangJump_Wall)
            {
                if (pMoveState == MoveState.Grab_Wall || pMoveState == MoveState.GrabJump_Wall 
                    || pMoveState == MoveState.Hang_Wall || pMoveState == MoveState.HangJump_Wall)
                    moveInfo.moveSpeed = 0;
            }

            //空中操作
            if (pMoveState>=MoveState.Fall_Sky && pMoveState<=MoveState.MoveJump_Sky)
            {
                //跳跃不能穿过头顶障碍
                if (pCollider2DCom.Collider.Up)
                    moveInfo.needJump = false;
                if (pCollider2DCom.Collider.Left && moveInfo.moveSpeed < 0)
                    moveInfo.moveSpeed = 0;
                if (pCollider2DCom.Collider.Right && moveInfo.moveSpeed > 0)
                    moveInfo.moveSpeed = 0;
            }
            
        }

        #region 执行移动状态
        
        private void ExecuteMoveState(MoveState pMoveState, Collider2DCom pCollider2DCom,  MoveCom pMoveCom)
        {
            MoveInfo moveInfo = pMoveCom.CurrentMoveInfo;
            
            //地面操作
            if (pMoveState >= MoveState.Stay_Ground && pMoveState <= MoveState.MoveJump_Ground)
            {
                HandleJump(pCollider2DCom, pMoveCom);
                HandleMove(pCollider2DCom, pMoveCom);
            }
            
            //墙壁操作
            if (pMoveState >= MoveState.Move_Wall && pMoveState <= MoveState.HangJump_Wall)
            {
                //挂墙或者抓墙
                if (pMoveState >= MoveState.Grab_Wall && pMoveState <= MoveState.HangJump_Wall)
                {
                    if (moveInfo.needJump)
                    {
                        if (pCollider2DCom.Collider.Left && pCollider2DCom.Collider.Right)
                        {
                            HandleJump(pCollider2DCom, pMoveCom);
                        }
                        else
                        {
                            if (pCollider2DCom.Collider.Left)
                            {
                                HandleJump(pCollider2DCom, pMoveCom);
                                pCollider2DCom.rig2D.velocity = new Vector2(10, pCollider2DCom.rig2D.velocity.y);
                            }

                            if (pCollider2DCom.Collider.Right)
                            {
                                HandleJump(pCollider2DCom, pMoveCom);
                                pCollider2DCom.rig2D.velocity = new Vector2(-10, pCollider2DCom.rig2D.velocity.y);
                            }
                        }
                    }
                }
                else
                {
                    HandleWallMove(pCollider2DCom, pMoveCom);
                    HandleJump(pCollider2DCom, pMoveCom);
                }
            }
            
            //空中操作
            if (pMoveState >= MoveState.Fall_Sky && pMoveState <= MoveState.MoveJump_Sky)
            {
                HandleJump(pCollider2DCom, pMoveCom);
                HandleMove(pCollider2DCom, pMoveCom);
            }
        }
        
        private void HandleJump(Collider2DCom pCollider2DCom,MoveCom pMoveCom)
        {
            if (pCollider2DCom.Collider.Up || !pMoveCom.CurrentMoveInfo.needJump)
            {
                pMoveCom.CurrentMoveInfo.needJump = false;
                return;
            }
            pCollider2DCom.rig2D.velocity = new Vector2(pCollider2DCom.rig2D.velocity.x, 0);
            pCollider2DCom.rig2D.AddForce(new Vector2(0,pMoveCom.CurrentMoveInfo.jumpSpeed));
            pMoveCom.CurrentMoveInfo.needJump = false;
        }

        private void HandleMove(Collider2DCom pCollider2DCom,MoveCom pMoveCom)
        {
            if (pMoveCom.CurrentMoveInfo.moveSpeed >0)
            {
                if (pCollider2DCom.Collider.Right)
                {
                    return;
                }
                else
                {
                    Vector2 targetVelocity = new Vector2(pMoveCom.CurrentMoveInfo.moveSpeed, pCollider2DCom.rig2D.velocity.y);
                    pCollider2DCom.rig2D.velocity = Vector2.SmoothDamp(pCollider2DCom.rig2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
                }
            }

            if (pMoveCom.CurrentMoveInfo.moveSpeed <0)
            {
                if (pCollider2DCom.Collider.Left)
                {
                    return;
                }
                else
                {
                    Vector2 targetVelocity = new Vector2(pMoveCom.CurrentMoveInfo.moveSpeed, pCollider2DCom.rig2D.velocity.y);
                    pCollider2DCom.rig2D.velocity = Vector2.SmoothDamp(pCollider2DCom.rig2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
                }
            }

            if (pMoveCom.CurrentMoveInfo.moveSpeed ==0)
            {
                pCollider2DCom.rig2D.velocity = new Vector2(0, pCollider2DCom.rig2D.velocity.y);
            }
        }
        
        private void HandleWallMove(Collider2DCom pCollider2DCom,MoveCom pMoveCom)
        {
            if (pMoveCom.CurrentMoveInfo.moveSpeed > 0)
            {
                //爬墙
                if (pCollider2DCom.Collider.Right && pCollider2DCom.Collider.UpRightCorner)
                {
                    Vector2 targetVelocity = new Vector2(0, pMoveCom.CurrentMoveInfo.moveSpeed);
                    pCollider2DCom.rig2D.velocity = Vector2.SmoothDamp(pCollider2DCom.rig2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
                }
            }

            if (pMoveCom.CurrentMoveInfo.moveSpeed < 0)
            {
                //爬墙
                if (pCollider2DCom.Collider.Left && pCollider2DCom.Collider.UpLeftCorner)
                {
                    Vector2 targetVelocity = new Vector2(0, -pMoveCom.CurrentMoveInfo.moveSpeed);
                    pCollider2DCom.rig2D.velocity = Vector2.SmoothDamp(pCollider2DCom.rig2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
                }
            }
        }

        #endregion
        

        private Collider2D[] results = new Collider2D[4];
        private Vector3 RayCastMovePos(Vector3 pMoveDelta,Collider2DCom pCollider2DCom)
        {
            results = new Collider2D[4];
            Vector2 offsetPos = pCollider2DCom.GetColliderOffset();
            float xValue = pMoveDelta.x;
            if (xValue !=0)
            {
                Vector2 pos = xValue > 0 ? pCollider2DCom.RightCheckInfo.centerPos : pCollider2DCom.LeftCheckInfo.centerPos;
                pos = pos + offsetPos + new Vector2(xValue,0);
                
                Vector2 size = xValue > 0 ? pCollider2DCom.RightCheckInfo.size : pCollider2DCom.LeftCheckInfo.size;
                size = new Vector2(xValue > 0 ? xValue : -xValue, size.y);

                int colliderCnt = Physics2D.OverlapBoxNonAlloc(pos, size, 0, results, LayerMask.GetMask("Map"));
                if (colliderCnt > 0)
                {
                    pMoveDelta.x = 0;
                }
            }

            float yValue = pMoveDelta.y;
            if (yValue != 0)
            {
                Vector2 pos = yValue > 0 ? pCollider2DCom.UpCheckInfo.centerPos : pCollider2DCom.DownCheckInfo.centerPos;
                pos = pos + offsetPos + new Vector2(0,yValue);
                
                Vector2 size = yValue > 0 ? pCollider2DCom.UpCheckInfo.size : pCollider2DCom.DownCheckInfo.size;
                size = new Vector2(size.x, yValue >0 ? yValue : -yValue);

                int colliderCnt = Physics2D.OverlapBoxNonAlloc(pos, size, 0, results, LayerMask.GetMask("Map"));
                if (colliderCnt > 0)
                {
                    pMoveDelta.y = 0;
                }
            }

            return pMoveDelta;
        }
    }
}