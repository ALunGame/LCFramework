﻿using System;
using System.Collections.Generic;
using LCMap;
using LCToolkit;
using UnityEngine;

namespace Demo.Com.MainActor.NewMove
{
    public enum ColliderDirType
    {
        Up,
        Down,
        Left,
        Right,
    }

    public class ColliderCheckInfo
    {
        public Vector2 dir;
        public Vector2 pos;
        public Vector2 size;

        public ColliderCheckInfo(Vector2 pDir, Vector2 pPos, Vector2 pSize)
        {
            dir = pDir;
            pos = pPos;
            size = pSize;
        }

        public ColliderCheckInfo()
        {
            
        }
    }
    
    /// <summary>
    /// 主角碰撞信息
    /// </summary>
    public class MainActorCollider
    {
        const float STEP = 0.1f;  //碰撞检测步长，对POINT检测用
        const float DEVIATION = 0.02f;  //碰撞检测误差
        
        /// <summary>
        /// 碰撞
        /// </summary>
        public BoxCollider2D Collider2D { get; private set; }
        
        /// <summary>
        /// 碰撞区域
        /// </summary>
        public Rect ColliderRect { get; private set; }
        
        /// <summary>
        /// 检测区域
        /// </summary>
        private Dictionary<ColliderDirType,ColliderCheckInfo> checkInfoMap = new Dictionary<ColliderDirType, ColliderCheckInfo>();

        /// <summary>
        /// 检测的碰撞层
        /// </summary>
        public int GroundMask { get; private set;}
        
        /// <summary>
        /// 合法的
        /// </summary>
        public bool IsLegal { get; private set;}

        private RaycastHit2D[] results = new RaycastHit2D[4];
        private Collider2D[] colResults = new Collider2D[4];
        private NewMainActorMoveCom moveCom;

        public MainActorCollider(NewMainActorMoveCom pMoveCom, Actor pActor)
        {
            moveCom = pMoveCom;
            GroundMask = LayerMask.GetMask("Map");
            IsLegal = false;
        }

        public void SetRect(BoxCollider2D pCollider2D)
        {
            Collider2D = pCollider2D;
            ColliderRect = new Rect(pCollider2D.offset, pCollider2D.size);
            IsLegal = true;

            CollectColliderCheckInfo();
        }

        private void CollectColliderCheckInfo()
        {
            checkInfoMap.Clear();
            
            ColliderCheckInfo upInfo = new ColliderCheckInfo();
            upInfo.dir = Vector2.up;
            upInfo.size = new Vector2(ColliderRect.size.x - DEVIATION*2, ColliderRect.size.y / 2);
            upInfo.pos = new Vector2(0,upInfo.size.y/2);
            checkInfoMap.Add(ColliderDirType.Up,upInfo);
            
            ColliderCheckInfo downInfo = new ColliderCheckInfo();
            downInfo.dir = Vector2.down;
            downInfo.size = new Vector2(ColliderRect.size.x - DEVIATION*2, ColliderRect.size.y / 2);
            downInfo.pos = new Vector2(0,-downInfo.size.y/2);
            checkInfoMap.Add(ColliderDirType.Down,downInfo);
            
            ColliderCheckInfo leftInfo = new ColliderCheckInfo();
            leftInfo.dir = Vector2.left;
            leftInfo.size = new Vector2(ColliderRect.size.x / 2, ColliderRect.size.y - DEVIATION * 2);
            leftInfo.pos = new Vector2(-leftInfo.size.x / 2,0);
            checkInfoMap.Add(ColliderDirType.Left,leftInfo);
            
            ColliderCheckInfo rightInfo = new ColliderCheckInfo();
            rightInfo.dir = Vector2.right;
            rightInfo.size = new Vector2(ColliderRect.size.x / 2, ColliderRect.size.y - DEVIATION * 2);
            rightInfo.pos = new Vector2(rightInfo.size.x / 2,0);
            checkInfoMap.Add(ColliderDirType.Right,rightInfo);
        }
        
        public void Init()
        {
            
        }

        public void Clear()
        {
            
        }

        public Vector2 ColliderPos()
        {
            return Collider2D.transform.position.ToVector2() + ColliderRect.position;
        }

        #region Climb

        /// <summary>
        /// 爬墙吸附
        /// </summary>
        public void ClimbSnap()
        {
            Vector2 origion = ColliderPos();
            Vector2 dir = Vector2.right * (int)moveCom.CurrDir;
            RaycastHit2D hit = Physics2D.BoxCast(origion, ColliderRect.size, 0, dir, MainActorConst.ClimbCheckDist + DEVIATION, GroundMask);
            if (hit)
            {
                //如果发生碰撞,则移动距离
                moveCom.SetPos(moveCom.Pos + dir * Mathf.Max((hit.distance - DEVIATION), 0));
            }
        }

        #endregion

        #region Check

        /// <summary>
        /// 检测地面
        /// </summary>
        /// <returns></returns>
        public bool CheckGround()
        {
            return CheckGround(Vector2.zero);
        }
        
        /// <summary>
        /// 检测地面
        /// </summary>
        /// <param name="pOffset">偏移</param>
        /// <returns></returns>
        public bool CheckGround(Vector2 pOffset)
        {
            ColliderCheckInfo checkInfo = checkInfoMap[ColliderDirType.Down];
            Vector2 checkPos  = ColliderPos() + checkInfo.pos + pOffset;
            Vector2 checkSize = checkInfo.size;

            int hitCnt = Physics2DEx.BoxCastNonAllocDraw(checkPos, checkSize,Vector2.down, results, DEVIATION, GroundMask, Color.white, Color.red);
            
            if (hitCnt >0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检测水平方向墙壁跳跃
        /// </summary>
        /// <param name="pDir"></param>
        /// <returns></returns>
        public bool CheckWall(ActorDir pDir)
        {
            if (pDir == ActorDir.Left)
            {
                return CollideCheck(ColliderDirType.Left);
            }
            else if (pDir == ActorDir.Right)
            {
                return CollideCheck(ColliderDirType.Right);
            }
            return false;
        }
        
        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <param name="pDir">方向</param>
        /// <param name="pDist">偏移</param>
        /// <returns></returns>
        public bool CollideCheck(Vector2 pDir, float pDist = 0)
        {
            //return Physics2D.OverlapBox(ColliderPos() + pDir * (pDist + DEVIATION), ColliderRect.size, 0, GroundMask);
            
            return Physics2DEx.OverlapBoxDraw(ColliderPos() + pDir * (pDist + DEVIATION), ColliderRect.size, GroundMask);
        }

        public bool CollideCheck(ColliderDirType pDir, float pDist = 0)
        {
            ColliderCheckInfo checkInfo = checkInfoMap[pDir];

            Vector2 checkPos  = ColliderPos() + checkInfo.pos + checkInfo.dir * (pDist + DEVIATION);
            Vector2 checkSize = checkInfo.size;
            return Physics2DEx.OverlapBoxDraw(checkPos, checkSize, GroundMask);
        }

        /// <summary>
        /// 攀爬检查
        /// </summary>
        /// <param name="pDir">方向</param>
        /// <param name="pYAdd">向上的攀爬距离</param>
        /// <returns></returns>
        public bool ClimbCheck(int pDir, float pYAdd = 0)
        {
            //获取当前的碰撞体
            Vector2 origion = ColliderPos();
            if (Physics2D.OverlapBox(origion + Vector2.up * (float)pYAdd + Vector2.right * pDir * (MainActorConst.ClimbCheckDist + DEVIATION), ColliderRect.size, 0, GroundMask))
            {
                return true;
            } 
            return false;
        }


        /// <summary>
        /// 前上方检测
        /// </summary>
        /// <param name="pYAdd">向上的攀爬距离</param>
        /// <returns></returns>
        public bool ForwardUpCheck(float pAddY = 0)
        {
            int direct = moveCom.CurrDir == ActorDir.Right ? 1 : -1;
            
            Vector2 origin = ColliderPos() + Vector2.up * ColliderRect.size.y / 2f + Vector2.right * (direct * (ColliderRect.size.x / 2f + STEP));
            Vector2 point1 = origin + Vector2.up * (-0.4f + pAddY);

            if(Physics2DEx.OverlapPointDraw(point1,colResults, GroundMask) > 0)
            {
                return true;
            }
            Vector2 point2 = origin + Vector2.up * (0.4f + pAddY);
            if (Physics2DEx.OverlapPointDraw(point2,colResults, GroundMask) > 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region UpdatePos

        /// <summary>
        /// 尝试移动水平位置
        /// </summary>
        /// <param name="pDistX"></param>
        public void TryMovePosX(float pDistX)
        {
            float distance = pDistX;
            float moved = MoveXStepWithCollide(distance);
            
            //更新位置
            moveCom.SetPos(moveCom.Pos + Vector2.right * moved);

            //无碰撞
            if (moved == distance)
            {
                return;
            }
            else
            {
                moveCom.Speed.x = 0;
            }
            // if (expr)
            // {
            //     
            // }
            //
            // //修复速度
            // float tempDist = distance - moved;
            // if (!CorrectX(tempDist))
            // {
            //     //未完成校正，则速度清零
            //     moveCom.Speed.x = 0;
            //     return;
            // }
        }
        
        //尝试移动
        private float MoveXStepWithCollide(float pDistX)
        {
            Vector2 moved = Vector2.zero;
            Vector2 direct = Math.Sign(pDistX) > 0 ? Vector2.right : Vector2.left;
            Vector2 origion = ColliderPos();

            ColliderDirType checkType = Math.Sign(pDistX) > 0 ? ColliderDirType.Right : ColliderDirType.Left;
            ColliderCheckInfo checkInfo = checkInfoMap[checkType];
            Vector2 checkPos  = ColliderPos() + checkInfo.pos;
            Vector2 checkSize = checkInfo.size;
            
            int hitCnt = Physics2DEx.BoxCastNonAllocDraw(checkPos, checkSize, direct, results, Mathf.Abs(pDistX) + DEVIATION, GroundMask,Color.clear, Color.black);
            
            if (hitCnt>0)
            {
                Vector2 offset = direct * Mathf.Max((results[0].distance - DEVIATION), 0);
                //如果发生碰撞,则移动距离
                moved += offset;
                return moved.x;
            }
            else
            {
                moved += Vector2.right * pDistX;
            }
            return moved.x;
        }
        
        private bool CorrectX(float pDistX)
        {
            Vector2 origion = ColliderPos();
            Vector2 direct = Math.Sign(pDistX) > 0 ? Vector2.right : Vector2.left;

            // 冲刺修正
            // if ((this.stateMachine.State == (int)EActionState.Dash))
            // {
            //     if (onGround && DuckFreeAt(moveCom.Pos + Vector2.right * pDistX))
            //     {
            //         Ducking = true;
            //         return true;
            //     }
            //     else if (this.Speed.y == 0 && this.Speed.x!=0)
            //     {
            //         for (int i = 1; i <= Constants.DashCornerCorrection; i++)
            //         {
            //             for (int j = 1; j >= -1; j -= 2)
            //             {
            //                 if (!CollideCheck(this.moveCom.Pos + new Vector2(0, j * i * 0.1f), direct, Mathf.Abs(pDistX)))
            //                 {
            //                     this.moveCom.Pos += new Vector2(pDistX, j * i * 0.1f);
            //                     return true;
            //                 }
            //             }
            //         }
            //     }
            // }
            return false;
        }

        /// <summary>
        /// 尝试移动竖直位置
        /// </summary>
        /// <param name="pDistY"></param>
        public void TryMovePosY(float pDistY)
        {
            Vector2 targetPosition = this.moveCom.Pos;
            //使用校正
            float distance = pDistY;
            float speedY = Mathf.Abs(moveCom.Speed.y);
            float moved = MoveYStepWithCollide(distance);
            
            //无碰撞退出循环
            moveCom.SetPos(moveCom.Pos + Vector2.up * moved);
            if (moved == distance) //无碰撞，且校正次数为0
            {
                return;
            }
            
            float tempDist = distance - moved;
            if (!CorrectY(tempDist))
            {
                moveCom.Speed.y = 0;//未完成校正，则速度清零
                return;
            }
        }
        
        //单步移动，参数和返回值都带方向，表示Y轴
        private float MoveYStepWithCollide(float pDistY)
        {
            Vector2 moved = Vector2.zero;
            Vector2 direct = Math.Sign(pDistY) > 0 ? Vector2.up : Vector2.down;
            Vector2 origion = ColliderPos();
            
            ColliderDirType checkType = Math.Sign(pDistY) > 0 ? ColliderDirType.Up : ColliderDirType.Down;
            ColliderCheckInfo checkInfo = checkInfoMap[checkType];
            Vector2 checkPos  = ColliderPos() + checkInfo.pos;
            Vector2 checkSize = checkInfo.size;
            
            int hitCnt = Physics2DEx.BoxCastNonAllocDraw(checkPos, checkSize, direct, results, Mathf.Abs(pDistY) + DEVIATION, GroundMask,Color.clear, Color.black);
            
            if (hitCnt>0)
            {
                Vector2 offset = direct * Mathf.Max((results[0].distance - DEVIATION), 0);
                //如果发生碰撞,则移动距离
                moved += offset;
                return moved.y;
            }
            else
            {
                moved += Vector2.up * pDistY;
            }
            
            return moved.y;
        }
        
        private bool CorrectY(float distY)
        {
            Vector2 origion = ColliderPos();
            Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;
            
            if (moveCom.Speed.y < 0)
            {
                // if ((this.stateMachine.State == (int)EActionState.Dash) && !DashStartedOnGround)
                // {
                //     if (this.Speed.x <= 0)
                //     {
                //         for (int i = -1; i >= -Constants.DashCornerCorrection; i--)
                //         {
                //             float step = (Mathf.Abs(i * 0.1f) + DEVIATION);
                //             
                //             if (!CheckGround(new Vector2(-step, 0)))
                //             {
                //                 this.moveCom.Pos += new Vector2(-step, distY);
                //                 return true;
                //             }
                //         }
                //     }
                //
                //     if (this.Speed.x >= 0)
                //     {
                //         for (int i = 1; i <= Constants.DashCornerCorrection; i++)
                //         {
                //             float step = (Mathf.Abs(i * 0.1f) + DEVIATION);
                //             if (!CheckGround(new Vector2(step, 0)))
                //             {
                //                 this.moveCom.Pos += new Vector2(step, distY);
                //                 return true;
                //             }
                //         }
                //     }
                // }
            }
            //向上移动
            else if (moveCom.Speed.y > 0)
            {
                //左上方移动
                if (moveCom.Speed.x <= 0)
                {
                    for (int i = 1; i <= MainActorConst.UpwardCornerCorrection; i++)
                    {
                        RaycastHit2D hit = Physics2D.BoxCast(origion + new Vector2(-i * 0.1f, 0), ColliderRect.size, 0, direct, Mathf.Abs(distY) + DEVIATION, GroundMask);
                        if (!hit)
                        {
                            moveCom.SetPos(moveCom.Pos + new Vector2(-i * 0.1f, 0));
                            return true;
                        }
                    }
                }

                //右上方移动
                if (moveCom.Speed.x >= 0)
                {
                    for (int i = 1; i <= MainActorConst.UpwardCornerCorrection; i++)
                    {
                        RaycastHit2D hit = Physics2D.BoxCast(origion + new Vector2(i * 0.1f, 0), ColliderRect.size, 0, direct, Mathf.Abs(distY) + DEVIATION, GroundMask);
                        if (!hit)
                        {
                            moveCom.SetPos(moveCom.Pos + new Vector2(i * 0.1f, 0));
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        #endregion

    }
}