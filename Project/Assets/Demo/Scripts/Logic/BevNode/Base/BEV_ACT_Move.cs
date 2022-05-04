﻿using Demo.Com;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using UnityEngine;

namespace Demo.BevNode
{
    /// <summary>
    /// 移动类型
    /// </summary>
    public enum MoveType
    {
        None,               //静止
        Run,                //跑
        Jump,               //跳
        Climb,              //爬墙
        ClimbJump,          //爬墙跳
        GrabWall,           //抓墙
    }

    /// <summary>
    /// 移动行为
    /// </summary>
    public class BEV_ACT_Move : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            //参数
            Vector2 inputMove = workData.GetParam().GetVect2();

            //组件
            MoveCom moveCom             = workData.MEntity.GetCom<MoveCom>();
            Collider2DCom collider2DCom = workData.MEntity.GetCom<Collider2DCom>();
            PropertyCom propertyCom     = workData.MEntity.GetCom<PropertyCom>();

            //跳跃阶段重置
            ResetJumpStep(moveCom, collider2DCom);

            if (CheckMoveTypeInCD(moveCom))
            {
                moveCom.HasNoReqMove = true;
                return;
            }
            else
            {
                moveCom.HasNoReqMove = false;
            }

            MoveType moveType = CalcMoveType(collider2DCom, moveCom, inputMove);
            switch (moveType)
            {
                case MoveType.None:
                    moveCom.ReqMoveSpeed = 0;
                    moveCom.ReqJumpSpeed = 0;
                    break;
                case MoveType.Run:
                    moveCom.ReqMoveSpeed = propertyCom.MoveSpeed.Curr * inputMove.x;
                    moveCom.ReqJumpSpeed = 0;
                    break;
                case MoveType.Jump:
                    moveCom.JumpStep ++;
                    moveCom.ReqMoveSpeed = 0;
                    moveCom.ReqJumpSpeed = propertyCom.JumpSpeed.Curr * inputMove.y;
                    break;
                case MoveType.Climb:
                    inputMove.x = inputMove.x < 0 ? -inputMove.x : inputMove.x;
                    moveCom.ReqMoveSpeed = 0;
                    moveCom.ReqJumpSpeed = propertyCom.ClimbSpeed.Curr * inputMove.x;
                    break;
                case MoveType.ClimbJump:
                    int moveValue = inputMove.x < 0 ? -1 : 1;
                    moveCom.ReqMoveSpeed = moveValue * 2;
                    moveCom.ReqJumpSpeed = propertyCom.ClimbSpeed.Curr;
                    break;
                case MoveType.GrabWall:
                    moveCom.ReqMoveSpeed = 0;
                    moveCom.ReqJumpSpeed = 0;
                    break;
                default:
                    break;
            }

            HandleMass(moveType, propertyCom);
            ClampMoveSpeed(moveCom, collider2DCom);
            HandleMoveTypeCD(moveType, moveCom);

            //记录状态
            moveCom.CurrMoveType = moveType;
            if (inputMove.y!=0)
            {
                Debug.LogWarningFormat("BEV_ACT_Move>>>>>{0}--ReqMoveSpeed：{1}--ReqJumpSpeed：{2}", moveType.ToString(), moveCom.ReqMoveSpeed, moveCom.ReqJumpSpeed);
            }
            
        }

        //重置跳跃阶段
        private void ResetJumpStep(MoveCom moveCom, Collider2DCom collider2DCom)
        {
            if (!collider2DCom.Collider.Down)
                return;
            moveCom.JumpStep = 0;
        }

        #region 判断移动状态

        private MoveType CalcMoveType(Collider2DCom collider2DCom, MoveCom moveCom, Vector2 inputMove)
        {
            //没有输入
            if (inputMove == Vector2.zero)
                return MoveType.None;

            if (CheckClimb(collider2DCom, inputMove))
                return MoveType.Climb;

            if (CheckClimbJump(collider2DCom, inputMove))
            {
                return MoveType.ClimbJump;
            }
            else
            {
                if (CheckGrabWall(collider2DCom, inputMove))
                    return MoveType.GrabWall;
            }

            if (CheckCanJump(moveCom, inputMove))
                return MoveType.Jump;

            return MoveType.Run;
        }

        private bool CheckClimb(Collider2DCom collider2DCom, Vector2 inputMove)
        {
            if (inputMove.y != 0)
            {
                return false;
            }

            if (!collider2DCom.Collider.Left && !collider2DCom.Collider.Right)
            {
                return false;
            }

            if (inputMove.x < 0 && collider2DCom.Collider.Right)
            {
                return false;
            }

            if (inputMove.x > 0 && collider2DCom.Collider.Left)
            {
                return false;
            }

            return true;
        }

        private bool CheckClimbJump(Collider2DCom collider2DCom, Vector2 inputMove)
        {
            if (collider2DCom.Collider.Down)
            {
                return false;
            }

            if (inputMove.y == 0)
                return false;

            if (collider2DCom.Collider.Left)
            {
                //右横跳
                if (inputMove.x > 0)
                {
                    return true;
                }
            }

            if (collider2DCom.Collider.Right)
            {
                //左横跳
                if (inputMove.x < 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckGrabWall(Collider2DCom collider2DCom, Vector2 inputMove)
        {
            if (collider2DCom.Collider.Down)
                return false;

            if (!collider2DCom.Collider.Right && !collider2DCom.Collider.Left)
                return false;

            if (inputMove.y != 0)
                return false;

            if (collider2DCom.Collider.Left)
            {
                //右横跳
                if (inputMove.x > 0)
                {
                    return true;
                }
            }

            if (collider2DCom.Collider.Right)
            {
                //左横跳
                if (inputMove.x < 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckCanJump(MoveCom moveCom,Vector2 inputMove)
        {
            if (inputMove.y == 0)
                return false;
            if (moveCom.JumpStep < 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 速度限制

        private void ClampMoveSpeed(MoveCom moveCom, Collider2DCom collider2DCom)
        {
            if (collider2DCom.Collider.Left)
            {
                if (moveCom.ReqMoveSpeed < 0)
                {
                    moveCom.ReqMoveSpeed = 0;
                }
            }
            if (collider2DCom.Collider.Right)
            {
                if (moveCom.ReqMoveSpeed > 0)
                {
                    moveCom.ReqMoveSpeed = 0;
                }
            }
        }

        #endregion

        #region 移动冷却

        private bool CheckMoveTypeInCD(MoveCom moveCom)
        {
            if (Time.realtimeSinceStartup > moveCom.MoveTypeCd)
            {
                moveCom.MoveTypeCd = 0;
                return false;
            }
            return true;
        }

        private void HandleMoveTypeCD(MoveType moveType,MoveCom moveCom)
        {
            if (moveType == MoveType.ClimbJump)
            {
                moveCom.MoveTypeCd = Time.realtimeSinceStartup + 1.5f;
            }
        }

        #endregion

        #region 处理重力

        private void HandleMass(MoveType moveType, PropertyCom propertyCom)
        {
            switch (moveType)
            {
                case MoveType.None:
                case MoveType.Run:
                case MoveType.Jump:
                    propertyCom.Mass = 1;
                    break;
                case MoveType.Climb:
                case MoveType.ClimbJump:
                case MoveType.GrabWall:
                    propertyCom.Mass = 0.6f;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}