using Demo.Com;
using LCECS.Core;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using UnityEngine;

namespace Demo.Behavior
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
    public class BEV_ACT_PlayerMove : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            
            //参数
            Vector2 inputMove = workData.GetParam<RequestInputMove>().inputMove;
            
            //组件
            TransCom transCom = workData.MEntity.GetCom<TransCom>();
            PlayerCom playerCom = workData.MEntity.GetCom<PlayerCom>();
            MoveCom moveCom = workData.MEntity.GetCom<MoveCom>();
            BasePropertyCom propertyCom = workData.MEntity.GetCom<BasePropertyCom>();
            Collider2DCom collider2DCom = workData.MEntity.GetCom<Collider2DCom>();
            
            //重置跳跃记录
            if (collider2DCom.Collider.Down)
            {
                playerCom.JumpStep.Curr = 0;
            }
            
            //X
            moveCom.Move(propertyCom.MoveSpeed.Curr * inputMove.x);
            
            //Y
            if (inputMove.y > 0)
            {
                playerCom.JumpStep.Curr++;
                if (!playerCom.JumpStep.CheckOutTotal())
                {
                    moveCom.Jump(500);
                }
            }
        }

        //protected override void OnEnter(NodeData wData)
        //{
        //    EntityWorkData workData = wData as EntityWorkData;

        //    //参数
        //    Vector2 inputMove = workData.GetParam().GetVect2();

        //    //组件
        //    PlayerMoveCom moveCom       = workData.MEntity.GetCom<PlayerMoveCom>();
        //    TransCom transCom       = workData.MEntity.GetCom<TransCom>();    
        //    Collider2DCom collider2DCom = workData.MEntity.GetCom<Collider2DCom>();
        //    BasePropertyCom propertyCom     = workData.MEntity.GetCom<BasePropertyCom>();
        //    PlayerPropertyCom playerPropertyCom     = workData.MEntity.GetCom<PlayerPropertyCom>();

        //    //跳跃阶段重置
        //    ResetJumpStep(moveCom, collider2DCom);

        //    if (CheckMoveTypeInCD(moveCom))
        //    {
        //        moveCom.HasNoReqMove = true;
        //        return;
        //    }
        //    else
        //    {
        //        moveCom.HasNoReqMove = false;
        //    }

        //    MoveType moveType = CalcMoveType(collider2DCom, moveCom, inputMove);
        //    switch (moveType)
        //    {
        //        case MoveType.None:
        //            moveCom.ReqMoveSpeed = 0;
        //            moveCom.ReqJumpSpeed = 0;
        //            break;
        //        case MoveType.Run:
        //            moveCom.ReqMoveSpeed = propertyCom.MoveSpeed.Curr * inputMove.x;
        //            moveCom.ReqJumpSpeed = 0;
        //            break;
        //        case MoveType.Jump:
        //            moveCom.JumpStep ++;
        //            moveCom.ReqMoveSpeed = 0;
        //            moveCom.ReqJumpSpeed = playerPropertyCom.JumpSpeed.Curr * inputMove.y;
        //            break;
        //        case MoveType.Climb:
        //            inputMove.x = inputMove.x < 0 ? -inputMove.x : inputMove.x;
        //            moveCom.ReqMoveSpeed = 0;
        //            moveCom.ReqJumpSpeed = playerPropertyCom.ClimbSpeed.Curr * inputMove.x;
        //            break;
        //        case MoveType.ClimbJump:
        //            int moveValue = inputMove.x < 0 ? -1 : 1;
        //            moveCom.ReqMoveSpeed = propertyCom.MoveSpeed.Curr * moveValue;
        //            moveCom.ReqJumpSpeed = playerPropertyCom.ClimbSpeed.Curr;
        //            break;
        //        case MoveType.GrabWall:
        //            moveCom.ReqMoveSpeed = 0;
        //            moveCom.ReqJumpSpeed = 0;
        //            break;
        //        default:
        //            break;
        //    }

        //    ClampMoveSpeed(moveCom, collider2DCom);
        //    HandleMoveTypeCD(moveType, moveCom);
        //    HandleMoveDir(moveCom, transCom);

        //    //记录状态
        //    moveCom.CurrMoveType = moveType;
        //    if (inputMove.y!=0)
        //    {
        //        Debug.LogWarningFormat("BEV_ACT_Move>>>>>{0}--ReqMoveSpeed：{1}--ReqJumpSpeed：{2}", moveType.ToString(), moveCom.ReqMoveSpeed, moveCom.ReqJumpSpeed);
        //    }
        //}

        #region Old
        //重置跳跃阶段
        private void ResetJumpStep(PlayerMoveCom moveCom, Collider2DCom collider2DCom)
        {
            if (!collider2DCom.Collider.Down)
                return;
            moveCom.JumpStep = 0;
        }

        #region 判断移动状态

        private MoveType CalcMoveType(Collider2DCom collider2DCom, PlayerMoveCom moveCom, Vector2 inputMove)
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

        private bool CheckCanJump(PlayerMoveCom moveCom, Vector2 inputMove)
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

        private void ClampMoveSpeed(PlayerMoveCom moveCom, Collider2DCom collider2DCom)
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

        private bool CheckMoveTypeInCD(PlayerMoveCom moveCom)
        {
            if (Time.realtimeSinceStartup > moveCom.MoveTypeCd)
            {
                moveCom.MoveTypeCd = 0;
                return false;
            }
            return true;
        }

        private void HandleMoveTypeCD(MoveType moveType, PlayerMoveCom moveCom)
        {
            if (moveType == MoveType.ClimbJump)
            {
                moveCom.MoveTypeCd = Time.realtimeSinceStartup + 0.2f;
            }
        }

        #endregion

        #region 方向

        private void HandleMoveDir(PlayerMoveCom moveCom, TransCom transCom)
        {
            if (moveCom.ReqMoveSpeed == 0)
                return;
            DirType dirType = moveCom.ReqMoveSpeed > 0 ? DirType.Right : DirType.Left;
            transCom.Roate(dirType);
        }

        #endregion 
        #endregion
    }
}